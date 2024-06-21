using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class LineData
    {
        public string WriteSafeID {get;}
        public string ID {get;}
        public string GUID {get;}
        public JObject JObject {get;}
        public SheetData ParentSheet {get;}
        public LineData(JObject e, SheetData parentSheet)
        {
            JObject = e;
            ID = e["id"].Value<string>();
            WriteSafeID = File.SanitizeFilename(ID);
            GUID = e["guid"].Value<string>();
            ParentSheet = parentSheet;
        }
    }
}