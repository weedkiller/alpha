using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace Multigroup.Portal.Utilities
{
    public class LogFile
    {
        private string fileName;
        public LogFile()
        {
            fileName = "/logDefault.txt";
        }

        public LogFile(string fileName)
        {
            this.fileName = fileName;
        }

        public void MyLogFile(string strCategory, string strMessage)
        {
            // Store the script names and test results in a output text file.
            using (StreamWriter writer = new StreamWriter(new FileStream(fileName, FileMode.Append)))
            {
                ;
                writer.WriteLine("{0}|{1} | {2}", DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToLongTimeString(), strCategory, strMessage);
            }
        }
    }
}
