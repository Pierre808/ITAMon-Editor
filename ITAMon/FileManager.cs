using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ITAMon
{
    internal class FileManager
    {
        public static bool CreateITAFile(string path, string filename, string content)
        {
            try
            {
                using (FileStream fs = File.Create(path + "/" + filename + ".ITA"))
                {
                    byte[] info = new UTF8Encoding(true).GetBytes(content);
                    fs.Write(info, 0, info.Length);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static bool CheckPath(string path)
        {
            if(Directory.Exists(path))
            {
                return true;
            }

            return false;
        }

        public static bool CheckIfImage(string path)
        {
            string extension = Path.GetExtension(path);

            if (extension == ".jpg" || extension == ".jpeg" || extension == ".png" || extension == ".gif")
            {
                if(File.Exists(path))
                {
                    return true;
                }
            }

            return false;
        }
    }
}
