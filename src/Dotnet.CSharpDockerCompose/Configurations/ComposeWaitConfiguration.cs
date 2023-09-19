using System;

namespace Dotnet.CSharpDockerCompose.Configurations
{
    public class ComposeWaitConfiguration
    {
        public TimeSpan WaitForEachTry { get; set; }
        public int RetryCount { get; set; }

        public ComposeWaitConfiguration(TimeSpan waitForEachTry, int retryCount)
        {
            WaitForEachTry = waitForEachTry;
            RetryCount = retryCount;
        }
    }
}
