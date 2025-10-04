// --------------------------------------------------------------------------------------------------------------------
// <copyright file="TemporarilySet{TValue}.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing
{
    using System;

    /// <summary>
    /// Temporary sets a value and restores the original value when disposed.
    /// </summary>
    /// <typeparam name="TValue">The value type.</typeparam>
    public sealed class TemporarilySet<TValue> : IDisposable
    {
        private readonly TValue originalValue;
        private readonly Func<TValue> getValueFunc;
        private readonly Action<TValue> setValueFunc;

        /// <summary>
        /// Initializes a new instance of the <see cref="TemporarilySet{TValue}"/> class.
        /// </summary>
        /// <remarks>Use this constructor to temporarily change a value within a scope. The original value
        /// is restored when the instance is disposed. This is useful for scenarios where a value must be set
        /// temporarily and reliably reverted, such as in testing or scoped configuration changes.</remarks>
        /// <param name="temporaryValue">The value to set temporarily for the duration of the instance.</param>
        /// <param name="setValueFunc">A delegate that sets the value. Cannot be null.</param>
        /// <param name="getValueFunc">A delegate that retrieves the current value. Cannot be null.</param>
        internal TemporarilySet(TValue temporaryValue, Action<TValue> setValueFunc, Func<TValue> getValueFunc)
        {
            this.getValueFunc = getValueFunc;
            this.setValueFunc = setValueFunc;
            this.originalValue = this.getValueFunc();
            this.setValueFunc(temporaryValue);
        }

        /// <summary>
        /// Resets the value to the original value when disposed.
        /// </summary>
        public void Dispose()
        {
            this.setValueFunc(this.originalValue);
        }
    }
}
