using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;
using THttp_req;
using System.Reflection;
using System.Threading;

namespace UTEC.FB.Lead.Controllers
{
    public class ValuesController : ApiController
    {


        [Route("api/webhooks")]
        [HttpGet]
        public int GetnFWh()
        {
            var chalange = Request.RequestUri;
            var uri = new Uri(chalange.ToString());

            int chl = int.Parse(HttpUtility.ParseQueryString(uri.Query).Get("hub.challenge"));

            return chl;
        }

        [Route("api/webhooks")]
        [HttpPost]
        public HttpResponseMessage GetLeadPost()
        {
            
           
            try
            {
                
                var jsled = Request.Content.ReadAsStringAsync().Result;
                var readObj = JsonConvert.DeserializeObject(jsled);
                using (StreamWriter fstream = new StreamWriter(@"C:\Temp\logFB_1.json", true, Encoding.Default))
                {
                    fstream.WriteLine("\n" + readObj);
                    fstream.Close();
                }
                Thread myth = new Thread(ssta);
                myth.Start();
                void ssta()
                {
                    FB_Lead lead = new FB_Lead();
                    lead.readLead(jsled);
                }
                

            }
            catch (Exception e)
            {
                using (StreamWriter streamWriter = new StreamWriter("C:\\Temp\\error_FB.txt"))
                {
                    streamWriter.WriteLineAsync(e.Message);
                }
            }
            return Request.CreateResponse(
                    HttpStatusCode.OK);
        }
    }
}
