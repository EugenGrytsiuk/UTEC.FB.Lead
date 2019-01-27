using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Xml.Linq;
using UTEC.FB.Lead.App_Start;
using UTEC.FB.Lead.Data;
using UTEC.FB.Lead.FB_Data;



namespace THttp_req
{
    public class FB_Lead
    {
        private string token = "";
        private string Idrequest = "https://graph.facebook.com/v3.2/";
        private long id_lead;          //client info
        private long id_lead_reklam;   //name form of advertising
        private long id_ads_reklam;    //info about advertising, not yet

        public static xmlData XmlData = new xmlData();
        private string zohoxmlData;
        //private string Unit;
        //private string Date_field = $"{DateTime.UtcNow.ToShortDateString()}";
        //private string Client_name;
        //private string Phones;
        //private string Email;
        //private string Status = "Не связывались";
        //private string Source = "mail";
        //private string Weight_txt = "1.0";
        //private string Description;
        //private string Channel;

        public void readLead(string json)
        {
            var readjson = JsonConvert.DeserializeObject<EntryArr>(json);

            foreach (var obj in readjson.entry)
            {
                foreach (var obj2 in obj.changes)
                {
                    id_lead = (long)obj2.value.leadgen_id;
                    id_lead_reklam = (long)obj2.value.form_id;
                    id_ads_reklam = (long)obj2.value.ad_id;  //after ads_manage on FB  
                    break;
                }
            }

            //duplicate check
            if (CheckLead(id_lead))
            {

            }
            else
            {
                SaveLead(id_lead);
                readClient(id_lead);
                readADVS(id_lead_reklam);
                EnableSecurityProtocol();
                BildRequestData();
                ZohoPost.Postdata(zohoxmlData);

            }
   
        }

        public string BildRequestData()
        {

            string requestToZoho = "<add>"
                  + "<field name='Unit'><value>" + XmlData.unit + "</value></field>"
                  + "<field name='Date_field'><value>" + XmlData.date_field + "</value></field>"
                  + "<field name='Client_name'><value>" + XmlData.client_name + "</value></field>"
                  + "<field name='Phones'><value>" + XmlData.phones + "</value></field>"
                  + "<field name='Email'><value>" + XmlData.email + "</value></field>"
                  + "<field name='Status'><value>" + XmlData.status + "</value></field>"
                  + "<field name='Source'><value>" + XmlData.source + "</value></field>"
                  + "<field name='Channel'><value>" + XmlData.channel + "</value></field>"
                  //+ "<field name='Weight_txt'><value>" + XmlData.weight_txt + "</value></field>"
                  + "<field name='Description'><value>Доставка" + "\n\n" + XmlData.description + "</value></field>"
              + "</add>";
             zohoxmlData = "<ZohoCreator><applicationlist><application name='clients'>"
                        + "<formlist><form name='Request'>"
                        + requestToZoho + "</form></formlist></application></applicationlist></ZohoCreator>";
            return zohoxmlData;
        }


        public class xmlData
        {
            public string unit = "Utec";
            public string date_field = $"{DateTime.UtcNow.ToShortDateString()}";
            public string client_name;
            public string phones;
            public string email;
            public string status = "Не связывались";
            public string source = "other";
            public string weight_txt = "666666";
            public string description;
            public long channel = 968696000172193483;
        }

        private static void EnableSecurityProtocol()
        {
            if (!ServicePointManager.SecurityProtocol.HasFlag(SecurityProtocolType.Tls12))
            {
                ServicePointManager.SecurityProtocol = ServicePointManager.SecurityProtocol | SecurityProtocolType.Tls12;
            }
        }

        public class IdLeads
        {
            public long IdInLeads { get; set; }
        }

        public static bool CheckLead(long idlead)
                    => GetAllLead().Where(x => x.IdInLeads == idlead).FirstOrDefault() != null;

        public static List<IdLeads> GetAllLead()
        {
            var xdoc = XDocument.Load(@"C:\Users\e.grytsiuk\source\repos\UTEC.FB.Lead\UTEC.FB.Lead\Id_Lead.xml");

            return (from mes in xdoc.Root.Descendants("IdLead")
                    select new IdLeads
                    {
                        IdInLeads =
                        long.Parse(
                            mes.Element(
                                "lead_id").Value),
                    }).ToList();

        }

        public static void SaveLead(long idLead)
        {
            var xdoc = XDocument.Load(@"C:\Users\e.grytsiuk\source\repos\UTEC.FB.Lead\UTEC.FB.Lead\Id_Lead.xml");

            xdoc.Root.Add(new XElement("IdLead",
                new XElement("lead_id", idLead)));
            xdoc.Save(@"C:\Users\e.grytsiuk\source\repos\UTEC.FB.Lead\UTEC.FB.Lead\Id_Lead.xml");

        }



        public void readClient(long idFromLead)
        {
            WebRequest request = WebRequest.Create(Idrequest + id_lead + "?access_token=" + token);
            WebResponse response = request.GetResponse();
            string responeString;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader read = new StreamReader(stream))
                {
                    responeString = read.ReadToEnd();
                    if (responeString.StartsWith("{"))
                    {
                        responeString = responeString.Insert(0, "[");
                        responeString = responeString.Insert(responeString.Length, "]");
                    }
                    else
                    {
                        //continue;
                    }
                    read.Close();
                }
            }

            var readjson = JsonConvert.DeserializeObject<List<RootObj>>(responeString);
            foreach (var item in readjson)
            {

                foreach (var p in item.field_data)
                {
                    if (p.name == "full_name")
                    {
                        foreach (var ii in p.values)
                        {
                            XmlData.client_name = ii.ToString();
                        }
                    }
                    if (p.name == "phone_number")
                    {
                        foreach (var ii in p.values)
                        {
                            XmlData.phones = ii.ToString();
                        }
                    }
                    if (p.name == "email")
                    {
                        foreach (var ii in p.values)
                        {
                            XmlData.email = ii.ToString();
                        }
                    }
                }
            }
        }

        public void readADVS(long id_reklam)
        {
            WebRequest request = WebRequest.Create(Idrequest + id_reklam + "?access_token=" + token);
            WebResponse response = request.GetResponse();
            string responeString;
            using (Stream stream = response.GetResponseStream())
            {
                using (StreamReader read = new StreamReader(stream))
                {
                    responeString = read.ReadToEnd();
                    if (responeString.StartsWith("{"))
                    {
                        responeString = responeString.Insert(0, "[");
                        responeString = responeString.Insert(responeString.Length, "]");
                    }
                    else
                    {
                        //continue;
                    }
                    read.Close();
                }
            }
            var readID = JsonConvert.DeserializeObject<List<FromAdvertising>>(responeString);
            foreach (var ob in readID)
            {
                XmlData.description = ob.name;
            }
        }
    }
}
