// --------------------------------------------------------------------------------------------------------------------
// <copyright file="DictionaryAnalyzerConfigOptions.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using System.Collections.Generic;
using Microsoft.CodeAnalysis.Diagnostics;

/// <summary>
/// Provides analyzer configuration options backed by a dictionary of key-value pairs.
/// </summary>
/// <remarks>This class enables retrieval of analyzer configuration values using string keys. It is typically used
/// to supply configuration data to analyzers in a format compatible with the Roslyn infrastructure. The options are
/// case-sensitive and are not thread-safe for concurrent modifications.</remarks>
public class DictionaryAnalyzerConfigOptions : AnalyzerConfigOptions
{
    private readonly Dictionary<string, string> options;

    /// <summary>
    /// Initializes a new instance of the <see cref="DictionaryAnalyzerConfigOptions"/> class.
    /// </summary>
    /// <param name="options">A dictionary containing key-value pairs that define configuration options for the analyzer. Cannot be null.</param>
    public DictionaryAnalyzerConfigOptions(Dictionary<string, string> options)
    {
        this.options = options;
    }

    /// <summary>
    /// Attempts to retrieve the value associated with the specified key from the collection.
    /// </summary>
    /// <param name="key">The key whose value to retrieve. Cannot be null.</param>
    /// <param name="value">When this method returns, contains the value associated with the specified key, if the key is found; otherwise,
    /// the default value for the type.</param>
    /// <returns>true if the collection contains an element with the specified key; otherwise, false.</returns>
    public override bool TryGetValue(string key, out string value)
    {
        return this.options.TryGetValue(key, out value);
    }
}