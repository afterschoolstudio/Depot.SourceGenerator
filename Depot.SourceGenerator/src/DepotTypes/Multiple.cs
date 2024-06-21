using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;


namespace Depot.SourceGenerator
{
    public class Multiple : ColumnData, IRequiresIntermediateType
    {
        public override string CSharpType => $"{Name}_FLAGS";
        public override string GetValue(LineData configuringLine, object o)
        {
            var value = o.ToString();
            if( !string.IsNullOrEmpty(value) && 
                ((JArray)o).HasValues && 
                !string.IsNullOrEmpty(((JArray)o).First.Value<string>()))
                {
                    var cat = new List<string>();
                    foreach (var item in (JArray)o)
                    {
                        cat.Add($"{ParentSheet.DataPath}.{CSharpType}.{item.Value<string>()}");
                    }
                    return string.Join("|",cat);
                }
            return $"{ParentSheet.DataPath}.{CSharpType}.None";
        }
        public Multiple(JObject e, SheetData parentSheet) : base(e,parentSheet){}
        public void BuildType(Utils.CodeWriter cw, SheetData d)
        {
            var index = 0;
            var enumValues = JObject["options"].Value<string>().Split(',').ToList();
            cw.AddLine($"[Flags]");
            cw.OpenScope($"public enum {CSharpType}");
            List<string> lines = new List<string>();
            lines.Add("None = 0,");
            enumValues.ForEach(value => {lines.Add(value.Replace(" ","") + $" = {Math.Pow(2,index)}" + ","); index++;});
            lines.Last().TrimEnd(',',' ');
            foreach (var item in lines)
            {
                cw.AddLine(item);
            }
            cw.CloseScope();
        }
    }
}