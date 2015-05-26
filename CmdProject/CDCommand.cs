using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdProject
{
    public class CDCommand
    {
        private static CDCommand cdCommand = new CDCommand();

        private CDCommand()
        {
        }

        public static CDCommand GetInstance()
        {
            return cdCommand;
        }

        public Boolean IsTyped(string paramStr)
        {
            if (paramStr.Length < 2)
                return false;
            else
                return (paramStr.Equals("cd", StringComparison.OrdinalIgnoreCase) || paramStr.Substring(0, 2).Equals("cd", StringComparison.OrdinalIgnoreCase));
        }

        public void Flow(string[] paramStrArr, ref DirectoryInfo paramDrInfo)
        {
            paramStrArr = RemoveSpace(paramStrArr);

            switch (paramStrArr.Length)
            {
                case 1:
                    {
                        Command1(paramStrArr[0], ref paramDrInfo);
                        break;
                    }
                case 2:
                    {
                        Command2(paramStrArr[1], ref paramDrInfo);
                        break;
                    }
                default:
                    Console.WriteLine("지정된 경로를 찾을 수 없습니다.\n");
                    break;
            }
            Console.WriteLine();
        }

        public void Command1(string paramStr, ref DirectoryInfo drInfo)
        {
            //if the length of paramStr is 2, you've just gotta print the current entire directories
            if (paramStr.Length == 2)
            {
                Console.WriteLine(drInfo);
            }
            else if (paramStr.Equals("cd."))
            {
            }
            else if (paramStr.Equals("cd.."))
            {
                drInfo = (drInfo.Parent != null) ? new DirectoryInfo(drInfo.Parent.FullName) : drInfo;
            }
            else if (paramStr.Equals("cd/") || paramStr.Equals("cd\\"))
            {
                drInfo = (drInfo.Parent != null) ? drInfo.Root : drInfo;
            }
            else
            {
                Console.WriteLine("지정된 경로를 찾을 수 없습니다.");
            }
            //if the length of paramStr is over 2, you need to consider whether 
        }

        public void Command2(string paramStr, ref DirectoryInfo drInfo)
        {
            DirectoryInfo rDrInfo = null;

            if (paramStr.Equals("."))
            {

            }
            else if (paramStr.Equals(".."))
            {
                drInfo = (drInfo.Parent != null) ? new DirectoryInfo(drInfo.Parent.FullName) : drInfo;
            }
            else
            {
                string path = Path.Combine(@drInfo.FullName, @paramStr);
                rDrInfo = new DirectoryInfo(path);

                if (rDrInfo.Exists)
                {
                    drInfo = rDrInfo;
                }
                else
                {
                    Console.WriteLine("지정된 경로를 찾을 수 없습니다.");
                }
            }
        }

        public string[] RemoveSpace(string[] paramStrArr)
        {
            string[] newStr = null;
            List<string> tempList = new List<string>();

            foreach (string str in paramStrArr)
            {
                if (!str.Equals(""))
                    tempList.Add(str);
            }
            newStr = tempList.ToArray<string>();

            return newStr;
        }
    }
}
