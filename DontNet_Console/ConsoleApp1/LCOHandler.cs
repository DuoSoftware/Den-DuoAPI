using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class LCOHandler
    {
        private string entity;
        private string severname = "";

        public LCOHandler(string _Entity, string _server)
        {
            //GuaLCOID = _LCOID;
            entity = _Entity;
            severname = _server;
        }

        public List<string> LCOWithAccounts()
        {

            List<string> lcolsit = new List<string>();
            string str = RestAPICall.MakeRequest("http://" + severname + "/lco/withaccounts" , null, "GET", "application/json", typeof(Program), entity);
            try {
                JObject o = JObject.Parse(str);
                if ((Boolean)o["success"])
                {
                    var items= o["result"].ToList();
                    foreach (JToken val in items)
                    {
                        JObject v = val as JObject;
                        lcolsit.Add((string)v["GULCOID"]);
                    }
                    
                }
                else
                {
                    cmd.WriteLine("Error : " + (string)o["result"]);
                }
            }
            catch(Exception ex)
            {
                cmd.WriteLine("Error Retriving GULCOIDs : " + ex.Message, Info.Error);
            }
            return lcolsit;
        }

        public List<string> LCOPending()
        {

            List<string> lcolsit = new List<string>();
            string str = RestAPICall.MakeRequest("http://" + severname + "/lco/files/pending/appx003-090-"+entity, null, "GET", "application/json", typeof(Program), entity);
            try
            {
                JObject o = JObject.Parse(str);
                if ((Boolean)o["success"])
                {
                    var items = o["result"].ToList();
                    foreach (JToken val in items)
                    {
                        JObject v = val as JObject;
                        lcolsit.Add((string)v["GULCOID"]);
                    }

                }
                else
                {
                    cmd.WriteLine("Error : " + (string)o["result"]);
                }
            }
            catch (Exception ex)
            {
                cmd.WriteLine("Error Retriving GULCOIDs : " + ex.Message, Info.Error);
            }
            return lcolsit;
        }
    }
}
