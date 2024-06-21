using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class Int : ColumnData
    {
        public override string CSharpType => "int";
        public override string GetValue(LineData configuringLine, object o)
        {
            return o.ToString();
        }
        public Int(JObject e, SheetData parentSheet) : base(e,parentSheet){}
    }
}