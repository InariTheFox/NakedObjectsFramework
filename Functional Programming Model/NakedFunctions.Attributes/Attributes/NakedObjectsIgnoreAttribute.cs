﻿using System;

namespace NakedFunctions
{
        [AttributeUsage(AttributeTargets.Property | AttributeTargets.Method)]
        public class NakedObjectsIgnoreAttribute : Attribute { }
}
