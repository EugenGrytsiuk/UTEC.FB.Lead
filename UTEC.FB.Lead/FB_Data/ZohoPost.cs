using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;

namespace UTEC.FB.Lead.FB_Data
{
    public class ZohoPost
    {
        //zoho not use
        private static string url =    "https://c106672b.ngrok.io/api/postlead"; 
        private static string owner =                               "me";    
        private static string tocken =       "123";   
        private string requestToZoho;

        public void readClientAsync()
        {
            //requestToZoho = "<add>"
            //      + "<field name='Unit'><value>" + Unit + "</value></field>"
            //      + "<field name='Date_field'><value>" + Date_field + "</value></field>"
            //      + "<field name='Client_name'><value>" + Client_name + "</value></field>"
            //      + "<field name='Phones'><value>" + Phones + "</value></field>"
            //      + "<field name='Email'><value>" + Email + "</value></field>"
            //      + "<field name='Status'><value>" + Status + "</value></field>"
            //      + "<field name='Source'><value>" + Source + "</value></field>"
            //      //+ "<field name='Channel'><value>" + Channel + "</value></field>"
            //      //+ "<field name='Weight_txt'><value>"+ Weight_txt+"</value></field>"
            //      + "<field name='Description'><value>Доставка" + "\n\n" + Description + "</value></field>"
            //  + "</add>";
            string xmlData = "<ZohoCreator><applicationlist><application name='clients'>"
                        + "<formlist><form name='Request'>"
                        + requestToZoho + "</form></formlist></application></applicationlist></ZohoCreator>";

            using (StreamWriter writer = new StreamWriter("C:\\Temp\\FB_To_Zoh.xml", true))
            {
                writer.WriteLineAsync(xmlData);
            }


        }

        public static string Postdata(string xmlData)
        {
            string postData = "scope=creatorapi";
            postData += "&authtoken=" + tocken;
            postData += "&zc_ownername=" + owner;
            postData += "&XMLString=" + HttpUtility.UrlEncode(xmlData);
            string result = InsertPostData(url, postData);

            return result;
        }

        public static string InsertPostData(string url, string postData)
        {
            string result = "";

            HttpWebRequest webRequest = null;
            HttpWebResponse response = null;
            webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.Timeout = 120000;
            webRequest.ContentType = "application/x-www-form-urlencoded";
            webRequest.Method = "POST";
            byte[] bytes = System.Text.Encoding.ASCII.GetBytes(postData);
            webRequest.ContentLength = bytes.Length;

            try
            {
                using (Stream requestStream = webRequest.GetRequestStream())
                {
                    requestStream.Write(bytes, 0, bytes.Length);
                    requestStream.Close();
                }
                response = (HttpWebResponse)webRequest.GetResponse();

                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                    sr.Close();
                }
            }
            catch (Exception ex)
            {
                using (StreamWriter stream = new StreamWriter("C:\\Temp\\zohoPost_err.txt"))
                {
                    stream.WriteLineAsync(ex.Message);
                }
            }
            finally
            {
                if (response != null)
                {
                    response.Close();
                }
            }

            using (StreamWriter stream = new StreamWriter("C:\\Temp\\zohoPost_result.txt"))
            {
                stream.WriteLineAsync(result);
            }
            return result;
        }
    }
}