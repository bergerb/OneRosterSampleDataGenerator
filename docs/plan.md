# OneRoster 1.1 / 1.2 Dual Support — Plan

## Goal
Make `OneRosterSampleDataGenerator` capable of generating CSV output compatible with **both OneRoster v1.1 and v1.2**, selectable via configuration.

## Architecture — Version Strategy Pattern

To avoid scattering version-aware conditionals (`if version == V1_1`) throughout the codebase, the design uses a **composition-based strategy pattern**. An `IRosterVersionStrategy` interface encapsulates all version-specific behavior: which export DTOs to use, manifest entries, file list, and whether to include roles. The orchestrator (`OneRoster`) and file writer (`FileProcessor`) become version-agnostic, delegating to the strategy.

This makes adding future versions (v1.3, v2.0) a matter of adding a new strategy class plus export DTOs — no changes to the core pipeline.

### Why Strategy Over Conditionals

| Approach | Problem |
|----------|---------|
| `if/else` in `FileProcessor` and `OneRoster` | Version logic scattered across 3+ files; every new version touches core code |
| `switch` on version in `Manifests` | Manifest generation coupled to version enum |
| **Strategy pattern** | Version logic encapsulated in one class per version; core pipeline is version-agnostic |

---

## Core Interfaces and Records

### `IRosterVersionStrategy`

```csharp
namespace OneRosterSampleDataGenerator.Models.Interfaces;

public interface IRosterVersionStrategy
{
    string ManifestVersion { get; }
    bool IncludesRoles { get; }
    void ProcessAllFiles(FileProcessor fileProcessor, RosterData data);
    IReadOnlyList<Manifest> GetManifestEntries(Org parentOrg);
}
```

Each implementation (`V11RosterStrategy`, `V12RosterStrategy`) maps domain models to the correct version-specific export DTOs and provides the right manifest entries.

### `RosterData`

A record bundling all domain data, passed from `OneRoster` to the strategy:

```csharp
namespace OneRosterSampleDataGenerator.Models;

public record RosterData(
    IEnumerable<AcademicSession> AcademicSessions,
    IEnumerable<Org> Orgs,
    IEnumerable<Course> Courses,
    IEnumerable<User> Users,
    IEnumerable<Class> Classes,
    IEnumerable<Enrollment> Enrollments,
    IEnumerable<Demographic> Demographics,
    IEnumerable<Manifest> Manifest,
    IEnumerable<Role>? Roles = null);
```

### Strategy Implementations

**`V11RosterStrategy.ProcessAllFiles()`:**
```csharp
fileProcessor.ProcessFile<AcademicSession, AcademicSessionFile>(data.AcademicSessions);
fileProcessor.ProcessFile<Org, OrgFile>(data.Orgs);
fileProcessor.ProcessFile<Course, CourseFile>(data.Courses);
fileProcessor.ProcessFile<User, UserFile>(data.Users);
fileProcessor.ProcessFile<Class, ClassFile>(data.Classes);
fileProcessor.ProcessFile<Enrollment, EnrollmentFile>(data.Enrollments);
fileProcessor.ProcessFile<Demographic, DemographicFile>(data.Demographics);
fileProcessor.ProcessFile<Manifest, ManifestFile>(data.Manifest);
```

**`V12RosterStrategy.ProcessAllFiles()`:**
```csharp
fileProcessor.ProcessFile<AcademicSession, V12.AcademicSessionFile>(data.AcademicSessions);
fileProcessor.ProcessFile<Org, V12.OrgFile>(data.Orgs);
fileProcessor.ProcessFile<Course, V12.CourseFile>(data.Courses);
fileProcessor.ProcessFile<User, V12.UserFile>(data.Users);
fileProcessor.ProcessFile<Class, V12.ClassFile>(data.Classes);
fileProcessor.ProcessFile<Enrollment, V12.EnrollmentFile>(data.Enrollments);
fileProcessor.ProcessFile<Demographic, V12.DemographicFile>(data.Demographics);
fileProcessor.ProcessFile<Role, V12.RoleFile>(data.Roles!);
fileProcessor.ProcessFile<Manifest, V12.ManifestFile>(data.Manifest);
```

### How `OneRoster` Uses the Strategy

**`OneRoster.cs` constructor:**
```csharp
_strategy = _args.Version switch
{
    OneRosterVersion.V1_1 => new V11RosterStrategy(),
    OneRosterVersion.V1_2 => new V12RosterStrategy(),
    _ => throw new ArgumentException($"Unsupported version: {_args.Version}")
};

// ... existing generation steps ...

if (_strategy.IncludesRoles)
{
    this.Roles = new Roles(DateLastModified, Students, Staff, Orgs).Generate();
}

this.Manifest = new Manifests(DateLastModified, ParentOrg, _strategy).Generate();
```

**`OneRoster.OutputCSVFiles()`:**
```csharp
public void OutputCSVFiles()
{
    SetupDirectory();
    FileProcessor fileProcessor = new(StatusChangeBuilder);

    var data = new RosterData(
        AcademicSessions, Orgs, Courses,
        Students.Union(Staff), Classes,
        Enrollments, Demographics, Manifest, Roles);

    _strategy.ProcessAllFiles(fileProcessor, data);

    StatusChangeBuilder.OutputChangeLog();
}
```

### How `Manifests` Uses the Strategy

`Manifests` accepts the strategy and delegates entry generation:

```csharp
public class Manifests(DateTime createdAt, Org parentOrg, IRosterVersionStrategy strategy)
    : Generator<Manifest>(createdAt)
{
    public override List<Manifest> Generate()
    {
        this.Items = strategy.GetManifestEntries(parentOrg).ToList();
        return this.Items;
    }
}
```

The strategy implementations provide the right entries:

**`V11RosterStrategy.GetManifestEntries()`:**
```csharp
// Returns: "oneroster.version" = "1.1", 7 file entries
```

**`V12RosterStrategy.GetManifestEntries()`:**
```csharp
// Returns: "oneroster.version" = "1.2", 7 bulk file entries + roles (bulk) + 14 absent entries
```

### `FileProcessor` — No Changes Needed

The existing generic `ProcessFile<T1, T2>()` method works as-is. The strategy calls it with the correct type parameters at runtime. No version logic needed in `FileProcessor`.

---

## 1. OneRoster 1.2 CSV Files (22 total)

The v1.2 spec defines up to 22 CSV files. The current code generates 8 (manifest + 7 data files).

### REQUIRED (always present):
| File | Status |
|------|--------|
| `manifest.csv` | Present today |

### EXISTING DATA FILES (present today, schemas need updating):
| File | Today | v1.2 Changes |
|------|-------|-------------|
| `academicSessions.csv` | Same | No changes |
| `orgs.csv` | Same | No changes |
| `courses.csv` | Same | No changes |
| `classes.csv` | Update | `termSourcedId` -> `termSourcedIds`; `SubjectCodes` -> `subjectCodes` |
| `users.csv` | **Major update** | Remove `orgSourcedIds`, `role`, `agentSourcedIds`, `userIds`, `sms`, `phone`, `password`, `middleName`; Add `primaryOrgSourcedId`, `preferredGivenName`, `preferredMiddleName`, `preferredFamilyName`, `userMasterIdentifier`, `pronouns` |
| `enrollments.csv` | Same | Add `courseSourcedId` column (already in domain model, missing from export) |
| `demographics.csv` | **Fix** | `nativeAmericanOrOtherPacificIslander` -> `nativeHawaiianOrOtherPacificIslander`; Add `white`, `demographicRaceTwoOrMoreRaces`, `hispanicOrLatinoEthnicity` |

### NEW v1.2 DATA FILES (infrastructure ready, manifest declares, not generated by default):
| File | Description | Default |
|------|-------------|---------|
| `roles.csv` | User->Org->Role mapping (replaces `orgSourcedIds`/`role` in users.csv) | Generated |
| `userProfiles.csv` | User profile data | `absent` |
| `userResources.csv` | User->Resource links | `absent` |
| `resources.csv` | Resource definitions | `absent` |
| `classResources.csv` | Class->Resource links | `absent` |
| `courseResources.csv` | Course->Resource links | `absent` |
| `categories.csv` | Gradebook categories | `absent` |
| `lineItems.csv` | Gradebook line items | `absent` |
| `lineItemLearningObjectiveIds.csv` | Learning objectives -> lineItems | `absent` |
| `lineItemScoreScales.csv` | Score scales -> lineItems | `absent` |
| `scoreScales.csv` | Score scale definitions | `absent` |
| `results.csv` | Gradebook results | `absent` |
| `resultLearningObjectiveIds.csv` | Learning objectives -> results | `absent` |
| `resultScoreScales.csv` | Score scales -> results | `absent` |

---

## 2. Full CSV Column Schemas

### 2.1 manifest.csv (required)
```
propertyName,value
"propertyName","value"
"manifest.version","1.0"
"oneroster.version","1.2"
"source.systemName","Solar School District OneRoster"
"source.systemCode","9999"
"file.academicSessions","bulk"
"file.orgs","bulk"
"file.courses","bulk"
"file.classes","bulk"
"file.users","bulk"
"file.enrollments","bulk"
"file.demographics","bulk"
"file.roles","absent"
"file.userProfiles","absent"
"file.userResources","absent"
"file.resources","absent"
"file.classResources","absent"
"file.courseResources","absent"
"file.categories","absent"
"file.lineItems","absent"
"file.lineItemLearningObjectiveIds","absent"
"file.lineItemScoreScales","absent"
"file.scoreScales","absent"
"file.results","absent"
"file.resultLearningObjectiveIds","absent"
"file.resultScoreScales","absent"
```

**v1.1 -> v1.2 changes**: `oneroster.version` = `"1.2"`; 8 new file entries added.

### 2.2 academicSessions.csv (unchanged from v1.1)

| Column | Required | Format | Description |
|--------|----------|--------|-------------|
| `sourcedId` | Yes | GUID | Unique ID |
| `status` | Yes for Delta | Enumeration | `active` / `tobedeleted` |
| `dateLastModified` | Yes for Delta | DateTime | ISO 8601 UTC |
| `title` | Yes | String | Name/title |
| `type` | Yes | Enumeration | `gradingPeriod` / `semester` / `schoolYear` / `term` |
| `startDate` | Yes | Date | Inclusive start (YYYY-MM-DD) |
| `endDate` | Yes | Date | Exclusive end (YYYY-MM-DD) |
| `parentSourcedId` | No | GUID Reference | Parent academic session |
| `schoolYear` | Yes | Year | School year (YYYY) |

### 2.3 orgs.csv (unchanged from v1.1)

| Column | Required | Format | Description |
|--------|----------|--------|-------------|
| `sourcedId` | Yes | GUID | Unique ID |
| `status` | Yes for Delta | Enumeration | `active` / `tobedeleted` |
| `dateLastModified` | Yes for Delta | DateTime | ISO 8601 UTC |
| `name` | Yes | String | Org name |
| `type` | Yes | Enumeration | `department` / `school` / `district` / `local` / `state` / `national` |
| `identifier` | No | String | Org identifier |
| `parentSourcedId` | No | GUID Reference | Parent org |

### 2.4 courses.csv (unchanged from v1.1)

| Column | Required | Format | Description |
|--------|----------|--------|-------------|
| `sourcedId` | Yes | GUID | Unique ID |
| `status` | Yes for Delta | Enumeration | `active` / `tobedeleted` |
| `dateLastModified` | Yes for Delta | DateTime | ISO 8601 UTC |
| `schoolYearSourcedId` | No | GUID Reference | AcademicSession (type=schoolYear) |
| `title` | Yes | String | Course name |
| `courseCode` | No | String | Course code |
| `grades` | No | List of Strings | Grade levels |
| `orgSourcedId` | Yes | GUID Reference | Owning org |
| `subjects` | No | List of Strings | Subject names |
| `subjectCodes` | No | List of Strings | Subject codes |

### 2.5 classes.csv (v1.2 updates)

| Column | Required | Format | Description |
|--------|----------|--------|-------------|
| `sourcedId` | Yes | GUID | Unique ID |
| `status` | Yes for Delta | Enumeration | `active` / `tobedeleted` |
| `dateLastModified` | Yes for Delta | DateTime | ISO 8601 UTC |
| `title` | Yes | String | Class name |
| `grades` | No | List of Strings | Grade levels |
| `courseSourcedId` | Yes | GUID Reference | Course instance of |
| `classCode` | No | String | Human-readable code |
| `classType` | Yes | Enumeration | `homeroom` / `scheduled` |
| `location` | No | String | Physical location |
| `schoolSourcedId` | Yes | GUID Reference | School org |
| `termSourcedIds` | Yes | List of GUID References | Academic sessions (plural!) |
| `subjects` | No | List of Strings | Subject names |
| `subjectCodes` | No | List of Strings | Subject codes (lowercase!) |
| `periods` | No | List of Strings | Time slots |

**v1.1 -> v1.2 changes**: `termSourcedId` -> `termSourcedIds`; `SubjectCodes` -> `subjectCodes`

### 2.6 users.csv (v1.2 — MAJOR changes)

| Column | Required | PII | Format | Description |
|--------|----------|-----|--------|-------------|
| `sourcedId` | Yes | No | GUID | Unique ID |
| `status` | Yes for Delta | No | Enumeration | `active` / `tobedeleted` |
| `dateLastModified` | Yes for Delta | No | DateTime | ISO 8601 UTC |
| `enabledUser` | No | No | Enumeration | `true` / `false` |
| `username` | Yes | Identifier | String | Login username |
| `givenName` | Yes | Yes | String | First name |
| `familyName` | Yes | Yes | String | Last name |
| `middleName` | No | Yes | String | Middle name |
| `preferredGivenName` | No | Yes | String | Preferred first name |
| `preferredMiddleName` | No | Yes | String | Preferred middle name |
| `preferredFamilyName` | No | Yes | String | Preferred last name |
| `identifier` | No | Identifier | String | Student/staff ID |
| `email` | No | Yes | String | Email address |
| `grades` | No | No | List of Strings | Grade levels |
| `primaryOrgSourcedId` | No | No | GUID Reference | Primary org |
| `userMasterIdentifier` | No | Identifier | String | Master/district ID |
| `pronouns` | No | No | String | Pronouns |

**Columns REMOVED from v1.1**: `orgSourcedIds`, `role`, `agentSourcedIds`, `userIds`, `sms`, `phone`, `password`
**Columns ADDED**: `preferredGivenName`, `preferredMiddleName`, `preferredFamilyName`, `primaryOrgSourcedId`, `userMasterIdentifier`, `pronouns`

### 2.7 enrollments.csv (v1.2 updates)

| Column | Required | PII | Format | Description |
|--------|----------|-----|--------|-------------|
| `sourcedId` | Yes | No | GUID | Unique ID |
| `status` | Yes for Delta | No | Enumeration | `active` / `tobedeleted` |
| `dateLastModified` | Yes for Delta | No | DateTime | ISO 8601 UTC |
| `classSourcedId` | Yes | No | GUID Reference | Class |
| `schoolSourcedId` | Yes | No | GUID Reference | School org |
| `userSourcedId` | Yes | Identifier | GUID Reference | User |
| `role` | Yes | Yes | Enumeration | `administrator` / `proctor` / `student` / `teacher` |
| `primary` | No | No | Enumeration | `true` / `false` |
| `beginDate` | No | No | Date | Start date |
| `endDate` | No | No | Date | End date |
| `courseSourcedId` | No | No | GUID Reference | Course (NEW in v1.2) |

### 2.8 demographics.csv (v1.2 updates)

| Column | Required | PII | Format | Description |
|--------|----------|-----|--------|-------------|
| `sourcedId` | Yes | Identifier | GUID | User sourcedId |
| `status` | Yes for Delta | No | Enumeration | `active` / `tobedeleted` |
| `dateLastModified` | Yes for Delta | No | DateTime | ISO 8601 UTC |
| `birthDate` | No | Yes | Date | YYYY-MM-DD |
| `sex` | No | Yes | Enumeration | `male` / `female` / `unspecified` / `other` |
| `americanIndianOrAlaskaNative` | No | Yes | Enumeration | `true` / `false` |
| `asian` | No | Yes | Enumeration | `true` / `false` |
| `blackOrAfricanAmerican` | No | Yes | Enumeration | `true` / `false` |
| `nativeHawaiianOrOtherPacificIslander` | No | Yes | Enumeration | `true` / `false` |
| `white` | No | Yes | Enumeration | `true` / `false` |
| `demographicRaceTwoOrMoreRaces` | No | Yes | Enumeration | `true` / `false` |
| `hispanicOrLatinoEthnicity` | No | Yes | Enumeration | `true` / `false` |
| `countryOfBirthCode` | No | Yes | String | Country code |
| `stateOfBirthAbbreviation` | No | Yes | String | State abbreviation |
| `cityOfBirth` | No | Yes | String | City |
| `publicSchoolResidenceStatus` | No | Yes | String | Residence status |

### 2.9 roles.csv (NEW — v1.2 only)

| Column | Required | PII | Format | Description |
|--------|----------|-----|--------|-------------|
| `sourcedId` | Yes | No | GUID | Unique ID |
| `status` | Yes for Delta | No | Enumeration | `active` / `tobedeleted` |
| `dateLastModified` | Yes for Delta | No | DateTime | ISO 8601 UTC |
| `userSourcedId` | Yes | Identifier | GUID Reference | Reference to user |
| `role` | Yes | Yes | Enumeration | `administrator` / `proctor` / `student` / `teacher` |
| `orgSourcedId` | Yes | No | GUID Reference | Reference to org |
| `beginDate` | No | No | Date | Start of role |
| `endDate` | No | No | Date | End of role |

### 2.10 userProfiles.csv (NEW v1.2 — infrastructure only)

| Column | Required | Format | Description |
|--------|----------|--------|-------------|
| `sourcedId` | Yes | GUID | Unique ID |
| `status` | Yes for Delta | Enumeration | `active` / `tobedeleted` |
| `dateLastModified` | Yes for Delta | DateTime | ISO 8601 UTC |
| `profileType` | Yes | String | Vendor ID |
| `profileName` | Yes | String | Profile property name |
| `profileValue` | Yes | String | Profile property value |
| `userSourcedId` | Yes | GUID Reference | Reference to user |

### 2.11-2.22 Remaining new v1.2 files (skeletal support only)

| # | File | Columns |
|---|------|---------|
| 11 | `userResources.csv` | `sourcedId`, `status`, `dateLastModified`, `title`, `resourceSourcedId`, `userSourcedId` |
| 12 | `resources.csv` | `sourcedId`, `status`, `dateLastModified`, `title`, `role`, `importMetadata`, `availability`, `vendorId`, `applicationId`, `resourceType`, `identifier` |
| 13 | `classResources.csv` | `sourcedId`, `status`, `dateLastModified`, `title`, `classSourcedId`, `resourceSourcedId` |
| 14 | `courseResources.csv` | `sourcedId`, `status`, `dateLastModified`, `title`, `courseSourcedId`, `resourceSourcedId` |
| 15 | `categories.csv` | `sourcedId`, `status`, `dateLastModified`, `title`, `weight` |
| 16 | `lineItems.csv` | `sourcedId`, `status`, `dateLastModified`, `title`, `assignDate`, `dueDate`, `categorySourcedId`, `courseSourcedId`, `classSourcedId`, `academicSessionSourcedId`, `resultValueMin`, `resultValueMax`, `schoolSourcedId` |
| 17 | `lineItemLearningObjectiveIds.csv` | `sourcedId`, `status`, `dateLastModified`, `lineItemSourcedId`, `learningObjectiveId` |
| 18 | `lineItemScoreScales.csv` | `sourcedId`, `status`, `dateLastModified`, `lineItemSourcedId`, `scoreScaleSourcedId` |
| 19 | `scoreScales.csv` | `sourcedId`, `status`, `dateLastModified`, `title`, `scoreScalePoints`, `gradingPeriodSourcedId` |
| 20 | `results.csv` | `sourcedId`, `status`, `dateLastModified`, `lineItemSourcedId`, `studentSourcedId`, `score`, `textScore`, `scoreStatus`, `classSourcedId`, `enrollmentSourcedId`, `inProgress`, `incomplete`, `late`, `missing` |
| 21 | `resultLearningObjectiveIds.csv` | `sourcedId`, `status`, `dateLastModified`, `resultSourcedId`, `learningObjectiveId` |
| 22 | `resultScoreScales.csv` | `sourcedId`, `status`, `dateLastModified`, `resultSourcedId`, `scoreScaleSourcedId` |

---

## 3. New Files

| File | Purpose |
|------|---------|
| `Models/Interfaces/IRosterVersionStrategy.cs` | Strategy interface: `ManifestVersion`, `IncludesRoles`, `ProcessAllFiles()`, `GetManifestEntries()` |
| `Models/Strategies/V11RosterStrategy.cs` | v1.1 implementation — routes to `Models/Exports/` DTOs, 8 files |
| `Models/Strategies/V12RosterStrategy.cs` | v1.2 implementation — routes to `Models/Exports/V12/` DTOs, 9 files |
| `Models/RosterData.cs` | Record bundling all domain data for strategy consumption |
| `Models/Role.cs` | Domain model: sourcedId, status, dateLastModified, userSourcedId, role, orgSourcedId, beginDate, endDate |
| `Models/Roles.cs` | Generator: creates one `Role` per user-org-role (v1.2 only) |
| `Models/Exports/V12/RoleFile.cs` | Export DTO for `roles.csv` |
| `Models/Exports/V12/UserFile.cs` | v1.2 user CSV: new columns, no `orgSourcedIds`/`role` |
| `Models/Exports/V12/DemographicFile.cs` | v1.2 demographic CSV: same as current fixed v1.1 |
| `Models/Exports/V12/ClassFile.cs` | v1.2: `termSourcedIds` (plural) |
| `Models/Exports/V12/EnrollmentFile.cs` | v1.2: same as current (courseSourcedId already present) |
| `Models/Exports/V12/AcademicSessionFile.cs` | Same as v1.1 |
| `Models/Exports/V12/OrgFile.cs` | Same as v1.1 |
| `Models/Exports/V12/CourseFile.cs` | Same as v1.1 |
| `Models/Exports/V12/ManifestFile.cs` | Same shape as v1.1 |

---

## 4. Modified Files

| File | Changes |
|------|---------|
| `Models/Vocab.cs` | Add `OneRosterVersion` enum (`V1_1`, `V1_2`) |
| `OneRoster.cs` | Add `Version` to `Args` (default `V1_1`); instantiate strategy from version; use `RosterData` record; conditionally generate roles via `strategy.IncludesRoles`; pass strategy to `Manifests`; simplify `OutputCSVFiles()` to delegate to strategy |
| `Models/Interfaces/ILeaUser.cs` | Add nullable: `PreferredGivenName`, `PreferredMiddleName`, `PreferredFamilyName`, `PrimaryOrgSourcedId`, `UserMasterIdentifier`, `Pronouns` |
| `Models/User.cs` | Implement new interface members (all nullable, default null) |
| `Models/Manifests.cs` | Accept `IRosterVersionStrategy`; delegate `GetManifestEntries()` to strategy instead of hardcoding |
| `FileProcessor.cs` | No structural changes — strategy calls `ProcessFile<T1, T2>()` with correct types |
| `Consts.cs` | Add `AcademicSessionsFile`, `RolesFile` constants; add v1.2 file list |
| Various tests | Parameterize over version; add v1.2 column schema assertions |

---

## 5. File Dependency Matrix (Bulk Processing)

```
manifest.csv                     standalone
academicSessions.csv             standalone (self-ref via parentSourcedId)
orgs.csv                         standalone
courses.csv                      -> academicSessions.csv (schoolYearSourcedId)
                               -> orgs.csv (orgSourcedId)
users.csv                        (no direct file deps in v1.2; roles.csv handles org links)
classes.csv                      -> academicSessions.csv (termSourcedIds)
                               -> courses.csv (courseSourcedId)
                               -> orgs.csv (schoolSourcedId)
enrollments.csv                  -> classes.csv (classSourcedId)
                               -> orgs.csv (schoolSourcedId)
                               -> users.csv (userSourcedId)
demographics.csv                 -> users.csv (sourcedId = user sourcedId)
roles.csv                        -> users.csv (userSourcedId)
                               -> orgs.csv (orgSourcedId)
```

---

## 6. Implementation Order

1. Add `OneRosterVersion` enum to `Vocab.cs`
2. Update `ILeaUser.cs` / `User.cs` with optional v1.2 properties
3. Create `IRosterVersionStrategy.cs` interface
4. Create `RosterData.cs` record
5. Create `V11RosterStrategy.cs` (moves current `OutputCSVFiles` logic into strategy)
6. Create `V12RosterStrategy.cs` with v1.2 routing
7. Create all `Models/Exports/V12/*.cs` export DTO files
8. Create `Role.cs` domain model and `Roles.cs` generator
9. Update `Manifests.cs` to accept strategy
10. Update `OneRoster.cs` — add `Version` to Args, wire up strategy, add roles generation
11. Update `Consts.cs` with missing constants
12. Add v1.2 test coverage — parameterize existing tests over version
13. Verify: `dotnet build && dotnet test`

---

## 7. Backward Compatibility

- Default `V1_1` — zero consumer changes
- v1.2 opt-in: `new OneRoster(new Args { Version = OneRosterVersion.V1_2 })`
- `FileProcessor` unchanged — strategy handles routing
- All existing v1.1 export DTOs untouched

---

## 8. Extensibility

Adding a future version (e.g., v1.3) requires:
1. Add enum value to `OneRosterVersion`
2. Create `Models/Exports/V13/` folder with export DTOs
3. Create `V13RosterStrategy.cs` implementing `IRosterVersionStrategy`
4. Register in the version switch in `OneRoster.cs`

No changes to `FileProcessor`, `Manifests`, or any existing strategy.

---

## 9. Bug Fixes for v1.1 (already applied)

These bugs in the current v1.1 output have been corrected:
1. **DemographicFile.cs**: `nativeAmericanOrOtherPacificIslander` -> `nativeHawaiianOrOtherPacificIslander` (fixed)
2. **DemographicFile.cs**: Missing columns `white`, `demographicRaceTwoOrMoreRaces`, `hispanicOrLatinoEthnicity` (fixed)
3. **ClassFile.cs**: `SubjectCodes` -> `subjectCodes` (fixed)
4. **EnrollmentFile.cs**: Missing `courseSourcedId` column (fixed)
