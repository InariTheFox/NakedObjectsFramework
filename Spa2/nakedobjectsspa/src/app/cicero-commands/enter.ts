import { CommandResult } from './command-result';
import { Command } from './Command';
import * as Models from '../models';
import * as Usermessages from '../user-messages';
import * as Constants from '../constants';
import { Dictionary } from 'lodash';
import map from 'lodash/map';
import forEach from 'lodash/forEach';
import reduce from 'lodash/reduce';
import keys from 'lodash/keys';
import mapValues from 'lodash/mapValues';
import mapKeys from 'lodash/mapKeys';
import fromPairs from 'lodash/fromPairs';
import * as moment from 'moment';
import * as Commandresult from './command-result';

export class Enter extends Command {

    shortCommand = "en";
    fullCommand = Usermessages.enterCommand;
    helpText = Usermessages.enterHelp;
    protected minArguments = 2;
    protected maxArguments = 2;

    isAvailableInCurrentContext(): boolean {
        return this.isDialog() || this.isEdit() || this.isTransient() || this.isForm();
    }

    doExecute(args: string, chained: boolean): Promise<CommandResult> {
        const fieldName = this.argumentAsString(args, 0);
        const fieldEntry = this.argumentAsString(args, 1, false, false);
        if (this.isDialog()) {
            return this.fieldEntryForDialog(fieldName, fieldEntry);
        } else {
            return this.fieldEntryForEdit(fieldName, fieldEntry);
        }
    };

    private fieldEntryForEdit(fieldName: string, fieldEntry: string) {
        return this.getObject().then(obj => {
            const fields = this.matchingProperties(obj, fieldName);

            switch (fields.length) {
                case 0:
                    const s = Usermessages.doesNotMatchProperties(fieldName);
                    return this.returnResult("", s);
                case 1:
                    const field = fields[0];
                    if (fieldEntry === "?") {
                        //TODO: does this work in edit mode i.e. show entered value
                        const s = this.renderFieldDetails(field, field.value());
                        return this.returnResult("", s);
                    } else {
                        this.findAndClearAnyDependentFields(field.id(), obj.propertyMembers());
                        return this.setField(field, fieldEntry);
                    }
                default:
                    const ss = reduce(fields, (s, prop) => s + prop.extensions().friendlyName() + "\n", `${fieldName} ${Usermessages.matchesMultiple}`);
                    return this.returnResult("", ss);
            }
        });
    }

    private findAndClearAnyDependentFields(changingField: string, allFields: Dictionary<Models.IField>) {

        forEach(allFields,
            field => {
                const promptLink = field.promptLink();

                if (promptLink) {
                    const pArgs = promptLink.arguments();
                    const argNames = keys(pArgs);

                    if (argNames.indexOf(changingField.toLowerCase()) >= 0) {
                        this.clearField(field);
                    }
                }
            });
    }

    private fieldEntryForDialog(fieldName: string, fieldEntry: string) {
        return this.getActionForCurrentDialog().then(action => {
            //TODO: error -  need to get invokable action to get the params.
            let params = map(action.parameters(), param => param);
            params = this.matchFriendlyNameAndOrMenuPath(params, fieldName);
            switch (params.length) {
                case 0:
                    return this.returnResult("", Usermessages.doesNotMatchDialog(fieldName));
                case 1:
                    if (fieldEntry === "?") {
                        const p = params[0];
                        const value = Commandresult.getParametersAndCurrentValue(p.parent, this.context)[p.id()];
                        const s = this.renderFieldDetails(p, value);
                        return this.returnResult("", s);
                    } else {
                        this.findAndClearAnyDependentFields(fieldName, action.parameters());
                        return this.setField(params[0], fieldEntry);
                    }
                default:
                    return this.returnResult("", `${Usermessages.multipleFieldMatches} ${fieldName}`);//TODO: list them
            }
        });
    }

    private clearField(field: Models.IField): void {
        this.context.cacheFieldValue(this.routeData().dialogId, field.id(), new Models.Value(null));

        if (field instanceof Models.Parameter) {
            this.context.cacheFieldValue(this.routeData().dialogId, field.id(), new Models.Value(null));
        } else if (field instanceof Models.PropertyMember) {
            const parent = field.parent as Models.DomainObjectRepresentation;
            this.context.cachePropertyValue(parent, field, new Models.Value(null));
        }
    }

    private setField(field: Models.IField, fieldEntry: string) {
        if (field instanceof Models.PropertyMember && field.disabledReason()) {
            return this.returnResult("", `${field.extensions().friendlyName()} ${Usermessages.isNotModifiable}`);
        }
        const entryType = field.entryType();
        switch (entryType) {
            case Models.EntryType.FreeForm:
                return this.handleFreeForm(field, fieldEntry);
            case Models.EntryType.AutoComplete:
                return this.handleAutoComplete(field, fieldEntry);

            case Models.EntryType.Choices:
                return this.handleChoices(field, fieldEntry);

            case Models.EntryType.MultipleChoices:
                return this.handleChoices(field, fieldEntry);

            case Models.EntryType.ConditionalChoices:
                return this.handleConditionalChoices(field, fieldEntry);

            case Models.EntryType.MultipleConditionalChoices:
                return this.handleConditionalChoices(field, fieldEntry);

            default:

                return this.returnResult("", Usermessages.invalidCase);
        }
    }

    private handleFreeForm(field: Models.IField, fieldEntry: string) {
        if (field.isScalar()) {
            let value: Models.Value = new Models.Value(fieldEntry);
            //TODO: handle a non-parsable date
            if (Models.isDateOrDateTime(field)) {
                const m = moment(fieldEntry, Constants.supportedDateFormats, "en-GB", true); //TODO get actual locale

                if (m.isValid()) {
                    const dt = m.toDate();
                    value = new Models.Value(Models.toDateString(dt));
                }
            }
            this.setFieldValue(field, value);
            return this.returnResult("", "", () => this.urlManager.triggerPageReloadByFlippingReloadFlagInUrl());
        } else {
            return this.handleReferenceField(field, fieldEntry);
        }
    }

    private setFieldValue(field: Models.IField, value: Models.Value): void {
        const urlVal = this.valueForUrl(value, field);
        if (field instanceof Models.Parameter) {
            this.setFieldValueInContextAndUrl(field, urlVal);
        } else if (field instanceof Models.PropertyMember) {
            const parent = field.parent;
            if (parent instanceof Models.DomainObjectRepresentation) {
                this.setPropertyValueinContextAndUrl(parent, field, urlVal);
            }
        }
    }

    private handleReferenceField(field: Models.IField, fieldEntry: string) {
        if (this.isPaste(fieldEntry)) {
            return this.handleClipboard(field);
        } else {
            return this.returnResult("", Usermessages.invalidRefEntry);
        }
    }

    private isPaste(fieldEntry: string) {
        return "paste".indexOf(fieldEntry) === 0;
    }

    private handleClipboard(field: Models.IField) {
        const ref = this.ciceroContext.ciceroClipboard;
        if (!ref) {

            return this.returnResult("", Usermessages.emptyClipboard);
        }
        const paramType = field.extensions().returnType();
        const refType = ref.domainType();
        return this.context.isSubTypeOf(refType, paramType).then(isSubType => {
            if (isSubType) {
                const obj = this.ciceroContext.ciceroClipboard as any;
                const selfLink = obj.selfLink();
                //Need to add a title to the SelfLink as not there by default
                selfLink.setTitle(obj.title());
                const value = new Models.Value(selfLink);
                this.setFieldValue(field, value);

                return this.returnResult("", "", () => this.urlManager.triggerPageReloadByFlippingReloadFlagInUrl());
            } else {

                return this.returnResult("", Usermessages.incompatibleClipboard);
            }
        });
    }

    private handleAutoComplete(field: Models.IField, fieldEntry: string) {
        //TODO: Need to check that the minimum number of characters has been entered or fail validation
        if (!field.isScalar() && this.isPaste(fieldEntry)) {
            return this.handleClipboard(field);
        } else {
            return this.context.autoComplete(field, field.id(), null, fieldEntry).then(choices => {
                const matches = this.findMatchingChoicesForRef(choices, fieldEntry);
                return this.switchOnMatches(field, fieldEntry, matches);
            });
        }
    }

    private handleChoices(field: Models.IField, fieldEntry: string) {
        let matches: Models.Value[];
        if (field.isScalar()) {
            matches = this.findMatchingChoicesForScalar(field.choices(), fieldEntry);
        } else {
            matches = this.findMatchingChoicesForRef(field.choices(), fieldEntry);
        }
        return this.switchOnMatches(field, fieldEntry, matches);
    }

    private switchOnMatches(field: Models.IField, fieldEntry: string, matches: Models.Value[]) {
        switch (matches.length) {
            case 0:
                return this.returnResult("", Usermessages.noMatch(fieldEntry));
            case 1:
                this.setFieldValue(field, matches[0]);
                return this.returnResult("", "", () => this.urlManager.triggerPageReloadByFlippingReloadFlagInUrl());
            default:
                let msg = Usermessages.multipleMatches;
                forEach(matches, m => msg += m.toString() + "\n");
                return this.returnResult("", msg);
        }
    }

    private getPropertiesAndCurrentValue(obj: Models.DomainObjectRepresentation): Dictionary<Models.Value> {
        const props = obj.propertyMembers();
        const values = mapValues(props, p => p.value());
        const modifiedProps = this.context.getObjectCachedValues(obj.id());

        forEach(values,
            (v, k) => {
                const newValue = modifiedProps[k];
                if (newValue) {
                    values[k] = newValue;
                }
            });
        return mapKeys(values, (v, k) => k.toLowerCase());
    }

    private handleConditionalChoices(field: Models.IField, fieldEntry: string) {
        let enteredFields: Dictionary<Models.Value>;

        if (field instanceof Models.Parameter) {
            enteredFields = Commandresult.getParametersAndCurrentValue(field.parent, this.context);
        }

        if (field instanceof Models.PropertyMember) {
            enteredFields = this.getPropertiesAndCurrentValue(field.parent as Models.DomainObjectRepresentation);
        }

        const args = fromPairs(map(field.promptLink().arguments(), (v: any, key: string) => [key, new Models.Value(v.value)])) as Dictionary<Models.Value>;
        forEach(keys(args), key => args[key] = enteredFields[key]);

        return this.context.conditionalChoices(field, field.id(), null, args).then(choices => {
            const matches = this.findMatchingChoicesForRef(choices, fieldEntry);
            return this.switchOnMatches(field, fieldEntry, matches);
        });
    }

    private renderFieldDetails(field: Models.IField, value: Models.Value): string {
        let s = Usermessages.fieldName(field.extensions().friendlyName());
        const desc = field.extensions().description();
        s += desc ? `\n${Usermessages.descriptionFieldPrefix} ${desc}` : "";
        s += `\n${Usermessages.typePrefix} ${Models.friendlyTypeName(field.extensions().returnType())}`;
        if (field instanceof Models.PropertyMember && field.disabledReason()) {
            s += `\n${Usermessages.unModifiablePrefix(field.disabledReason())}`;
        } else {
            s += field.extensions().optional() ? `\n${Usermessages.optional}` : `\n${Usermessages.mandatory}`;
            if (field.choices()) {
                const label = `\n${Usermessages.choices}: `;
                s += reduce(field.choices(), (s, cho) => s + cho + " ", label);
            }
        }
        return s;
    }
}