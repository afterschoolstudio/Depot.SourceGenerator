# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Depot.SourceGenerator is a C# source generator that processes Depot (.dpo) files and generates strongly-typed C# classes for game data management. It parses JSON-formatted Depot files containing sheets, columns, and lines, then generates corresponding C# types with proper references and relationships.

## Building and Testing

Build the entire solution:
```bash
dotnet build Depot.SourceGenerator.sln
```

Build only the generator:
```bash
dotnet build Depot.SourceGenerator/Depot.SourceGenerator.csproj
```

Build the sample project (which references the generator):
```bash
dotnet build Depot.SourceGenerator.Sample/Depot.SourceGenerator.Sample.csproj
```

Run the sample project:
```bash
dotnet run --project Depot.SourceGenerator.Sample/Depot.SourceGenerator.Sample.csproj
```

View generated files: Generated source files appear in `Depot.SourceGenerator.Sample/obj/Debug/net8.0/generated/Depot.SourceGenerator/`

## Architecture

### Source Generation Pipeline

1. **Entry Point** (`GeneratorEntry.cs`): Implements `ISourceGenerator`, filters AdditionalFiles for `GenerateDepotSource="true"` metadata
2. **File Parsing** (`DepotFileData.cs`): Parses .dpo JSON files, builds sheet hierarchy including subsheet relationships
3. **Code Generation** (`DepotSourceGenerator.cs`): Orchestrates code generation for sheets, lines, and references
4. **Type System** (`src/DepotTypes/*.cs`): 14 column types (bool, enum, file, float, grid, image, int, lineReference, list, longtext, multiple, props, sheetReference, text)

### Key Components

- **Sheets**: Top-level data containers (becomes `DepotSheet` subclasses)
- **Subsheets**: Nested sheets (Props or Lists) that belong to parent sheets, tracked via `SubsheetTree`
- **Lines**: Individual data rows in sheets (becomes `DepotSheetLine` instances)
- **Columns**: Define the schema and types for each field
- **LineReferences**: Special handling to prevent circular reference issues by using indirection pattern with `LineReference` wrapper classes

### Generated Code Structure

For each sheet, the generator creates:
- A `{SheetName}` class extending `DepotSheet`
- A `{SheetName}Line` class extending `DepotSheetLine`
- A `{SheetName}LineReference` class for lazy-loaded references
- Static instances for each line in the sheet
- A static `Lines` collection of all line instances

Props (property sheets) generate simpler `{SheetName}Props` classes without the line infrastructure.

### MSBuild Integration

The generator requires MSBuild metadata to identify which .dpo files to process. Consuming projects must:

1. Add `<AdditionalFiles Include="file.dpo" GenerateDepotSource="true" />` in their .csproj
2. Either:
   - Reference the package with `PrivateAssets="None"` to flow props to dependent projects, OR
   - Create a `Directory.Build.props` with `CompilerVisibleProperty` and `CompilerVisibleItemMetadata` declarations (see README.md)

The `Depot.SourceGenerator.props` file (packed into the NuGet package) makes these metadata items visible to the compiler.

## Important Notes

- The generator targets `netstandard2.0` for compatibility with Roslyn
- Newtonsoft.Json is bundled with the generator assembly (packed into `analyzers/dotnet/cs`)
- LineReferences use lazy initialization to handle circular dependencies between sheets
- Subsheets are processed after parent sheets to ensure proper GUID resolution
- Known issue: sheetref doesn't currently work (per README.md)
- Core runtime types (`DepotSheet`, `DepotSheetLine`, etc.) are generated once via `ConstantSourceFiles.Core`

## Debugging Source Generators

Use the launch settings profile in `Properties/launchSettings.json` to debug the generator itself. Generated code can be inspected in the consuming project's `obj/Debug/{tfm}/generated/` directory.
