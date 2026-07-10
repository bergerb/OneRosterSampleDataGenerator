# Copilot Instructions for OneRosterSampleDataGenerator

## Build, test, and lint commands

- **Restore:** `dotnet restore OneRosterSampleDataGenerator.sln`
- **Build (Debug, closest lint gate in this repo):** `dotnet build OneRosterSampleDataGenerator.sln -c Debug`
- **Build (Release):** `dotnet build OneRosterSampleDataGenerator.sln -c Release`
- **Run all tests:** `dotnet test Tests\Tests.csproj -c Debug`
- **Run a single test class:** `dotnet test Tests\Tests.csproj --filter "FullyQualifiedName~Tests.OneRosterGenerationTests"`
- **Run a single test method:** `dotnet test Tests\Tests.csproj --filter "FullyQualifiedName~Tests.OneRosterGenerationTests.OrgCount_ShouldBeEqualToSchoolCount_WhenGeneratedAndSetToValue"`
- **Pack NuGet package:** `dotnet pack OneRosterSampleDataGenerator\OneRosterSampleDataGenerator.csproj -c Release -o .\nupkgs`

Notes:
- Target framework is **.NET 10** (`net10.0`).
- There is no separate lint command in the repo; quality gates are primarily build + tests (Debug config treats key nullable/package warnings as errors).

## High-level architecture

`OneRoster` is the orchestration entrypoint. Its constructor builds the full in-memory OneRoster dataset in dependency order:

1. `AcademicSessions` and `Grades`
2. `Orgs` (depends on grades)
3. `Courses` (depends on parent org + sessions + grades)
4. `Students` and `Staffs`
5. `Classes` (assigns teachers and creates student/teacher enrollments)
6. `Demographics` and `Manifest`

### Generation model

- Most domain generators inherit `Models.Base.Generator<T>` and store state in `Items`.
- `Generate()` usually materializes deterministic lists from `Create...()` iterators, with randomness from `Bogus`/`Random`.
- ID seeds (`StudentIdStart`, `StaffIdStart`) are controlled through `OneRoster.Args`.

### Output pipeline

- `OutputCSVFiles()` writes CSVs under `.\OneRoster`.
- `OutputOneRosterZipFile()` calls `OutputCSVFiles()` then zips from `AppContext.BaseDirectory\OneRoster` into `OneRoster{version}.zip`.
- `FileProcessor` maps domain models to export DTOs via `IExportable<TModel, TExport>` switch routing.
- CSV schema is defined in `Models/Exports/*` via CsvHelper `[Name(...)]` and `[Format(...)]` attributes.
- `StatusChangeBuilder` records create/deactivate/error events and emits `OneRosterChanges.txt`.

### Incremental/delta mode

If `Args.IncrementalDaysToCreate` is set, constructor-driven flow also:
- writes an initial zip,
- loops day-by-day applying `DeactivateStudentDataService` + `AddStudentDataService`,
- writes additional versioned zips and updates change log.

## Key codebase conventions

- **Preserve generation order in `OneRoster`**: later generators rely on earlier lists being populated.
- **When adding/changing an output file**, update all relevant surfaces together:
  1. export DTO in `Models/Exports`,
  2. `FileProcessor.GetExportable` routing,
  3. `OneRoster.OutputCSVFiles`,
  4. manifest/file stats/constants as needed.
- **Use existing status semantics** for deltas: active vs `tobedeleted` (`StatusType`), plus `UserExtensions.DeactivateUser` and `Enrollments.InactivateEnrollment`.
- **Keep export column names exact** using CsvHelper attributes; schema correctness lives in export DTOs, not domain models.
- **Tests convention**:
  - NUnit + Shouldly
  - test classes commonly inherit `RosterTest` (which cleans zip artifacts and creates a fresh `OneRoster` in `[SetUp]`)
  - prefer assertions that tolerate randomized data unless testing explicit deterministic options.
- **Style/construction pattern** follows existing repo choices:
  - block-scoped namespaces,
  - frequent primary constructors for classes/records,
  - nullable enabled; some warnings elevated in Debug build config.