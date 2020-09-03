﻿using System;

namespace NakedFunctions
{
    /// <summary>
    ///     Tell Nakedobjects that this class, property or function will never be displayed. It will not be introspected, either.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, Inherited = true, AllowMultiple = false)]
        public class BoundedAttribute : Attribute { }
}
