namespace Dotnet.CSharpDockerCompose.Configurations
{
    public class ComposeLifecycleConfiguration
    {
        public bool KeepRunning { get; set; }
        public bool BuildWithNoCache { get; set; }

        public ComposeLifecycleConfiguration(bool keepRunning, bool buildWithNoCache)
        {
            KeepRunning = keepRunning;
            BuildWithNoCache = buildWithNoCache;
        }
    }
}
