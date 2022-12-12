# OneRoster Sample Data Generator

This class library can be utilized to generate randomly generated data or in-memory construct(s) of [OneRoster](http://www.imsglobal.org/oneroster-v11-final-csv-tables) roster files for educational applications.

## Description

The objects generated in this class can be used for a number of things.  They can be outputted to to `Csv` and `OneRoster` file types.  They are generated and well-formed OneRoster files that match the `1.1` spec of OneRoster.  The in-memory construct can also be used to import into various systems, test-systems, and other EdTech applications.

Creates
--
* Academic Sessions
* Classes
* Courses
* Demographics
* Enrollments
* Grades
* Manifest
* Org
* Staff
* Students

> School Year generated runs from 8/16/{currentYear} to  8/15/{nextSchoolYear}

Semesters (Terms)
--
* Full Year
* MP1
* MP2
* MP3
* MP4
* Semester 1
* Semester 2
* Summer Semester

## Getting Started

### Dependencies

* NET 6
* Bogus 34.0.1

### Installing

Copy generated dll into your bin

### Executing program

#### Simpliest Execution

```csharp
	var oneRoster = new OneRoster();
```

#### Generating Files

```csharp
	OneRoster oneRoster = new();
	oneRoster.OutputCSVFiles();
	oneRoster.OutputOneRosterZipFile();
```

> This generates the needed OneRoster files to current application (AppDomain.CurrentDomain.BaseDirectory) path `OneRoster` folder.
> Then compiles the files into a `OneRoster.zip` file

```csharp
    OneRoster oneRoster = new(schoolCount: 3);
```
> The ability to set the number of generated school.  The default is `22`.

More documentation to come.

## Help

Please add an issue to this project

https://github.com/bergerb/OneRosterSampleDataGenerator/issues

## Authors

Brent Berger
[hachyderm.io/@bergerb](https://hachyderm.io/@bergerb)

## Version History

* TBD
    * Current Release

## License

This project is licensed under the `Unlicense license` License
