cd K:\AndroidUI-SkiaSharp-Google\
dotnet tool restore
$RESULT = $?
if ($RESULT) {
	./PS_GENERATE_BINGINDS_API.ps1
	$RESULT = $?
	if ($RESULT) {
		if (-not(Test-Path BUILD_NUMBER.txt)) {
			echo 0 > BUILD_NUMBER.txt;
		}

		$SKIA_SHARP_PROJECT_BUILD_NUMBER = [int] (cat BUILD_NUMBER.txt)
		$SKIA_SHARP_PROJECT_BUILD_NUMBER++
		echo $SKIA_SHARP_PROJECT_BUILD_NUMBER > BUILD_NUMBER.txt
		$RESULT = $SKIA_SHARP_PROJECT_BUILD_NUMBER
	}
}
return $RESULT