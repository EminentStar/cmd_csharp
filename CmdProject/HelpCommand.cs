using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdProject
{
    public class HelpCommand
    {
        private static HelpCommand helpCommand = new HelpCommand();

        private HelpCommand()
        {
        }

        public static HelpCommand GetInstance()
        {
            return helpCommand;
        }

        public void Flow()
        {
            string line;
            using (StreamReader reader = new StreamReader("../../help_command_text.txt", Encoding.Default, true))
            {
                line = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    Console.WriteLine(line);
                }
            }
        }

        public Boolean IsTyped(string paramStr)
        {
            if (paramStr.Length < 4)
                return false;
            else
                return paramStr.Equals("help", StringComparison.OrdinalIgnoreCase);
        }
    }
}
