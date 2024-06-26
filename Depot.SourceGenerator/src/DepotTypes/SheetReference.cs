using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class SheetReference : ColumnData
    {
        public override string CSharpType => "DepotSheet";
        public override string GetValue(LineData configuringLine, object o)
        {
            var value = o.ToString();
            return Utils.GetSheetDataPathFromGuid(configuringLine,this,value);
        }
        public SheetReference(JObject e, SheetData parentSheet) : base(e,parentSheet){}
    }
}