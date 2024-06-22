using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Depot.SourceGenerator;

public static class AdditionalFileUtils
{
    //https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#access-analyzer-config-properties
    //https://github.com/dotnet/roslyn/blob/main/docs/features/source-generators.cookbook.md#consume-msbuild-properties-and-metadata
    public static List<(AdditionalText text, bool generateDepotSource)> GetLoadOptions(GeneratorExecutionContext context)
    {
        var l = new List<(AdditionalText text, bool generateDepotSource)>();
        foreach (AdditionalText file in context.AdditionalFiles)
        {
            context.AnalyzerConfigOptions.GetOptions(file).TryGetValue("build_metadata.AdditionalFiles.GenerateDepotSource", out string generateDepotSourceString);
            bool test = string.IsNullOrEmpty(generateDepotSourceString) ? false : generateDepotSourceString!.Equals("true", StringComparison.OrdinalIgnoreCase);
            l.Add((file,test));
        } 
        return l;
    }
}