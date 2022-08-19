cd K:\AndroidUI-SkiaSharp\
dotnet tool restore

if ($?) {
	if (-not(Test-Path BUILD_NUMBER.txt)) {
		echo 3000 > BUILD_NUMBER.txt;
	}

	$build_number = [int] (cat BUILD_NUMBER.txt)
	$build_number++
	echo $build_number > BUILD_NUMBER.txt

	dotnet cake --target=nuget --buildall=true --skipexternals=all --buildnumber=$build_number
}