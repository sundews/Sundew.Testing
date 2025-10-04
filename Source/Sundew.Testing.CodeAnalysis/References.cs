// --------------------------------------------------------------------------------------------------------------------
// <copyright file="References.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

/// <summary>
/// Represents an immutable collection of references used within the application.
/// </summary>
/// <param name="ReferenceList">An array of <see cref="IReference"/> objects to include in the collection. Cannot be null.</param>
public sealed record References(params IReference[] ReferenceList);