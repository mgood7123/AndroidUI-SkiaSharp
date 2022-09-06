cd K:\AndroidUI-SkiaSharp\
dotnet tool restore

if ($?) {
	dotnet run --project=utils/SkiaSharpGenerator/SkiaSharpGenerator.csproj -- generate --config binding/libSkiaSharp.json --skia externals/skia --output binding/Binding/SkiaApi.generated.cs

	if ($?) {
		dotnet run --project=utils/SkiaSharpGenerator/SkiaSharpGenerator.csproj -- verify --config binding/libSkiaSharp.json --skia externals/skia

		if ($?) {
			if (-not(Test-Path BUILD_NUMBER.txt)) {
				echo 3000 > BUILD_NUMBER.txt;
			}

			$build_number = [int] (cat BUILD_NUMBER.txt)
			$build_number++
			echo $build_number > BUILD_NUMBER.txt

			# externals # everything
			# externals-windows
			# externals-uwp
			dotnet cake --target=externals-windows --buildall=true --buildnumber=$build_number --configuration=Debug
		}
	}
}