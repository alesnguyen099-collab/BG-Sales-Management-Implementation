# Build verification

A GitHub Actions workflow is defined at `.github/workflows/windows-build.yml`. It restores and builds `BGSM.sln` on `windows-latest` with .NET 8.

A workflow file is not evidence of a successful build. Confirm the run result and inspect logs after GitHub Actions executes. Native WinUI builds require a compatible Windows SDK and Windows App SDK restore.

Local manual check on Windows:
1. Install Visual Studio 2022 with WinUI application development workload and compatible Windows SDK.
2. Run `dotnet restore BGSM.sln`.
3. Run `dotnet build BGSM.sln --configuration Release`.
4. Record errors, package versions, SDK version and run URL before declaring pass.