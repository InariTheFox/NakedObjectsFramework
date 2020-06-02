// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using System;
using NakedObjects.Resources;

namespace NakedObjects.Core {
    /// <summary>
    ///     Indicates that a value entry is not valid. The entry may still parse correctly,
    ///     but it does not fulfill other other entry requirements
    /// </summary>
    public class InvalidEntryException : NakedObjectApplicationException {
        public InvalidEntryException(string message)
            : base(message) { }

        public InvalidEntryException(string message, Exception cause)
            : base(message, cause) { }
    }
}