To build `SkiaSharp` on `Windows`:

this requires `~50GB to ~100GB` for `installation/setup`
and an additional `44GB` for building natives and bindings/views

building is recommended on `EXTERNAL HARD-DRIVE` if free space on internal hdd is small

install `VS 2019`
install `VS 2022`
install specified https://github.com/mono/SkiaSharp/wiki/Building-SkiaSharp for both 2019 and 2022
`~25GB` for each VS installation

`Windows 10 SDK (10.0.10240)` can be obtained from https://go.microsoft.com/fwlink/p/?LinkId=619296
`Windows 10 SDK (10.0.16299.91)` can be obtained from https://go.microsoft.com/fwlink/p/?linkid=864422

`Android NDK` is `not available in VS component selection`

install `Android NDK 21` from https://github.com/android/ndk/wiki/Unsupported-Downloads#r21e

`Android NDK 22` restructures the following root paths, which are not yet adapted to this build system
```
   * platforms
   * sources/cxx-stl
   * sysroot
   * toolchains (with the exception of toolchains/llvm)

   In general this change should only affect build system maintainers, or those
   using build systems that are not up to date. ndk-build and the CMake
   toolchain users are unaffected, and neither are
   `make_standalone_toolchain.py` users (though that script has been unnecessary
   since r19).
```


go to `System > Advanced System Settings > Environment Variables`
add the following
```
name:  ANDROID_SDK_ROOT
value: C:\Program Files (x86)\Android\android-sdk
```
the above is where VS installs the sdk to


```
name:  ANDROID_NDK_ROOT
value: C:\Users\small\Downloads\android-ndk-r21
```
```
name:  NDK_ROOT
value: C:\Users\small\Downloads\android-ndk-r21
```
(replace "small" with your username)





open and replace `./scripts/install-android-platform.ps1` with the following content
```ps1
Param(
    [string] $API
)

$ErrorActionPreference = 'Stop'

$sdk = "$env:ANDROID_SDK_ROOT"

$apiPath = "$sdk/platforms/android-$API/android.jar"
if (Test-Path $apiPath) {
    Write-Host "Android API level $API was already installed."
    exit 0
}

$latest = "$sdk/cmdline-tools/latest"
if (-not (Test-Path $latest)) {
    $versions = Get-ChildItem ("$sdk/cmdline-tools")
    $latest = "$sdk/cmdline-tools/" + ($versions | Select-Object -Last 1)[0]
}

if (-not $IsMacOS -and -not $IsLinux) {
    $ext = ".bat"
}

$sdkmanager = "$latest/bin/sdkmanager$ext"

Set-Content -Value "y" -Path "yes.txt"
try {
    if ($IsMacOS -or $IsLinux) {
        sh -c "`"$sdkmanager`" `"platforms\;android-$API`" < yes.txt"
    } else {
        cmd /c "`"$sdkmanager`" `"platforms;android-$API`" < yes.txt"
    }
} finally {
    Remove-Item "yes.txt"
}

exit $LASTEXITCODE
```

next execute the following in an admin powershell
```
./scripts/install-android-platform.ps1 29
```

execute the following in an admin powershell
```
Invoke-WebRequest 'https://raw.githubusercontent.com/Samsung/Tizen.NET/main/workload/scripts/workload-install.ps1' -OutFile 'workload-install_tizen.ps1';
.\workload-install_tizen.ps1

./scripts/install-7zip.ps1 # required for llvm to extract
```
go to `System > Advanced System Settings > Environment Variables`
add the following to system Path
```
value: C:\Program Files\7-zip
```
log out and log back in

execute the following in an admin powershell
```
./scripts/install-llvm.ps1 # windows llvm
```
go to `System > Advanced System Settings > Environment Variables`
add the following
```
name: LLVM_HOME
value: C:\Program Files\LLVM
```
add the following to system Path
```
value: C:\Program Files\LLVM\bin
```
log out and log back in

execute the following in an admin powershell
```
./scripts/install-gtk.ps1  # windows gtk
```

execute the following in a non-admin powershell
```
python3 # needed for git sync deps, will take you to microsoft store if not installed, otherwise exit via `exit()`
./scrips/install-maui.ps1
./scrips/install-mono.ps1

dotnet workload install --temp-dir K:\dotnet_workload_cache android ios tvos macos maccatalyst wasm-tools maui tizen --source https://api.nuget.org/v3/index.json --source https://pkgs.dev.azure.com/dnceng/public/_packaging/dotnet6/nuget/v3/index.json --source https://pkgs.dev.azure.com/dnceng/public/_packaging/darc-pub-dotnet-runtime-bd261ea4/nuget/v3/index.json --source https://pkgs.dev.azure.com/dnceng/public/_packaging/darc-pub-dotnet-emsdk-52e9452f-3/nuget/v3/index.json
```


add the following to `cake/msbuild.cake`

find
```ps1
        if (!string.IsNullOrEmpty(MSBUILD_EXE)) {
            c.ToolPath = MSBUILD_EXE;
        } else if (IsRunningOnWindows() && !string.IsNullOrEmpty(VS_INSTALL)) {
            c.ToolPath = ((DirectoryPath)VS_INSTALL).CombineWithFilePath("MSBuild/Current/Bin/MSBuild.exe");
        }
```
and replace it with
```ps1
        if (!string.IsNullOrEmpty(MSBUILD_EXE)) {
            c.ToolPath = MSBUILD_EXE;
        } else if (IsRunningOnWindows()) {
            if (!string.IsNullOrEmpty(VS_INSTALL)) {
                c.ToolPath = ((DirectoryPath)VS_INSTALL).CombineWithFilePath("MSBuild/Current/Bin/MSBuild.exe");
            } else {
                // check for msbuild 17, this is required to build .net 6 projects (non-natives)
                // VS 2022 is in preview
                // check VS 2022 Community when it comes out of preview
                bool vs2022Cexists = DirectoryExists("C:/Program Files/Microsoft Visual Studio/2022/Community");
                bool vs2022Pexists = DirectoryExists("C:/Program Files/Microsoft Visual Studio/2022/Preview");
                if (!vs2022Pexists && !vs2022Cexists) {
                    throw new Exception("Visual Studio 2022 Preview or Community is required");
                }
                if (vs2022Pexists) {
                    c.ToolPath = "C:/Program Files/Microsoft Visual Studio/2022/Preview/MSBuild/Current/Bin/MSBuild.exe";
                }
                if (vs2022Cexists) {
                    c.ToolPath = "C:/Program Files/Microsoft Visual Studio/2022/Community/MSBuild/Current/Bin/MSBuild.exe";
                }
            }
        }
```





next, we can begin building

we will assume `K:\` is location of `SkiaSharp git repo` and the repo has been cloned into `SkiaSharp`
```ps1
cd K:\SkiaSharp\ ; dotnet tool restore ; dotnet cake --target=everything
```