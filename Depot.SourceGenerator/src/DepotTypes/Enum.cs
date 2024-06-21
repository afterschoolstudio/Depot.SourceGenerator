using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class Enum : ColumnData, IRequiresIntermediateType
    {
        public override string CSharpType => $"{Name}_ENUM";
        public override string GetValue(LineData configuringLine, object o)
        {
            var value = o.ToString();
            return $"{ParentSheet.DataPath}.{CSharpType}.{value}";
        }
        public Enum(JObject e, SheetData parentSheet) : base(e,parentSheet){}
        public void BuildType(Utils.CodeWriter cw, SheetData d)
        {
            var enumValues = new List<string>();
            enumValues.AddRange(JObject["options"].Value<string>().Split(',').ToList());
            if(!enumValues.Contains("None"))
            {
                enumValues.Insert(0,"None");
            }
            cw.OpenScope($"public enum {CSharpType}");
            List<string> lines = new List<string>();
            enumValues.ForEach(value => lines.Add(value.Replace(" ","") + ","));
            lines.Last().TrimEnd(',',' ');
            foreach (var item in lines)
            {
                cw.AddLine(item);
            }
            cw.CloseScope();
        }
    }
}