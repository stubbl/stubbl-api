var target = Argument("target", "Default");

var configuration = Argument("configuration", EnvironmentVariable("CONFIGURATION") ?? "Release");

var artifactsDirectory = "./artifacts";

Task("Clean")
    .Does(() =>
    {
        CleanDirectories(artifactsDirectory);

        StartAndReturnProcess("dotnet", new ProcessSettings
            {
                Arguments = "clean"
            })
            .WaitForExit();
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
        StartAndReturnProcess("dotnet", new ProcessSettings
            {
                Arguments = $"build --configuration {configuration} --no-restore"
            })
            .WaitForExit();
    });

Task("Test")
    .IsDependentOn("Build")
    .Does(() =>
    {
        foreach (var filePath in GetFiles(@".\test\**\*.csproj")) 
        { 
            StartAndReturnProcess("dotnet", new ProcessSettings
                {
                    Arguments = $"test {filePath} --configuration {configuration} --logger trx;LogFileName=TestResult.xml --no-build --no-restore"
                })
                .WaitForExit();
        }

        if (AppVeyor.IsRunningOnAppVeyor)
        {
            foreach (var filePath in GetFiles(@".\test\**\TestResult.xml"))
            {
                AppVeyor.UploadTestResults(filePath, AppVeyorTestResultsType.NUnit3);
            }
        }
    });

Task("Publish")
    .IsDependentOn("Test")
    .Does(() => 
    {
        var version = EnvironmentVariable("APPVEYOR_BUILD_VERSION") ?? "1.2.3";

        StartAndReturnProcess("dotnet", new ProcessSettings
            {
                Arguments = $"publish src/Stubbl.Api --configuration {configuration} --no-restore /p:Version={version}"
            })
            .WaitForExit();
    });

Task("Pack")
    .IsDependentOn("Publish")
    .Does(() =>
    {
        CreateDirectory(artifactsDirectory);

        var artifactFilePath = $"{artifactsDirectory}/stubbl-api.zip";
        
        Zip($"./src/Stubbl.Api/bin/{configuration}/netcoreapp2.0/publish/", artifactFilePath); 
        
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