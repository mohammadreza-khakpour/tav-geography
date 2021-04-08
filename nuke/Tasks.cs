using System.IO;
using Microsoft.Extensions.Configuration;
using Nuke.Common;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Tasks : NukeBuild
{
    public static int Main () => Execute<Tasks>(_ => _.Run);

    AbsolutePath SourceDirectory => RootDirectory / "src";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath RestApiProjectDirectory => SourceDirectory / "Geography.RestApi";
    AbsolutePath MigrationsProjectDirectory => SourceDirectory / "Geography.Migrations";

    Target Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            DeleteDirectory(OutputDirectory);
        });

    Target Run => _ => _
        .Executes(() =>
        {
            return DotNetRun(_ => _.SetProjectFile(RestApiProjectDirectory));
        });

    Target Build => _ => _
        .Executes(() =>
        {
            return DotNetPublish(_ => _
                .SetProject(RestApiProjectDirectory)
                .SetConfiguration(Configuration.Release)
                .SetOutput(OutputDirectory)
                .DisableSelfContained()
                .SetProperty("PublishSingleFile", true)
                .SetProperty("IsTransformWebConfigDisabled", true)
                .SetProperty("DebugType", "None")
                .SetRuntime("win-x64"));
        });

    Target BuildIIS => _ => _
        .Executes(() =>
        {
            return DotNetPublish(_ => _
                .SetProject(RestApiProjectDirectory)
                .SetConfiguration(Configuration.Release)
                .SetOutput(OutputDirectory)
                .DisableSelfContained()
                .SetProperty("PublishSingleFile", true)
                .SetProperty("DebugType", "None")
                .SetProperty("AspNetCoreModuleName", "AspNetCoreModule")
                .SetProperty("AspNetCoreHostingModel", "OutOfProcess")
                .SetRuntime("win-x64"));
        });

    Target BuildLinux => _ => _
        .Executes(() =>
        {
            return DotNetPublish(_ => _
                .SetProject(RestApiProjectDirectory)
                .SetConfiguration(Configuration.Release)
                .SetOutput(OutputDirectory)
                .DisableSelfContained()
                .SetProperty("PublishSingleFile", true)
                .SetProperty("IsTransformWebConfigDisabled", true)
                .SetProperty("DebugType", "None")
                .SetRuntime("linux-musl-x64"));
        });

    Target Migrate => _ => _
        .Executes(() =>
        {
            var settingsPath = Path.Combine(RestApiProjectDirectory, "appsettings.json");
            var settings = new ConfigurationBuilder().AddJsonFile(settingsPath, optional: true).Build();
            var connectionString = settings.GetValue<string>("dbConnectionString");

            return DotNetRun(_ => _
                .SetProjectFile(MigrationsProjectDirectory)
                .SetApplicationArguments($@"connection-string=""{connectionString}""")
                .SetWorkingDirectory(RestApiProjectDirectory));
        });
}
