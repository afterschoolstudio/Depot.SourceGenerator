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
        //this basically searches to see if we want to actually generate depot source for a file
        //could also add in other options here
        List<(string,bool)> meta = new ();
        foreach (AdditionalText file in context.AdditionalFiles)
        {
            context.AnalyzerConfigOptions.GetOptions(file).TryGetValue("build_metadata.AdditionalFiles.GenerateDepotSource", out string generateDepotSourceString);
            bool test = false;
            if(string.IsNullOrEmpty(generateDepotSourceString))
            {
                test = false;
            }
            else
            {
                test = generateDepotSourceString!.Equals("true", StringComparison.OrdinalIgnoreCase);
                meta.Add((file.Path,test));
            }
            l.Add((file,test));
        } 
        context.AddSource("additionalFiles.txt",$"/*\n{string.Join("\n",meta)}\n*/");
        return l;
    }
}