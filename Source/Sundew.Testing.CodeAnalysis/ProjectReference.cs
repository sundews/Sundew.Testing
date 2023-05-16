namespace Sundew.Testing.CodeAnalysis;

using System;
using Microsoft.CodeAnalysis;

public class ProjectReference : IReference
{
    private readonly Lazy<MetadataReference> metadataReference;

    public ProjectReference(IProject project)
    {
        this.metadataReference = new Lazy<MetadataReference>(() => project.Compile().ToMetadataReference());
    }


    public MetadataReference GetMetadataReference()
    {
        return this.metadataReference.Value;
    }
}