﻿// Copyright Naked Objects Group Ltd, 45 Station Road, Henley on Thames, UK, RG9 1AT
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0.
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and limitations under the License.

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace NakedObjects.SystemTest.Util {
    [TestClass]
    public class ReasonBuilderTest {
        private ReasonBuilder builder;

        [TestInitialize]
        public void SetUp() {
            builder = new ReasonBuilder();
        }

        private void AssertMessageIs(string expected) {
            Assert.AreEqual(expected, builder.Reason);
        }

        [TestMethod]
        public void WithNothingAppendedReasonReturnsNull() {
            Assert.IsNull(builder.Reason);
            builder.Append("Reason 1");
            Assert.IsNotNull(builder.Reason);
            AssertMessageIs("Reason 1");
        }

        [TestMethod]
        public void Append() {
            builder.Append("Reason 1");
            AssertMessageIs("Reason 1");
            builder.Append("Reason 2");
            AssertMessageIs("Reason 1; Reason 2");
            builder.Append("Reason 3");
            AssertMessageIs("Reason 1; Reason 2; Reason 3");
        }

        [TestMethod]
        public void AppendOnCondition() {
            builder.AppendOnCondition(false, "Reason 1");
            Assert.IsNull(builder.Reason);
            builder.AppendOnCondition(true, "Reason 2");
            AssertMessageIs("Reason 2");
            builder.AppendOnCondition(false, "Reason 3");
            AssertMessageIs("Reason 2");
            builder.AppendOnCondition(true, "Reason 4");
            AssertMessageIs("Reason 2; Reason 4");
        }
    }
}