namespace Dotnet.CSharpDockerCompose.Configurations
{
    public class ComposeFileConfiguration
    {
        public string FilePath { get; set; }
        public string ContextPath { get; set; }
        public string FileName { get; set; }

        public ComposeFileConfiguration(string filePath, string fileName, string contextPath)
        {
            FilePath = filePath;
            FileName = fileName;
            ContextPath = contextPath;
        }
    }
}
