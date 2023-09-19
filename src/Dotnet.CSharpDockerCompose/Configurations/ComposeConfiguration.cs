using System.IO;
using System;
using System.Collections.Generic;

namespace Dotnet.CSharpDockerCompose.Configurations
{
    public class ComposeConfiguration
    {
        public string ContainerPrefixName { get; set; }
        public string FilePath { get; set; }
        public string ContextPath { get; set; }
        public string FileName { get; set; }
        public TimeSpan WaitForEachTry { get; set; }
        public int RetryCount { get; set; }
        public bool KeepRunning { get; set; }
        public bool BuildWithNoCache { get; set; }
        public List<ContainerConfiguration> ContainersConfigurations { get; set; }

        public ComposeConfiguration(string containerPrefixName, ComposeFileConfiguration composeFileConfiguration, ComposeWaitConfiguration composeWaitConfiguration, ComposeLifecycleConfiguration composeLifecycleConfiguration)
        {
            FilePath = composeFileConfiguration.FilePath;
            FileName = composeFileConfiguration.FileName;
            ContextPath = composeFileConfiguration.ContextPath;
            ContainerPrefixName = containerPrefixName;
            WaitForEachTry = composeWaitConfiguration.WaitForEachTry;
            RetryCount = composeWaitConfiguration.RetryCount;
            KeepRunning = composeLifecycleConfiguration.KeepRunning;
            BuildWithNoCache = composeLifecycleConfiguration.BuildWithNoCache;
            ContainersConfigurations = new List<ContainerConfiguration>();
        }

        public string GetComposeFullName() => Path.Combine(FilePath, FileName);
        public string GetContainerFullName(ContainerConfiguration container) => Path.Combine(ContainerPrefixName, container.ContainerSufixName);
    }
}

