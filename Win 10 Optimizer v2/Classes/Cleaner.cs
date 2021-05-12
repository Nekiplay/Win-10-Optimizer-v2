using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
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
