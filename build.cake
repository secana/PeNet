#tool nuget:?package=vswhere

var target = Argument("target", "Default");
var solutionDir = System.IO.Directory.GetCurrentDirectory();
var testResultDir = Argument("testResultDir", System.IO.Path.Combine(solutionDir, "test-results"));     // ./build.sh --target publish -testResultsDir="somedir"
var artifactDir = Argument("artifactDir", System.IO.Path.Combine(solutionDir, "artifacts")); 												// ./build.sh --target publish -artifactDir="somedir"
var testFailed = false;

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
		if(DirectoryExists(testResultDir))
			DeleteDirectory(testResultDir, recursive:true);

		if(DirectoryExists(artifactDir))
			DeleteDirectory(artifactDir, recursive:true);

		var binDirs = GetDirectories("./**/bin");
		var objDirs = GetDirectories("./**/obj");
		var testResDirs = GetDirectories("./**/TestResults");
		
		DeleteDirectories(binDirs, true);
		DeleteDirectories(objDirs, true);
		DeleteDirectories(testResDirs, true);
	});


Task("PrepareDirectories")
	.IsDependentOn("Clean")
	.Does(() =>
	{
		EnsureDirectoryExists(testResultDir);
		EnsureDirectoryExists(artifactDir);
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

        MSBuild(peditorProj, peditorSetting.WithTarget("publish").WithProperty("PublishDir", artifactDir + @"\"));
	});


Task("Publish")
	.IsDependentOn("Test")
	.Does(() =>
	{
		if(testFailed)
		{
			Information("Do not publish because tests failed");
			return;
		}
		var projects = GetFiles("./src/**/*.csproj");

		foreach(var project in projects)
		{
			var projectDir = System.IO.Path.GetDirectoryName(project.FullPath);
			var projectName = new System.IO.DirectoryInfo(projectDir).Name;
			var outputDir = System.IO.Path.Combine(artifactDir, projectName);
			EnsureDirectoryExists(outputDir);

			Information("Publish {0} to {1}", projectName, outputDir);

			var settings = new DotNetCorePublishSettings
			{
				OutputDirectory = outputDir,
				Configuration = "Release"
			};

			DotNetCorePublish(project.FullPath, settings);
		}
	});

Task("Default")
	.IsDependentOn("Test")
	.Does(() =>
	{
		Information("Build and test the whole solution.");
		Information("To pack (nuget) the application use the cake build argument: --target Pack");
		Information("To publish (to run it somewhere else) the application use the cake build argument: --target Publish");
	});


FilePathCollection GetSrcProjectFiles()
{
	return GetFiles("./src/**/*.csproj");
}

FilePathCollection GetTestProjectFiles()
{
	return GetFiles("./test/**/*Test/*.csproj");
}

RunTarget(target);