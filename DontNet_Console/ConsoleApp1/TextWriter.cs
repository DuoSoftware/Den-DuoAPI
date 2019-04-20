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
    class TextWriter
    {
        StreamWriter sw;
        string path;
        string filename,entity;
        public TextWriter(string FilePath, string FileName,string Entity, bool overwrite)
        {
            path = FilePath + Entity+"/" + FileName;
            filename = FileName;
            entity = Entity;
            if (!Directory.Exists(FilePath+Entity))
            {
                Directory.CreateDirectory(FilePath + Entity);
            }
            if (!File.Exists(path))
            {
                //if (overwrite) { }
                sw = File.CreateText(path);
            }
            else
            {
                if (overwrite)
                {
                    File.Delete(path);
                    sw = File.CreateText(path);
                }
                else
                {
                    sw = File.AppendText(path);
                }
            }
        } 

        public void WriteLine(string str)
        {
            sw.WriteLine(str);
        }

        public void Close()
        {
            sw.Close();
        }
        

        public void Upload(string topath)
        {
            sw.Close();
            //string topath = path; //+"/upload/"+entity+"/";
            if (!Directory.Exists(topath))
            {
                Directory.CreateDirectory(topath);
            }

            if (!File.Exists(topath+filename))
            {
                File.Move(path, topath + filename);
            }
            else
            {
                //File.Move(topath, backuppath + GuaLCOID + "-" + DateTime.Now.ToString("MMddyyyyhhmmss") + ".csv");
                File.Delete(topath + filename);
                File.Move(path, topath + filename);
                //File.Replace(path,topath,);
            }

            string servername = ConfigurationSettings.AppSettings["appserver"];
            string username = ConfigurationSettings.AppSettings["username"];
            string password = ConfigurationSettings.AppSettings["password"];

            ConnectionInfo connectionInfo = new PasswordConnectionInfo(servername, 22, username, password);
            try
            {
                using (var client = new SftpClient(connectionInfo))
                {
                    //SftpClient
                    client.Connect();

                    using (var file = File.OpenRead(topath + filename))
                    {
                        //client.get
                        try
                        {
                            client.CreateDirectory("/var/media/files/" + entity + "/");
                        }
                        catch { }
                        try
                        {
                            client.DeleteFile("/var/media/files/" + entity + "/" + filename );
                        }
                        catch { }
                        client.UploadFile(file, "/var/media/files/" + entity + "/" + filename );
                        cmd.WriteLine("Upload Complete.");
                    }
                    //client.
                }
            }
            catch (Exception e)
            {

                cmd.WriteLine("Upload failed : " + e.Message, Info.Error);
            }

        }

    }
}
