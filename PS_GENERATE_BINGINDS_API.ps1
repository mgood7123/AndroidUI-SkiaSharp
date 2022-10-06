dotnet run --project=utils/SkiaSharpGenerator/SkiaSharpGenerator.csproj -- generate --config binding/libSkiaSharp.json --skia binding-native --output binding/Binding/SkiaApi.generated.cs
$RETURN = $?
if ($RETURN) {
	dotnet run --project=utils/SkiaSharpGenerator/SkiaSharpGenerator.csproj -- verify --config binding/libSkiaSharp.json --skia binding-native
	$RETURN = $?
}
return $RETURN