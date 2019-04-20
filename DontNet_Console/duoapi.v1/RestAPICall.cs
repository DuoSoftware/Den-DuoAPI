using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace duoapi.v1
{
    class RestAPICall
    {
        private string Apikey;
        private string UserName;
        private string Entity;
        private string APIUri= "http://172.16.8.85/html/";

        public RestAPICall(string apikey,string username,string entity)
        {
            Apikey = apikey;
            UserName = username;
            Entity = entity;
        }
        public  T Get<T>(string requestURI)
        {
            string str = MakeRequest(APIUri + requestURI, "", "GET", "application/json", Entity);
            T results = JsonConvert.DeserializeObject<T>(str);
            return results;
        }


        public T POST<T>(string requestURI,object PostObj)
        {
            string Reqjson= JsonConvert.SerializeObject(PostObj);
            string str = MakeRequest(APIUri + requestURI, Reqjson, "POST", "application/json", Entity);
            T results = JsonConvert.DeserializeObject<T>(str);
            return results;
        }

        private  string MakeRequest(string requestUrl, string JSONRequest, string JSONmethod, string JSONContentType,  string Entity)
        {

            try
            {

                HttpWebRequest request = WebRequest.Create(requestUrl) as HttpWebRequest;
                //WebRequest WR = WebRequest.Create(requestUrl);   
                request.Headers["entity"] = Entity;
                var sb = JSONRequest;// JsonConvert.SerializeObject(JSONRequest); 
                request.Method = JSONmethod;
                if (JSONmethod == "POST")
                {
                    request.ContentType = JSONContentType; // "application/json";   

                    Byte[] bt = Encoding.UTF8.GetBytes(sb);
                    Stream st = request.GetRequestStream();
                    st.Write(bt, 0, bt.Length);
                    st.Close();
                }


                using (HttpWebResponse response = request.GetResponse() as HttpWebResponse)
                {

                    if (response.StatusCode != HttpStatusCode.OK) throw new Exception(String.Format(
                        "Server error (HTTP {0}: {1}).", response.StatusCode,
                    response.StatusDescription));

                    // DataContractJsonSerializer jsonSerializer = new DataContractJsonSerializer(typeof(Response));// object objResponse = JsonConvert.DeserializeObject();Stream stream1 = response.GetResponseStream();   
                    //Stream stream1 =new Stream();
                    StreamReader sr = new StreamReader(response.GetResponseStream());
                    string strsb = sr.ReadToEnd();
                    //object objResponse = JsonConvert.DeserializeObject(strsb, JSONResponseType);
                    Console.WriteLine("End -> Request: " + DateTime.Now.ToString() + " - Remaining " + requestUrl);
                    Console.WriteLine("---------------------------------------------------");
                    return strsb;
                }
            }
            catch (Exception e)
            {

               throw e;
            }
        }

        //public 
    }
}
