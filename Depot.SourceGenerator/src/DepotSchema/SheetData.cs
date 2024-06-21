using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public class SheetData
    {
        public string RawName {get;}
        public string Name {get;}
        public string Description {get;}
        public string GUID {get;}
        public bool Hidden {get;}
        public bool IsProps {get;}
        public DepotFileData ParentDepotFile {get;}
        public JObject JObject {get;}
        public List<LineData> Lines {get; protected set;}
        public List<ColumnData> Columns {get; protected set;}
        public string DataPath => ParentDepotFile.GetPathToSheet(this);
        public SheetData(JObject e, DepotFileData parentDepotFile)
        {
            JObject = e;
            ParentDepotFile = parentDepotFile;
            RawName = e["name"].Value<string>();
            Name = File.SanitizeFilename(RawName);
            Description = e["description"].Value<string>();
            GUID = e["guid"].Value<string>();
            Hidden = e["hidden"].Value<bool>();
            IsProps = false; //old sheets dont have this already defined (but those sheets were also never props)
            if(e["isProps"].HasValues)
            {
                IsProps = e["isProps"].Value<bool>();
            }
        }

        public void InitColumns()
        {
            Columns = new List<ColumnData>(){
                new Guid(this)
            };
            if(!IsProps)
            {
                Columns.Add(new Id(this));
            }
            foreach (var column in JObject["columns"])
            {
                var type = DepotSourceGenerator.DepotTypeDict[column["typeStr"].Value<string>()];
                var cd = (ColumnData) Activator.CreateInstance(type,column as JObject,this);
                Columns.Add(cd);
            }

            Columns = Columns.OrderBy(x => x.Name).ToList();
        }
        
        public void InitLines()
        {
            Lines = new List<LineData>();
            foreach (var line in JObject["lines"])
            {
                Lines.Add(new LineData(line as JObject,this));
            }
        }
    }

    public class SubsheetData : SheetData
    {
        public string ParentSheetGUID {get;}
        public string ColumnGUID {get;}
        public SubsheetData(JObject e, DepotFileData d) : base(e,d)
        {
            ParentSheetGUID = e["parentSheetGUID"].Value<string>();
            ColumnGUID = e["columnGUID"].Value<string>();
        }

    }
}