# Depot.SourceGenerator
source generator for depot files

sheetref doesnt currently work
could lineref just point directly at the static line?
can the whole thing be static? can we use a big record?

depot requires that consuming libs use the props file (packed in nuget) to be able to annotate additionalfiles with the `GenerateDepotSource="true"` metadata.

this works automatically if the project itself consumes this and uses it, but if you consume this in a project that is then referenced by another project, you need to either declare the package reference in your project as:
`<PackageReference Include="Depot.SourceGenerator" Version="1.0.5" PrivateAssets="None"/>`

or in the parent dir that holds the two projects, create a Directory.Build.props file with the following content:
```
<Project>
    <ItemGroup>
        <CompilerVisibleProperty Include="GenerateDepotSource" />
        <CompilerVisibleItemMetadata Include="AdditionalFiles" MetadataName="GenerateDepotSource" />
    </ItemGroup>
</Project>
```