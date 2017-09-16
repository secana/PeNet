#tool nuget:?package=vswhere
#addin nuget:?package=Cake.OctoDeploy

var target					= Argument("target", "Default");
var solutionDir				= System.IO.Directory.GetCurrentDirectory();
var testResultDir			= Argument("testResultDir", System.IO.Path.Combine(solutionDir, "test-results"));   // ./build.sh --target test -testResultsDir="somedir"
var artifactDir				= Argument("artifactDir", System.IO.Path.Combine(solutionDir, "artifacts"));		// ./build.sh --target pack -artifactDir="somedir"
var apiKey					= Argument<string>("apiKey", null);													// ./build.sh --target push -apiKey="nuget api key"
var accessToken				= Argument<string>("accessToken", null);											// ./build.sh --target release -accessToken="github access token"
var peditorArtifactDir		= System.IO.Path.Combine(artifactDir, "PEditor");
string peditorReleaseZip	= null;
string peditorVersion		= null;
var testFailed				= false;

var peNetProj = System.IO.Path.Combine(solutionDir, "src", "PeNet", "PeNet.csproj");
var peditorProj = System.IO.Path.Combine(solutionDir, "src", "PEditor", "PEditor.csproj");

// Get the latest VS installation to find the latest MSBuild tools.
DirectoryPath vsLatest  = VSWhereLatest();
FilePath msBuildPathX64 = (vsLatest==null)
                            ? null
                            : vsLatest.CombineWithFilePath("./MSBuild/15.0/Bin/amd64/MSBuild.exe");


Information("Solution Directory: {0}", solutionDir);
Information("Test Results Directory: {0}", testResultDir);


Task("Clean")
	.Does(() =>
	{
		var delSettings = new DeleteDirectorySettings { Recursive = true, Force = true };
			
		if(DirectoryExists(testResultDir))
			DeleteDirectory(testResultDir, delSettings);

		if(DirectoryExists(artifactDir))
			DeleteDirectory(artifactDir, delSettings);

		var binDirs = GetDirectories("./**/bin");
		var objDirs = GetDirectories("./**/obj");
		var testResDirs = GetDirectories("./**/TestResults");
		
		DeleteDirectories(binDirs, delSettings);
		DeleteDirectories(objDirs, delSettings);
		DeleteDirectories(testResDirs, delSettings);
	});


Task("PrepareDirectories")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		EnsureDirectoryExists(testResultDir);
		EnsureDirectoryExists(artifactDir);
		EnsureDirectoryExists(peditorArtifactDir);
	});


Task("Restore")
	.IsDependentOn("PrepareDirectories")
	.Does(() =>
	{
		DotNetCoreRestore();	  
	});

Task("Build")
	.IsDependentOn("Restore")
	.Does(() =>
	{
		MSBuild(solutionDir, new MSBuildSettings {
			ToolPath = msBuildPathX64,
			Verbosity = Verbosity.Minimal,
			Configuration = "Release"
		});
	});


Task("Test")
	.IsDependentOn("Build")
	.ContinueOnError()
	.Does(() =>
	{
		var tests = GetTestProjectFiles();
		
		foreach(var test in tests)
		{
			var projectFolder = System.IO.Path.GetDirectoryName(test.FullPath);

			try
			{
				DotNetCoreTest(test.FullPath, new DotNetCoreTestSettings
				{
					ArgumentCustomization = args => args.Append("-l trx"),
					WorkingDirectory = projectFolder
				});
			}
			catch(Exception e)
			{
				testFailed = true;
				Error(e.Message.ToString());
			}
		}

		// Copy test result files.
		var tmpTestResultFiles = GetFiles("./**/*.trx");
		CopyFiles(tmpTestResultFiles, testResultDir);
	});


Task("Pack")
	.IsDependentOn("Test")
	.Does(() =>
	{
		if(testFailed)
		{
			Information("Do not pack because tests failed");
			return;
		}
		
		Information($"Pack {peNetProj}");
		var settings = new DotNetCorePackSettings
		{
			Configuration = "Release",
			OutputDirectory = artifactDir
		};
		DotNetCorePack(peNetProj, settings);

		Information($"Pack {peditorProj}");

		var peditorSetting = new MSBuildSettings {
			ToolPath = msBuildPathX64,
			Verbosity = Verbosity.Minimal,
			Configuration = "Release",
		};

        MSBuild(peditorProj, peditorSetting
			.WithTarget("publish")
			.WithProperty("PublishDir", peditorArtifactDir + @"\")
			);

		peditorVersion = GetPEditorVersion();
		peditorReleaseZip = System.IO.Path.Combine(artifactDir, $"PEditor-{peditorVersion}.zip");
		Zip(peditorArtifactDir, peditorReleaseZip);
	});

Task("Push")
    .IsDependentOn("Pack")
    .Does(() => {
        var package = GetFiles($"{artifactDir}/PeNet.*.nupkg").ElementAt(0);
        var source = "https://www.nuget.org/api/v2/package";

        if(apiKey==null)
            throw new ArgumentNullException(nameof(apiKey), "The \"apiKey\" argument must be set for this task.");

        Information($"Push {package} to {source}");

        NuGetPush(package, new NuGetPushSettings {
            Source = source,
            ApiKey = apiKey
        });
    });

/* Need to wait for octodeploy update
Task("Release")
	.IsDependentOn("Pack")
	.Does(() =>
	{
		if(testFailed)
		{
			Information("Do not publish because tests failed");
			return;
		}

		if(accessToken == null)
			throw new ArgumentNullException(nameof(accessToken), "You need to provide an GitHub access token to release PEditor.");

		var octoSettings = new OctoDeploySettings {
			AccessToken = accessToken,
			Owner = "secana",
			Repository = @"https://github.com/secana/PeNet"
		};

		var tag = $"v{peditorVersion}";
		var releaseTitle = $"PEditor Version {peditorVersion}";
		var releaseNotes = "Latest release of the GUI editor for Portable Executable headers based on the PeNet library.";
		var draftRelease = false;
		var preRelease = false;
		var artifactPath = new FilePath(peditorReleaseZip);
		var artifactName = artifactPath.GetFilename().FullPath;
		var artifactMimeType = "application/zip";
		
		Information($"ArtifactName: {artifactName}");
		Information($"ArtifactPath: {artifactPath}");
		
		PublishReleaseWithArtifact(
			tag,
			releaseTitle,
			releaseNotes,
			draftRelease,
			preRelease,
			artifactPath,
			artifactName,
			artifactMimeType,
			octoSettings); 
	});
*/

Task("Default")
	.IsDependentOn("Test")
	.Does(() =>
	{
		Information("Build and test the whole solution.");
		Information("To pack the PeNet library and Zip the PEditor use the cake build argument: --target Pack");
		Information("To release the PEditor use the cake build argument: --target Release -accessToken=\"github access token\"");
	});

string GetPEditorVersion()
{
	var versionPath = System.IO.Directory.GetDirectories(System.IO.Path.Combine(peditorArtifactDir, "Application Files")).ElementAt(0);
	var version = versionPath.Split(new char[] { '_' }, 2, StringSplitOptions.RemoveEmptyEntries).ElementAt(1).Replace('_', '.');
	Information($"Extracted {version} for PEditor");
	return version;
}

FilePathCollection GetSrcProjectFiles()
{
	return GetFiles("./src/**/*.csproj");
}

FilePathCollection GetTestProjectFiles()
{
	return GetFiles("./test/**/*Test/*.csproj");
}

RunTarget(target);