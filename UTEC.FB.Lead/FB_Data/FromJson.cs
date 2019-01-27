using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace UTEC.FB.Lead.Data
{
    public class EntryArr//#1json with lead id
    {
        public List<Entry> entry { get; set; }
        [JsonProperty("object")]
        public string _object { get; set; }
        public override string ToString()
        {
            Newtonsoft.Json.Formatting jsonFormatting = Newtonsoft.Json.Formatting.Indented;
            JsonSerializerSettings jsonSerializerSettings = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            return JsonConvert.SerializeObject(this, jsonFormatting, jsonSerializerSettings);
        }
    }

    public class Entry//#1
    {
        public List<Change> changes { get; set; }
        public string id { get; set; }
        public int time { get; set; }
    }

    public class Change//#1
    {
        public string field { get; set; }
        public Value value { get; set; }
    }

    public class Value//#1
    {
        public long ad_id { get; set; }
        public long form_id { get; set; }
        public long leadgen_id { get; set; }
        public long created_time { get; set; }
        public long page_id { get; set; }
        public long adgroup_id { get; set; }
    }

    public class RootObj//#2json lead info
    {
        public string created_time { get; set; }
        public Int64 id { get; set; }
        public List<FieldData> field_data { get; set; }
    }

    public class FieldData//#2
    {
        public string name { get; set; }
        public List<string> values { get; set; }
    }

    public class FromAdvertising//#3 info from Advertising(reklama)
    {
        public string form_id { get; set; }
        public string export_url { get; set; }
        public string locate { get; set; }
        public string name { get; set; }
        public string status { get; set; }
    }
}