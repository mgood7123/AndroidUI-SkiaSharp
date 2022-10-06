$SKIA_SHARP_PROJECT_BUILD_NUMBER = ./PS_SETUP_FOR_BUILD.ps1 | select -Last 1
if (!$SKIA_SHARP_PROJECT_BUILD_NUMBER.Equals($True) -and !$SKIA_SHARP_PROJECT_BUILD_NUMBER.Equals($False)) {
	dotnet cake --target=nuget --buildall=true --skipexternals=all --buildnumber=$build_number --configuration=Debug
	if ($?) {
		./PS_INSTALL_NUGET.ps1
	}
}