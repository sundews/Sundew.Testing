﻿// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IReference.cs" company="Sundews">
// Copyright (c) Sundews. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace Sundew.Testing.CodeAnalysis;

using Microsoft.CodeAnalysis;

public interface IReference
{
    MetadataReference GetMetadataReference();
}