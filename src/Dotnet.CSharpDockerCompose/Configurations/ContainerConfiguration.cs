namespace Dotnet.CSharpDockerCompose.Configurations
{
    public class ContainerConfiguration
    {
        private const bool ShouldCheckHealthCheckDefault = false;

        public string ContainerSufixName { get; set; }
        public bool ShouldCheckHealthCheck { get; set; }

        public ContainerConfiguration(string containerSufixName, bool shouldCheckHealthCheck)
        {
            ContainerSufixName = containerSufixName;
            ShouldCheckHealthCheck = shouldCheckHealthCheck;
        }

        public ContainerConfiguration(string containerSufixName)
            : this(containerSufixName, ShouldCheckHealthCheckDefault)
        {

        }
    }
}
