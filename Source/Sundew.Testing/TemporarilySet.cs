// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemporarilySet.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing
{
    using System;

    /// <summary>
    /// Provides factory methods for creating instances of the <see cref="TemporarilySet{TValue}"/> class, which manage the temporary
    /// assignment and restoration of a value.
    /// </summary>
    /// <remarks>This class is intended to simplify scenarios where a value must be set temporarily and then
    /// restored to its original state, such as when modifying global or static settings within a scoped context. All
    /// members are static and thread safety depends on the usage of the provided set and get functions.</remarks>
    public static class TemporarilySet
    {
        /// <summary>
        /// Creates a new instance of the <see cref="TemporarilySet{TValue}"/> class, setting a value temporarily and restoring the original value when disposed.
        /// </summary>
        /// <remarks>Use this method to temporarily change a value within a scope, ensuring the original value is restored automatically. This is useful for scenarios where a value must be set for a limited time, such as during a test or a specific operation.</remarks>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="temporaryValue">The value to set temporarily for the duration of the <see cref="TemporarilySet{TValue}"/>  instance.</param>
        /// <param name="setValueFunc">An action that sets the value. This delegate is called to apply the temporary value and to restore the original value when the instance is disposed.</param>
        /// <param name="getValueFunc">A function that retrieves the current value before it is temporarily changed. This is used to restore the original value when the instance is disposed.</param>
        /// <returns>A <see cref="TemporarilySet{TValue}"/>  instance.</returns>
        public static TemporarilySet<TValue> Create<TValue>(TValue temporaryValue, Action<TValue> setValueFunc, Func<TValue> getValueFunc)
        {
            return new TemporarilySet<TValue>(temporaryValue, setValueFunc, getValueFunc);
        }
    }
}
