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
        private readonly List<ClearSettings> Logs = new List<ClearSettings>();

        public void UpdateDataBase()
        {
            using (WebClient wc = new WebClient())
            {
                string responce = wc.DownloadString("https://raw.githubusercontent.com/Nekiplay/Win-10-Optimizer-v2-DataBase/main/CleanerFiles.lua");
                string[] splited = responce.Split(new string[] { "\n" }, StringSplitOptions.RemoveEmptyEntries);
                foreach (string sp in splited)
                {
                    string filepath = Regex.Match(sp, "{\"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\"}").Groups[1].Value;
                    string dirpath = Regex.Match(sp, "{\"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\"}").Groups[2].Value;
                    string expansion = Regex.Match(sp, "{\"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\"}").Groups[3].Value;
                    string mode = Regex.Match(sp, "{\"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\"}").Groups[4].Value;
                    string type = Regex.Match(sp, "{\"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\", \"(.*)\"}").Groups[5].Value;
                    Console.WriteLine(filepath);
                    Console.WriteLine(dirpath);
                    Console.WriteLine(expansion);
                    Console.WriteLine(mode);
                    Console.WriteLine(type);
                    if (mode == "Files")
                    {

                    } 
                    else if (mode == "Directory")
                    {

                    }
                }
            }
        }

        public class ClearSettings
        {
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
            public bool IsExists
            {
                get
                {
                    if (Mode == Modes.Files)
                    {
                        if (File.Exists(FilePath)) { return true; }
                        else { return false; }
                    }
                    else if (Mode == Modes.Directory)
                    {
                        if (Directory.Exists(DirectoryPath)) { return true; }
                        else { return false; }
                    }
                    else { return false; }
                }
            }
            public void Clear()
            {
                if (IsExists && Mode == Modes.Files)
                {
                    var result = System.IO.Directory.EnumerateFiles(FilePath, Expansion);
                    if (result.Count() != 0)
                    {
                        foreach (var m in result)
                        {
                            try
                            {
                                System.IO.FileInfo file = new System.IO.FileInfo(m);
                                System.IO.File.Delete(m);
                            }
                            catch { }
                        }
                    }
                }
                else if (IsExists && Mode == Modes.Directory)
                {
                    System.IO.DirectoryInfo myDirInfo = new System.IO.DirectoryInfo(DirectoryPath);
                    foreach (System.IO.FileInfo file in myDirInfo.GetFiles()) 
                    { 
                        try 
                        { 
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
            }
        }
    }
}
