using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class Image : FileColumn
    {
        public Image(JObject e, SheetData parentSheet) : base(e,parentSheet){}
    }
}