﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemporarilySet.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing
{
    using System;

    public static class TemporarilySet
    {
        public static TemporarilySet<TValue> Create<TValue>(TValue temporaryValue, Action<TValue> setValueFunc, Func<TValue> getValueFunc)
        {
            return new TemporarilySet<TValue>(temporaryValue, setValueFunc, getValueFunc);
        }
    }
}
