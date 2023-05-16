namespace Sundew.Testing.CodeAnalysis;

using Microsoft.CodeAnalysis;

public class AssemblyReference : IReference
{
    private readonly MetadataReference metadataReference;

    public AssemblyReference(string path)
    {
        this.metadataReference = MetadataReference.CreateFromFile(path, MetadataReferenceProperties.Assembly);
    }

    public MetadataReference GetMetadataReference()
    {
        return this.metadataReference;
    }
}