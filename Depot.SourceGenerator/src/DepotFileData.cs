using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class DepotFileData
    {
        public string RawFileName {get; protected set;}
        public string WriteSafeName {get; protected set;}
        public List<SheetData> Sheets {get; protected set;}
        public string FileText {get; protected set;}
        public Dictionary<SheetData,List<SubsheetData>> SubsheetTree {get; protected set;}
        public DepotFileData(string fileName, string allText, JObject root)
        {
            RawFileName = fileName;
            WriteSafeName = File.SanitizeFilename(RawFileName);
            FileText = allText;
            Sheets = new List<SheetData>();
            var enumeratedSheets = root["sheets"] as JArray;
            //create the parent sheets first
            foreach (var sheet in enumeratedSheets.Where(x => !x["hidden"].Value<bool>()))
            {
                SheetData sheetData = new SheetData(sheet as JObject,this);
                Sheets.Add(sheetData);
            }
            foreach (var sheet in enumeratedSheets.Where(x => x["hidden"].Value<bool>()))
            {
                SubsheetData sheetData = new SubsheetData(sheet as JObject,this);
                Sheets.Add(sheetData);
            }
            foreach (var sheet in Sheets.Where(x => !(x is SubsheetData)))
            {
                //this is deferred so the Sheet list is full and we can reference sheets and lines by guids
                sheet.InitColumns();
            }
            foreach (var sheet in Sheets.Where(x => x is SubsheetData))
            {
                //this is deferred so the Sheet list is full and we can reference sheets and lines by guids
                sheet.InitColumns();
            }
            foreach (var sheet in Sheets.Where(x => !(x is SubsheetData)))
            {
                //this is deferred so the Sheet list is full and we can reference sheets and lines by guids
                sheet.InitLines();
            }

            SubsheetTree = new Dictionary<SheetData, List<SubsheetData>>();
            foreach (SubsheetData item in Sheets.Where(x => x is SubsheetData))
            {
                var parent = Sheets.Find(x => x.GUID == item.ParentSheetGUID);
                if(parent == null)
                {
                    // theres a bug in older versions of depot where deleting parent sheets didnt always delete the subsheets
                    // so the parent would be null here but the subsheet would exist
                    // we just log this and skip this subsheet
                    DepotSourceGenerator.Logs.Add($"unable to resolve parent sheet with guid {item.ParentSheetGUID} on subsheet with name {item.RawName} guid {item.GUID}");
                    continue;
                }
                if(!SubsheetTree.ContainsKey(parent))
                {
                    SubsheetTree.Add(parent,new List<SubsheetData>(){item});
                    continue;
                }
                SubsheetTree[parent].Add(item);
            }
        }

        public string GetPathToSheet(SheetData s)
        {
            if(!(s is SubsheetData)){return s.Name;}
            var b = "";
            var parent = Sheets.Find(x => x.GUID == s.JObject["parentSheetGUID"].Value<string>());
            b += $"{GetPathToSheet(parent)}.";
            b += s.IsProps ? $"{s.Name}Props" : $"{s.Name}List"; //only subsheets are props and lists
            return b;
        }
    }
}