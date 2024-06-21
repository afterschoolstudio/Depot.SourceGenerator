using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class FileColumn : ColumnData
    {
        public override string CSharpType => "FileInfo";
        public override string GetValue(LineData configuringLine, object o)
        {
            var value = o.ToString();
            return !string.IsNullOrEmpty(value) ? "new FileInfo(" + string.Format(@"""{0}""",value.Replace("\\","/")) + ")" : "null"; //todo - byte[];
        }
        public FileColumn(JObject e, SheetData parentSheet) : base(e,parentSheet){}
    }
}