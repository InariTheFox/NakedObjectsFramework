// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using Common.Logging;
using NakedObjects.Architecture.Component;
using NakedObjects.Architecture.Transaction;
using NakedObjects.Core.Transaction;
using NakedObjects.Core.Util;

namespace NakedObjects.Core.Component {
    public sealed class TransactionManager : ITransactionManager {
        private static readonly ILog Log = LogManager.GetLogger(typeof(TransactionManager));
        private readonly IObjectStore objectStore;
        private ITransaction transaction;
        private bool userAborted;

        public TransactionManager(IObjectStore objectStore) {
            Assert.AssertNotNull(objectStore);
            this.objectStore = objectStore;
        }

        private ITransaction Transaction => transaction ?? new NestedTransaction(objectStore);

        #region ITransactionManager Members

        public void StartTransaction() {
            if (transaction == null) {
                transaction = new NestedTransaction(objectStore);
                TransactionLevel = 0;
                userAborted = false;
                objectStore.StartTransaction();
            }

            TransactionLevel++;
        }

        public void AbortTransaction() {
            if (transaction != null) {
                transaction.Abort();
                transaction = null;
                TransactionLevel = 0;
                objectStore.AbortTransaction();
            }
        }

        public void UserAbortTransaction() {
            AbortTransaction();
            userAborted = true;
        }

        public void EndTransaction() {
            TransactionLevel--;
            if (TransactionLevel == 0) {
                Transaction.Commit();
                transaction = null;
            }
            else if (TransactionLevel < 0) {
                TransactionLevel = 0;
                if (!userAborted) {
                    throw new TransactionException(Log.LogAndReturn("No transaction running to end"));
                }
            }
        }

        public int TransactionLevel { get; private set; }

        #endregion
    }

    // Copyright (c) Naked Objects Group Ltd.
}