using System;
using System.Linq;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;

namespace Depot.SourceGenerator
{
    public abstract class ColumnData
    {
        public string Name {get;}
        public string RawName {get;}
        public string GUID {get;}
        public JObject JObject { get;}
        public SheetData ParentSheet {get;}
        public DepotFileData ParentFile => ParentSheet.ParentDepotFile;
        public ColumnData(JObject e, SheetData parentSheet)
        {
            JObject = e;
            ParentSheet = parentSheet;
            RawName = e["name"].Value<string>();
            Name = File.SanitizeFilename(RawName);
            GUID = e["guid"].Value<string>();
        }
        /// <summary>
        /// This type is used to build the constructors for a type
        /// </summary>
        /// <value></value>
        public abstract string CSharpType {get;}
        /// <summary>
        /// This wraps the passed in object value as the type of this column. This is used as the actual constructor values.
        /// </summary>
        /// <param name="configuringLine">Parent confguring line</param>
        /// <param name="o">Value from Depot</param>
        /// <returns>String value to be placed directly in constructor</returns>
        public abstract string GetValue(LineData configuringLine, object o);
    }

    public interface IRequiresIntermediateType
    {
        void BuildType(Utils.CodeWriter cw, SheetData d);
    }
}

