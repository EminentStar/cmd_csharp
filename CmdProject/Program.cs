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
            

            cmd.CheckCommand(drInfo);

        }
    }
}
