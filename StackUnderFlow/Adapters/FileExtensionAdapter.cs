using StackUnderFlow.Model;

namespace StackUnderFlow.Adapters;

public class FileExtensionAdapter
{
    public static AcceptedLanguage DetermineFileType(string extension)
    {
        return extension switch
        {
            "py" => AcceptedLanguage.Python,
            "cs" => AcceptedLanguage.Csharp,
            _ => throw new ArgumentOutOfRangeException(nameof(extension), extension, null)
        };
    }
}