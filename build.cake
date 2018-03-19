var target = Argument("target", "Default");

var configuration = Argument("configuration", EnvironmentVariable("CONFIGURATION") ?? "Release");

var artifactsDirectory = "./artifacts";

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(artifactsDirectory);

        StartProcess("dotnet", new ProcessSettings
        {
            Arguments = "clean"
        });
    });

Task("Restore")
    .IsDependentOn("Clean")
    .Does(() =>
    {
        DotNetCoreRestore();
    });

Task("Build")
    .IsDependentOn("Restore")
    .Does(() =>
    {
        var version = EnvironmentVariable("APPVEYOR_BUILD_VERSION") ?? "0.0.0";

        StartProcess("dotnet", new ProcessSettings
        {
            Arguments = $"build --configuration {configuration} --no-restore /p:Version={version}"
        });
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        foreach(var filePath in GetFiles(@".\test\**\*.csproj")) 
        { 
            StartProcess("dotnet", new ProcessSettings
            {
                Arguments = $"test {filePath} --configuration {configuration} --logger trx;LogFileName=TestResult.xml --no-build --no-restore"
            });
        }

        if (AppVeyor.IsRunningOnAppVeyor)
        {
            foreach(var filePath in GetFiles(@".\test\**\TestResult.xml"))
            {
                AppVeyor.UploadTestResults(filePath.FullPath, AppVeyorTestResultsType.NUnit);
            }
        }
    });

Task("Publish")
    .IsDependentOn("Test")
    .Does(() => 
    {
        StartProcess("dotnet", new ProcessSettings
        {
            Arguments = $"publish src/Stubbl.Api --configuration {configuration} --no-build"
        });
    });

Task("Pack")
    .IsDependentOn("Publish")
    .Does(() =>
    {
        CreateDirectory(artifactsDirectory);

        var artifactFilePath = $"{artifactsDirectory}/stubbl-api.zip";
        
        Zip($"src/Stubbl.Api/bin/{configuration}/netstandard2.0/publish", artifactFilePath); 
        
        if (AppVeyor.IsRunningOnAppVeyor)
        {
            AppVeyor.UploadArtifact(artifactFilePath, new AppVeyorUploadArtifactsSettings
            {
                DeploymentName = "stubbl-api"
            });
        }
    });

Task("Default")
    .IsDependentOn("Pack");

RunTarget(target);