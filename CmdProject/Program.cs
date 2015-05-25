using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CmdProject
{
    class Program
    {
        static void Main(string[] args)
        {
            Command cmd = new Command();

            
            DirectoryInfo drInfo = new DirectoryInfo(@Environment.GetFolderPath(Environment.SpecialFolder.UserProfile));

            //DirectoryInfo testInfo = new DirectoryInfo(@"c:\users\준규\Desktop\test");
            //if(testInfo.Exists)
            //{
            //    Console.WriteLine("true");
            //}
            //else
            //{
            //    Console.WriteLine("false");
            //}


            cmd.CheckCommand(drInfo);

        }
    }
}
