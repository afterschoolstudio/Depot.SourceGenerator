using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class Float : ColumnData
    {
        public override string CSharpType => "float";
        public override string GetValue(LineData configuringLine, object o)
        {
            return o.ToString() + "f";
        }
        public Float(JObject e, SheetData parentSheet) : base(e,parentSheet){}
    }
}