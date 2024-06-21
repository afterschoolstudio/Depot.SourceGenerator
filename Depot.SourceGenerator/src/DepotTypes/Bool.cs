using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class Bool : ColumnData
    {
        public override string CSharpType => "bool";
        public override string GetValue(LineData configuringLine, object o)
        {
            return o.ToString().ToLower();
        }
        public Bool(JObject e, SheetData parentSheet) : base(e,parentSheet){}
    }
}