using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class LongText : Text
    {
        public LongText(JObject e, SheetData parentSheet) : base(e,parentSheet){}
    }
}