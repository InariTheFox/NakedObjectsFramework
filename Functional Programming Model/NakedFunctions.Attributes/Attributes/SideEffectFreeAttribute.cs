﻿using System;

namespace NakedFunctions
{
    [AttributeUsage(AttributeTargets.Method, Inherited = true, AllowMultiple = false)]
    public class SideEffectFreeAttribute : Attribute { }
}