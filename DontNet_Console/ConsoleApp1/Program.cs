using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Configuration;
using Renci.SshNet;
using System.Threading;
//using System.Web

namespace ConsoleApp1
{
    class Program
    {

        private static void RunProcess(List<string> strlist,string entity)
        {
            string serveruri = ConfigurationSettings.AppSettings["Server"];
            int ThreadID = Convert.ToInt32(ConfigurationSettings.AppSettings["Threads"]);
            List<Action> list = new List<Action>();
            foreach (string val in strlist)
            {

                Action a = new Action(() =>
                {
                    ExportLCORenewRecharge l = new ExportLCORenewRecharge(val, entity, serveruri);
                    l.Start();


                });
                list.Add(a);
                ThreadID -= 1;
                if (ThreadID == 0)
                {
                    Parallel.Invoke(list.ToArray());
                    list = new List<Action>();
                    ThreadID = Convert.ToInt32(ConfigurationSettings.AppSettings["Threads"]);
                }


            }
        }
        
        //private static make()
        static void Main(string[] args)
        {
            Console.WriteLine("Start Process");
            //Test Default Values
            string GuaLCOID = "M000100010000100001000007088";
            string UserID = "M000100010000100001000008437";
            string UserName = "GGN01_QualiTV1";
            
            string entity = "82-dendb";
            bool test = true;

            


            //int pages=
            entity =ConfigurationSettings.AppSettings["ENTITY"];
            string serveruri = ConfigurationSettings.AppSettings["Server"];

            switch (args.Length)
            {
                case 1:
                    int Service = Convert.ToInt32(ConfigurationSettings.AppSettings["Service"]);
                    entity = args[0];
                    LCOHandler lobj = new LCOHandler(entity, serveruri);
                    if (Service == 1)
                    {
                        while (true)
                        {
                            List<string> strlist = lobj.LCOPending();
                            RunProcess(strlist,entity);
                            Thread.Sleep(60000);
                        }
                    }
                    else
                    {
                        
                        List<string> strlist = lobj.LCOWithAccounts();
                        RunProcess(strlist, entity);

                    }
                    
                    
                    
                    break;
                case 2:
                    GuaLCOID = args[0];
                    test = false;
                    entity = args[1];
                    duoapi.v1.Ledger l = new duoapi.v1.Ledger(duoapi.v1.LedgerType.LCO, entity, "test", "test");
                    l.GetVauletBalance(GuaLCOID);
                    l.BlockAmount(GuaLCOID, 100);
                    cmd.WriteLine("Complete Path :" + args[1], Info.Info);
                    ExportLCORenewRecharge ex = new ExportLCORenewRecharge(GuaLCOID, entity, serveruri);
                    ex.Start();
                    break;
            }

            
            if (test)
            {
                ExportLCORenewRecharge ex = new ExportLCORenewRecharge(GuaLCOID, entity, serveruri);
                ex.Start();
            }
           


        }
        
    }
}
