using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace Depot.SourceGenerator;

[Generator]
public class GeneratorEntry : ISourceGenerator
{
    public void Initialize(GeneratorInitializationContext context)
    {
        // No initialization required for this generator.
    }

    public void Execute(GeneratorExecutionContext context)
    {
        bool generatedConst = false;
        var possibleFiles = AdditionalFileUtils.GetLoadOptions(context).Where(x => x.generateDepotSource);
        // foreach (var f in possibleFiles)
        // {
        //     //NOTE: Up to user to make sure they are passing in a valid depot file here
        //     if (!generatedConst)
        //     {
        //         context.AddSource($"Depot.Core.cs", ConstantSourceFiles.Core);
        //         generatedConst = true;
        //     }
        //     DepotSourceGenerator.GenerateSource(f.text, context);
        // }
    }
}