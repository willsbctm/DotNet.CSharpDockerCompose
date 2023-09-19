using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Logging;
using System.Linq;
using Dotnet.CSharpDockerCompose.Configurations;

namespace Dotnet.CSharpDockerCompose
{
    public class DockerCompose : IDisposable
    {
        private readonly string _cli;
        private readonly ILogger _logger;
        private readonly ComposeConfiguration _configuration;
        private Process _composeUp = null!;

        public DockerCompose(ILogger logger, ComposeConfiguration configuration)
        {
            _cli = "docker";
            _configuration = configuration;
            _logger = logger;

            Write("Checking if compose file exists...");
            CheckIfComposeFileExists();

            Write("Checking docker...");
            if (!IsDockerRunning())
                throw new Exception("Docker is not running");
        }

        public bool IsDockerRunning()
        {
            var process = CreateProcess("ps");
            process.Start();
            process.WaitForExit();
            return process.ExitCode == 0;
        }


        public bool IsComposeRunning()
        {
            return _configuration.ContainersConfigurations.All(x =>
                        IsRunning(_configuration.GetContainerFullName(x))
                        && (!x.ShouldCheckHealthCheck || IsHealthy(_configuration.GetContainerFullName(x)))
                    );
        }

        public async Task Up()
        {
            if (_configuration.KeepRunning && IsComposeRunning())
            {
                Write("Compose already is running...");
                return;
            }
            else if (!_configuration.KeepRunning)
            {
                Write("Killing existing containers...");
                Kill();
            }

            if(_configuration.BuildWithNoCache)
                BuildWithNoCache();
            else
                Build();

            Write("Getting up the containers...");
            var process = CreateProcess("compose up");
            WriteProcessLog(process);
            process.Start();
            process.BeginOutputReadLine();

            Write("Waiting for container is healthy/running...");
            int counter = _configuration.RetryCount;
            do
            {
                Write("Trying...");
                if (process.HasExited)
                    throw new Exception("Error to compose up!");

                if (counter < 0)
                    throw new Exception("Error to compose up! Timeout.");

                await Task.Delay(_configuration.WaitForEachTry);

                counter--;
            }
            while (!IsComposeRunning());
            _composeUp = process;
        }

        private Process Down()
        {
            Write("Getting down the containers...");
            var process = CreateProcess("compose down");
            WriteProcessLog(process);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            return process;
        }

        public void Build() => Build("compose build");

        public void BuildWithNoCache() => Build("compose build --no-cache");

        private void Build(string command)
        {
            Write("Building images...");
            var process = CreateProcess(command);
            WriteProcessLog(process);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            if (process.ExitCode != 0)
                throw new Exception("Error building images");
        }

        private void Kill()
        {
            var process = CreateProcess("compose kill");
            WriteProcessLog(process);
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
        }

        private bool IsHealthy(string containerName)
        {
            var process = CreateProcess($"inspect --format=\"{{{{json .State.Health.Status}}}}\" {containerName}");
            process.Start();
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            Write(output);
            return output != null && output.StartsWith("\"healthy\"");
        }

        private bool IsRunning(string containerName)
        {
            var process = CreateProcess($"inspect --format=\"{{{{json .State.Status}}}}\" {containerName}");
            process.Start();
            process.WaitForExit();

            string output = process.StandardOutput.ReadToEnd();
            Write(output);
            return output != null && !output.StartsWith("\"exited\"");
        }

        private Process CreateProcess(string command) => new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = _cli,
                Arguments = $"{command}",
                UseShellExecute = false,
                CreateNoWindow = true,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                WorkingDirectory = _configuration.ContextPath
            }
        };

        private void CheckIfComposeFileExists()
        {
            var composePath = Path.Combine(_configuration.FilePath, _configuration.FileName);
            if (!File.Exists(composePath))
                throw new Exception("Compose does not exists!");
        }

        private void WriteProcessLog(Process process)
            => process.OutputDataReceived += (sender, outLine) =>
            {
                if (string.IsNullOrWhiteSpace(outLine.Data))
                    return;

                Write(outLine.Data);
            };

        private void Write(string message) => _logger.LogInformation(message);

        public void Dispose()
        {
            var downProcess = Down();
            downProcess?.Dispose();

            _composeUp?.Dispose();
        }
    }
}
