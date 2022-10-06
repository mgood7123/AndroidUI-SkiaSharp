$SKIA_SHARP_PROJECT_BUILD_NUMBER = ./PS_SETUP_FOR_BUILD.ps1 | select -Last 1
if (!$SKIA_SHARP_PROJECT_BUILD_NUMBER.Equals($True) -and !$SKIA_SHARP_PROJECT_BUILD_NUMBER.Equals($False)) {
	dotnet cake --target=externals-android --buildall=true --buildnumber=$SKIA_SHARP_PROJECT_BUILD_NUMBER --configuration=Release
}