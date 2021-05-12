using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Win_10_Optimizer_v2.Classes
{
    public class Cleaner
    {
        public readonly List<ClearSettings> DataBase = new List<ClearSettings>();
        public List<ClearSettings> GetByType(string Type)
        {
            List<ClearSettings> temp = new List<ClearSettings>();
            foreach (ClearSettings cl in DataBase)
            {
                if (cl.Type == Type || cl.Type == "All")
                {
                    temp.Add(cl);
                }
            }
            return temp;
        }
        public void UpdateDataBase()
        {
            using (WebClient wc = new WebClient())
            {
                string responce = wc.DownloadString("https://raw.githubusercontent.com/Nekiplay/Win-10-Optimizer-v2-DataBase/main/CleanerFiles.js");
                string[] splited = responce.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                DataBase.Clear();
                foreach (string sp in splited)
                {
                    if (sp.StartsWith("{") && sp.EndsWith("}") && !sp.StartsWith("//") && !sp.StartsWith("/*"))
                    {
                        string filepath = Regex.Match(sp, "{ \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\" }").Groups[1].Value;
                        string dirpath = Regex.Match(sp, "{ \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\" }").Groups[2].Value;
                        string expansion = Regex.Match(sp, "{ \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\" }").Groups[3].Value;
                        string mode = Regex.Match(sp, "{ \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\" }").Groups[4].Value;
                        string type = Regex.Match(sp, "{ \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\" }").Groups[5].Value;
                        filepath = filepath.Replace("%appdata%", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData).ToString());
                        filepath = filepath.Replace("%user%", Environment.GetFolderPath(Environment.SpecialFolder.UserProfile).ToString());
                        filepath = filepath.Replace("%mydocs%", Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments).ToString());
                        DataBase.Add(new ClearSettings(filepath, dirpath, expansion, mode, type));
                    }
                }
            }
        }

        public class ClearSettings
        {
            public string Type;
            /* File Settings */
            public string FilePath;
            public string Expansion;

            public string DirectoryPath;
            public Modes Mode;
            public enum Modes
            {
                Files,
                Directory,
            }
            public ClearSettings(string FilePath, string DirectoryPath, string Expansion, Modes Mode, string Type)
            {
                this.FilePath = FilePath;
                this.DirectoryPath = DirectoryPath;
                this.Expansion = Expansion;
                this.Mode = Mode;
                this.Type = Type;
            }

            public ClearSettings(string FilePath, string DirectoryPath, string Expansion, string Mode, string Type)
            {
                this.FilePath = FilePath;
                this.DirectoryPath = DirectoryPath;
                this.Expansion = Expansion;
                if (Mode == Modes.Directory.ToString())
                {
                    this.Mode = Modes.Directory;
                }
                else if (Mode == Modes.Files.ToString())
                {
                    this.Mode = Modes.Files;
                }
                this.Type = Type;
            }
            public bool IsExists
            {
                get
                {
                    if (Mode == Modes.Files && !string.IsNullOrEmpty(FilePath))
                    {
                        if (Directory.Exists(FilePath)) { return true; }
                        else { return false; }
                    }
                    else if (Mode == Modes.Directory && !string.IsNullOrEmpty(DirectoryPath))
                    {
                        if (Directory.Exists(DirectoryPath)) { return true; }
                        else { return false; }
                    }
                    else { return false; }
                }
            }
            public long Clear()
            {
                long bytesdeleted = 0;
                if (IsExists && Mode == Modes.Files && !string.IsNullOrEmpty(FilePath))
                {
                    var result = System.IO.Directory.EnumerateFiles(FilePath, Expansion);
                    if (result.Count() != 0)
                    {
                        foreach (var m in result)
                        {
                            try
                            {
                                System.IO.FileInfo file = new System.IO.FileInfo(m);
                                bytesdeleted += file.Length;
                                System.IO.File.Delete(m);
                            }
                            catch { }
                        }
                    }
                }
                else if (IsExists && Mode == Modes.Directory && !string.IsNullOrEmpty(DirectoryPath))
                {
                    System.IO.DirectoryInfo myDirInfo = new System.IO.DirectoryInfo(DirectoryPath);
                    foreach (System.IO.FileInfo file in myDirInfo.GetFiles()) 
                    { 
                        try 
                        {
                            bytesdeleted += file.Length;
                            file.Delete(); 
                        } catch { } 
                    }
                    foreach (System.IO.DirectoryInfo diri in myDirInfo.GetDirectories()) 
                    { 
                        try 
                        {
                            diri.Delete(true); 
                        } catch 
                        { } 
                    }
                    try { System.IO.Directory.Delete(DirectoryPath, true); } catch { }
                }
                return bytesdeleted;
            }
        }
    }
}
