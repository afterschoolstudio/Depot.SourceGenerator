using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class Props : ColumnData
    {
        public override string CSharpType => handlePropsReference();
        public override string GetValue(LineData configuringLine, object o)
        {
            var value = o.ToString();
            return !string.IsNullOrEmpty(value) ? BuildPropsCtor(configuringLine,value,JObject["sheet"].Value<string>(),this) : "null"; //todo - this is just an object sonstructo;
        }
        string handlePropsReference()
        {
            var sheetGUID = JObject["sheet"].Value<string>();
            return ParentSheet.ParentDepotFile.GetPathToSheet(Utils.GetSheetDataFromGUID(this,sheetGUID));
        }
        public Props(JObject e, SheetData parentSheet) : base(e,parentSheet){}
        public string BuildPropsCtor(LineData configuringLine, string v, string propsSheetGuid, ColumnData parentColumn)
        {
            var values = new List<string>();
            var sheet = Utils.GetSheetDataFromGUID(this,propsSheetGuid);
            var path = ParentSheet.ParentDepotFile.GetPathToSheet(sheet);
            var data = JObject.Parse(v);
            if(!data.HasValues){return "null";}
            var reqColumns = new List<ColumnData>(sheet.Columns);
            foreach (var prop in data.Properties().OrderBy(x=>x.Name))
            {
                if(prop.Name == "guid")
                {
                    values.Add(string.Format(@"""{0}""",prop.Value.Value<string>()));
                    continue;
                }
                var typeColumn = sheet.Columns.Find(x => x.RawName == prop.Name);
                if(typeColumn == null)
                {
                    //no line has been selected, return null
                    DepotSourceGenerator.Logs.Add($"WARN: unable to find matching column for line data key {prop.Name} with value {prop.Value.Value<string>()} on line with id {configuringLine.ID}, data will not be reflected in source");
                    continue;
                }
                if(typeColumn is Props p)
                {
                    values.Add(p.BuildPropsCtor(configuringLine,prop.Value.ToString(),typeColumn.JObject["sheet"].Value<string>(),this));
                }
                else if(typeColumn is List li)
                {
                    values.Add(li.BuildListCtor(configuringLine,prop.Value.ToString(),typeColumn.JObject["sheet"].Value<string>(),this));
                }
                else
                {
                    values.Add(typeColumn.GetValue(configuringLine,prop.Value.Value<object>()));
                }
                reqColumns.Remove(typeColumn);
            }
            if(reqColumns.Count > 0)
            {
                //this means the data on this object lacked some of the values needed for this colum
                //this happens sometimes because of large data changes in depot or just old depot files having sort of broken data
                //we emit a report here to the logs so you can fix your data (often by just adding in a value at the problem path and saving)
                foreach (var item in reqColumns.Where(x => !(x is Id || x is Guid)))
                {
                    //note we do this hacky parent path thing because we sometimes hack depot parent sheet references to re-used column configs
                    //in cantata, we use BuildableInteractables.ProjectInfo for all project types, and instead of redefining it we create the props column
                    //and then manually change the guid to the BuildableInteractables.ProjectInfo guid, allowing us to reuse that config
                    //however this means if you used item.ParentSheet, all projects would point to the path of BuildableInteractables.ProjectInfo
                    //this is _technically_ correct, but isnt useful as a log statement, as it accidentally obscures the path you actually care about
                    //so instead we pass in the parent column here and use that as a way to get where this is actually happening
                    DepotSourceGenerator.Logs.Add($"ERROR: {configuringLine.ID} at {parentColumn.ParentSheet.DataPath}.{item.ParentSheet.RawName} did not have a value for {item.RawName}, ctor will be broken");
                }
            }
            return $"new {path}({string.Join(",",values)})";
        }
    }
}