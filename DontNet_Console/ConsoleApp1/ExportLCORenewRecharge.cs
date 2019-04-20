using Newtonsoft.Json.Linq;
using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class ExportLCORenewRecharge
    {
        private string GuaLCOID;
        private string entity;
        private  TextWriter sw;
        private TextWriter swDayExp7;
        private TextWriter swexp;
        private int RecordCount=0;
        private int LastID = 0;
        private  string FileName = "";
        private string severname = "";

        public ExportLCORenewRecharge(string _LCOID,string _Entity,string _server)
        {
            GuaLCOID = _LCOID;
            entity = _Entity;
            severname = _server;
        }

        public  string MakeRequestTread(string lcoid,string call)
        {
            
            try
            {
                string str = RestAPICall.MakeRequest("http://" + severname + "/lco/accounts_v1/" + lcoid + "/" + call + "/" + LastID, null, "GET", "application/json", typeof(Program), entity);

                JObject o = JObject.Parse(str);
                StringBuilder sb = new StringBuilder();
                if ((Boolean)o["success"])
                {
                    IList<JToken> items = o["result"]["results"].Children().ToList();
                    if (o["result"]["record_count"] != null)
                    {
                        RecordCount = (int)o["result"]["record_count"];
                        FileName = (string)o["result"]["Name"];
                        try
                        {
                            string error = (string)o["result"]["Error"];
                            if (error!=null)
                            {
                                cmd.WriteLine("http://" + severname + "/lco/accounts_v1/" + lcoid + "/" + call + "/" + LastID + error, Info.Error);

                                cmd.WriteLine("Bulk Insert Failed: "+lcoid +"page id"+LastID +" - " + error, Info.Error);
                            }
                        }catch(Exception ex)
                        {
                            cmd.WriteLine("Error Not Found:"+ex.Message, Info.Error);
                        }
                        //JToken val in items[0]
                        if (RecordCount != 0)
                        {
                            JObject v = items[0] as JObject;
                            string HeaderList = "";
                            foreach (JProperty prop in v.Properties())
                            {
                                HeaderList += prop.Name + ",";
                            }
                            sw.WriteLine(HeaderList.Substring(0, HeaderList.Length - 1));
                            swDayExp7.WriteLine(HeaderList.Substring(0, HeaderList.Length - 1));
                            swexp.WriteLine(HeaderList.Substring(0, HeaderList.Length - 1));
                        }
                        else
                        {
                            return "";
                        }
                        //Console.WriteLine();
                    }
                    foreach (JToken val in items)
                    {
                        JObject v = val as JObject;
                        string HeaderList = "";
                        DateTime expdate = DateTime.Now;
                        if (v["ExpiryDate"].GetType()== typeof(JObject))
                        {
                            string expiry = Convert.ToString(v["ExpiryDate"]["date"]);
                            DateTime.TryParse(expiry, out expdate);
                        }
                        
                        
                        string Status =(string)v["Status"];
                        
                        foreach (JProperty prop in v.Properties())
                        {
                            if (val[prop.Name].GetType() != typeof(JArray) || val[prop.Name].GetType() != typeof(JObject))
                            {
                                try
                                {
                                    if(prop.Name== "ExpiryDate")
                                    {
                                        HeaderList += expdate.ToString("MM/dd/yyyy") + ",";
                                    }
                                    else {
                                        HeaderList += (string)val[prop.Name] + ",";
                                    }
                                    
                                }
                                catch (Exception e)
                                {
                                    HeaderList += ",";
                                }
                            }
                            else
                            {
                                HeaderList += ",";
                            }
                        }
                        if (Status.ToLower() != "active")
                        {
                            swexp.WriteLine(HeaderList.Substring(0, HeaderList.Length - 1));
                        }
                        else
                        {
                            var days = (expdate - DateTime.Now).TotalDays;
                            if (days <= 7)
                            {
                                swDayExp7.WriteLine(HeaderList.Substring(0, HeaderList.Length - 1));
                            }
                            sw.WriteLine(HeaderList.Substring(0, HeaderList.Length - 1));
                        }

                    }
                    LastID += 1;
                }
                else
                {
                    //throw 
                    cmd.WriteLine((string)o["result"],Info.Error);
                    return null;
                    //throw new Exception((string)o["result"]);
                }
                
                return sb.ToString();
            }
            catch (Exception Ex)
            {
                cmd.WriteLine("Error ." + Ex.Message + "-" + lcoid,Info.Error);
                return null;
                //throw Ex;
            }


        }

        public void Start()
        {
            cmd.WriteLine("Start Process");
            
            string path = AppDomain.CurrentDomain.BaseDirectory + "/files_processing/" + entity+"/"+GuaLCOID + "";
            string topath = AppDomain.CurrentDomain.BaseDirectory + "/files/" + entity+"/";
            string backuppath = AppDomain.CurrentDomain.BaseDirectory + "/backup/";
            int pagesisize = 11943;
            int LastID = -1;
            int PAGE = 50;
            int TreadID = 1;
            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/files_processing/" + entity))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/files_processing/" + entity);
            if (!Directory.Exists(backuppath))
                Directory.CreateDirectory(backuppath);

            if (!Directory.Exists(AppDomain.CurrentDomain.BaseDirectory + "/files/" + entity ))
                Directory.CreateDirectory(AppDomain.CurrentDomain.BaseDirectory + "/files/" + entity );
            DateTime StatTime = DateTime.Now;
            cmd.WriteLine("Start: " + DateTime.Now.ToString() + "- LCOID " + GuaLCOID);
            cmd.WriteLine("Create: " + DateTime.Now.ToString() + "- FileProcessing path " + path);
            cmd.WriteLine("Create: " + DateTime.Now.ToString() + "- File Saving path " + topath);
            if (ConfigurationSettings.AppSettings["skip"] == "1")
            {
                if (File.Exists(topath))
                {

                    cmd.WriteLine("Skiped File", Info.Warning);

                    return;
                }
            }
            
            if (ConfigurationSettings.AppSettings["skip"] == "1")
            {
               if( File.Exists(topath + GuaLCOID + ".csv")){
                    
                    cmd.WriteLine("Skiped File", Info.Warning);
                    
                    return;
                }
            }
            sw = new TextWriter(AppDomain.CurrentDomain.BaseDirectory + "/files_processing/" + entity + "/", GuaLCOID + ".csv", entity, true);
            swDayExp7 = new TextWriter(AppDomain.CurrentDomain.BaseDirectory + "/files_processing/" + entity + "/", GuaLCOID + "_7day.csv", entity, true);
            swexp = new TextWriter(AppDomain.CurrentDomain.BaseDirectory + "/files_processing/" + entity + "/", GuaLCOID + "_exp.csv", entity, true);
            string calltype = ConfigurationSettings.AppSettings["call"];
            MakeRequestTread(GuaLCOID, calltype);

            pagesisize = RecordCount-50;
            cmd.WriteLine("Start: " + DateTime.Now.ToString() + "- Requests made for costomer accounts " + pagesisize);
            if (RecordCount != 0)
            {
                while (pagesisize >= 0)
                {
                    List<Action> list = new List<Action>();
                    int i = 0;
                    try
                    {
                        //i/f (!File.Exists(topath + GuaLCOID + ".csv"))
                        //{
                            string str = MakeRequestTread(GuaLCOID, calltype);
                            if (str != null)
                            {
                                pagesisize -= 50;
                                cmd.WriteLine("Start -> Request: " + DateTime.Now.ToString() + " - remaining " + pagesisize);
                            }
                            else
                            {
                                cmd.WriteLine("Start -> Request: " + DateTime.Now.ToString() + " - remaining " + pagesisize, Info.Error);
                            }
                        //}
                        //else
                        //{
                            cmd.WriteLine("Start -> Request: " + DateTime.Now.ToString() + " - remaining " + pagesisize, Info.Warning);

                        //}
                    }
                    catch { }

                }
            }
            //sw.Close();
            sw.Upload(AppDomain.CurrentDomain.BaseDirectory + "/files/" + entity + "/");
            swDayExp7.Upload(AppDomain.CurrentDomain.BaseDirectory + "/files/" + entity + "/");
            swexp.Upload(AppDomain.CurrentDomain.BaseDirectory + "/files/" + entity + "/");

            cmd.WriteLine("End: " + DateTime.Now.ToString(),Info.Info);
            TimeSpan difference = DateTime.Now.Subtract(StatTime);
            cmd.WriteLine("Completed in : " + difference.TotalSeconds + "secs", Info.Info);
            //Console.ReadLine();

        }


    }
}
