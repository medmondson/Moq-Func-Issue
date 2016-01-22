namespace MoqIssue
{
    public interface ICsvConversionProcessParameter
    {
        string OutputFolderPath { get; }
        string InputFolderPath { get; }
    }
}