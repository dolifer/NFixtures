using System.Collections.Generic;
using _build;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tooling;
using Nuke.Common.Tools.Coverlet;
using Nuke.Common.Tools.DotNet;
using Nuke.Common.Tools.GitVersion;
using Nuke.Common.Tools.ReportGenerator;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.IO.FileSystemTasks;
using static Nuke.Common.Tools.DotNet.DotNetTasks;
using static Nuke.Common.Tools.ReportGenerator.ReportGeneratorTasks;

[CheckBuildProjectConfigurations]
[UnsetVisualStudioEnvironmentVariables]
class Build : NukeBuild
{
    [Parameter] readonly string GitHubToken;

    public static int Main() => Execute<Build>(x => x.Compile);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    [Required] [GitVersion(Framework = "net5.0", NoFetch = true)] readonly GitVersion GitVersion;
    static AbsolutePath SourceDirectory => RootDirectory / "src";
    static AbsolutePath TestsDirectory => RootDirectory / "tests";
    static AbsolutePath ArtifactsDirectory => RootDirectory / "artifacts";

    Target Clean => _ => _
        .Executes(() =>
        {
            SourceDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            TestsDirectory.GlobDirectories("**/bin", "**/obj").ForEach(DeleteDirectory);
            EnsureCleanDirectory(ArtifactsDirectory);
        });

    Target Restore => _ => _
        .DependsOn(Clean)
        .Executes(() =>
        {
            DotNetRestore(_ => _
                .SetProjectFile(Solution));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            DotNetBuild(_ => _
                .SetProjectFile(Solution)
                .SetConfiguration(Configuration)
                .SetAssemblyVersion(GitVersion.AssemblySemVer)
                .SetFileVersion(GitVersion.AssemblySemFileVer)
                .SetInformationalVersion(GitVersion.InformationalVersion)
                .EnableNoRestore());
        });

    [Partition(2)] readonly Partition TestPartition;
    static AbsolutePath TestResultDirectory => ArtifactsDirectory / "test-results";
    static AbsolutePath CoverletResultDirectory => TestResultDirectory / "coverlet";
    static AbsolutePath JunitResultDirectory => TestResultDirectory / "junit";
    IEnumerable<Project> TestProjects => TestPartition.GetCurrent(Solution.GetProjects("*Tests"));
    static AbsolutePath CoverageReportDirectory => ArtifactsDirectory / "coverage-report";

    [Parameter] string NugetApiUrl = "https://nuget.pkg.github.com/dolifer/index.json"; //default

    Target Test => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            DotNetTest(_ => _
                .SetConfiguration(Configuration)
                .SetNoBuild(InvokedTargets.Contains(Compile))
                .EnableNoRestore()
                .ResetVerbosity()
                .SetResultsDirectory(TestResultDirectory)
                .EnableCollectCoverage()
                .SetCoverletOutputFormat(CoverletOutputFormat.cobertura)
                .SetExcludeByFile("*.Generated.cs")
                .CombineWith(TestProjects, (_, v) => _
                    .SetProjectFile(v)
                    .SetLogger(
                        $"junit;LogFilePath={JunitResultDirectory}/{v.Name}.xml;MethodFormat=Class;FailureBodyFormat=Verbose")
                    .SetCoverletOutput($"{CoverletResultDirectory}/{v.Name}.xml")));
        });

    Target NugetPublish => _ => _
        .DependsOn(Compile)
        .Executes(() =>
        {
            SourceDirectory.GlobFiles("*.nupkg")
                .NotEmpty()
                .ForEach(x =>
                {
                    DotNetNuGetPush(s => s
                        .SetTargetPath(x)
                        .SetSource(NugetApiUrl)
                        .SetApiKey(GitHubToken)
                    );
                });
        });

    Target Coverage => _ => _
        .DependsOn(Test)
        .TriggeredBy(Test)
        .Consumes(Test)
        .Produces(CoverageReportDirectory)
        .Executes(() =>
        {
            ReportGenerator(_ => _
                .SetReports(CoverletResultDirectory / "*.xml")
                .SetReportTypes(ReportTypes.HtmlInline_AzurePipelines, ReportTypes.Badges)
                .SetTargetDirectory(CoverageReportDirectory)
                .SetFramework("netcoreapp2.1"));
        });
}
