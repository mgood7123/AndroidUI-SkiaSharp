$SKIA_SHARP_PROJECT_BUILD_NUMBER = [int] (cat BUILD_NUMBER.txt)
$SKIA_SHARP_PROJECT_BUILD_NUMBER--

$MILESTONE = (((select-string "define" externals\skia\include\core\SkMilestone.h).line | select-string "SK_MILESTONE").line).SubString(21);

dotnet remove C:\Users\AndroidUI\Desktop\AndroidUI\AndroidUI\AndroidUI.csproj package SkiaSharp
dotnet remove C:\Users\AndroidUI\Desktop\AndroidUI\AndroidUI\AndroidUI.csproj package SkiaSharp.HarfBuzz
dotnet remove C:\Users\AndroidUI\Desktop\AndroidUI\AndroidUI\AndroidUI.csproj package SkiaSharp.Views
dotnet add C:\Users\AndroidUI\Desktop\AndroidUI\AndroidUI\AndroidUI.csproj package SkiaSharp --version 2.$MILESTONE.1-preview.$SKIA_SHARP_PROJECT_BUILD_NUMBER --source=K:\AndroidUI-SkiaSharp-Google\output\nugets

#dotnet add C:\Users\AndroidUI\Desktop\AndroidUI\AndroidUI\AndroidUI.csproj package SkiaSharp.HarfBuzz --version 2.88.1-preview.3223 --source=K:\AndroidUI-SkiaSharp\output\nugets
#dotnet add C:\Users\AndroidUI\Desktop\AndroidUI\AndroidUI\AndroidUI.csproj package SkiaSharp.Views --version 2.88.1-preview.3223 --source=K:\AndroidUI-SkiaSharp\output\nugets
#dotnet add C:\Users\AndroidUI\Desktop\AndroidUI\AndroidUI\AndroidUI.csproj package SkiaSharp.Views.WindowsForms --version 2.88.1-preview.3223 --source=K:\AndroidUI-SkiaSharp\output\nugets

dotnet add C:\Users\AndroidUI\Desktop\AndroidUI\AndroidUI\AndroidUI.csproj package SkiaSharp.HarfBuzz --version 2.$MILESTONE.1-preview.$SKIA_SHARP_PROJECT_BUILD_NUMBER --source=K:\AndroidUI-SkiaSharp-Google\output\nugets
return $?