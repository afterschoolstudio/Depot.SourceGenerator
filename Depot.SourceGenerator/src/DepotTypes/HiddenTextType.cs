using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace Depot.SourceGenerator
{

    public abstract class HiddenDataType : Text //hidden types are only raw text guid and id
    {
        public class SpoofedColumnData
        {
            public string name {get;}
            public string guid {get;} = "none";
            public SpoofedColumnData(string name)
            {
                this.name = name;
            }
            public JObject ToJsonElement()
            {
                return JObject.Parse(JsonConvert.SerializeObject(this));
            }
        }
        public override string GetValue(LineData configuringLine, object o)
        {
            var value = o.ToString();
            return string.Format(@"""{0}""",value);
        }
        public HiddenDataType(string name, SheetData parentSheet) : 
            base (new SpoofedColumnData(name).ToJsonElement(),parentSheet)
        {
        }
    }

    public class Guid : HiddenDataType
    {
        public Guid(SheetData parentSheet) : base("guid",parentSheet){}
    }

    public class Id : HiddenDataType
    {
        public Id(SheetData parentSheet) : base("id",parentSheet){}
    }
}