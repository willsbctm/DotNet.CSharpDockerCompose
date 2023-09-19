using System;
using System.Collections.Generic;
using System.Linq;
using Dotnet.CSharpDockerCompose.Configurations;

namespace Dotnet.CSharpDockerCompose
{
    public class ComposeConfigurationBuilder
    {
        public const string DefaultFileName = "docker-compose.yaml";
        public const int DefaultRetryCount = 5;
        public const int DefaultWaitInSeconts = 12;
        public const bool DefaultKeepRunning = false;
        public const bool DefaultBuildWithNoCache = false;

        private string _containerPrefixName = null!;
        private string _filePath = null!;
        private string _contextPath = null!;
        private string _fileName = DefaultFileName;
        private TimeSpan _waitForEachTry = TimeSpan.FromSeconds(DefaultWaitInSeconts);
        private int _retryCount = DefaultRetryCount;
        private bool _keepRunning = DefaultKeepRunning;
        private bool _buildWithNoCache = DefaultBuildWithNoCache;
        private readonly List<ContainerConfiguration> _containersConfigurations = new List<ContainerConfiguration>();

        public ComposeConfigurationBuilder WithContainerPrefixName(string name)
        {
            _containerPrefixName = name;
            return this;
        }

        public ComposeConfigurationBuilder WithComposeFilePath(string filePath)
        {
            _filePath = filePath;
            return this;
        }

        public ComposeConfigurationBuilder WithComposeContextPath(string contextPath)
        {
            _contextPath = contextPath;
            return this;
        }

        public ComposeConfigurationBuilder WithComposeFileName(string fileName)
        {
            _fileName = fileName;
            return this;
        }

        public ComposeConfigurationBuilder WithWaitForEachCheckIfIsRunning(TimeSpan wait)
        {
            _waitForEachTry = wait;
            return this;
        }

        public ComposeConfigurationBuilder WithRetryCountToCheckIfIsRunning(int retryCount)
        {
            _retryCount = retryCount;
            return this;
        }

        public ComposeConfigurationBuilder WithComposeKeepRunning()
        {
            _keepRunning = true;
            return this;
        }

        public ComposeConfigurationBuilder WithBuildWithNoCache()
        {
            _buildWithNoCache = true;
            return this;
        }

        public ComposeConfigurationBuilder WithContainerConfiguration(string sufixName, bool checkHealthCheck)
        {
            _containersConfigurations.Add(new ContainerConfiguration(sufixName, checkHealthCheck));
            return this;
        }

        public ComposeConfigurationBuilder WithContainerConfiguration(string sufixName) => WithContainerConfiguration(sufixName, false);

        public ComposeConfiguration Build()
        {
            if (string.IsNullOrEmpty(_containerPrefixName))
                throw new ArgumentNullException(nameof(_containerPrefixName), "should not be null or empty");

            if (string.IsNullOrEmpty(_fileName))
                throw new ArgumentNullException(nameof(_filePath), "should not be null or empty");

            if (string.IsNullOrEmpty(_contextPath))
                throw new ArgumentNullException(nameof(_contextPath), "should not be null or empty");

            if (string.IsNullOrEmpty(_contextPath))
                throw new ArgumentNullException(nameof(_contextPath), "should not be null or empty");

            if (_containersConfigurations == null || !_containersConfigurations.Any())
                throw new ArgumentNullException(nameof(_contextPath), "should not be null or empty");

            return new ComposeConfiguration(_containerPrefixName,
                new ComposeFileConfiguration(_filePath, _fileName, _contextPath),
                new ComposeWaitConfiguration(_waitForEachTry, _retryCount),
                new ComposeLifecycleConfiguration(_keepRunning, _buildWithNoCache));
        }
    }
}

