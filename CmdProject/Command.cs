using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Management;

namespace CmdProject
{
    public class Command
    {

        public void CheckCommand(DirectoryInfo drInfo)
        {
            DriveInfo[] drives = DriveInfo.GetDrives();
            DriveInfo drive = DriveInfo.GetDrives()[0];

            DirectoryInfo[] dirList = null;
            FileInfo[] fileList = null;
            FileSystemInfo[] fsList = null;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia"); //put System.Management assembly
            string[] strArr = null;
            string cmdLine = null;
            string tempStr = null;
            string SerialNo = null;
            long fileLen = 0, fileLenSum = 0, freespace = 0;
            int fileCnt = 0, dirCnt = 0;

            // char[] delimiters = { ' ', '(', ')', ':', ';', ',' };

            while (true)
            {
                Console.Write("{0}>", drInfo);
                cmdLine = Console.ReadLine();
                cmdLine = cmdLine.Trim();
                strArr = cmdLine.Split(new char[] { ' ', '(', ')', ';', ',' });
                
                if (strArr.Length == 1 && strArr[0].Equals(""))// just press enter
                {
                }
                else if (IsTypedCls(strArr[0])) //cls command
                {
                    Console.Clear();
                }
                else if (IsTypedCd(strArr[0]))//cd command
                {
                    CDFlow(strArr, drives, ref drInfo);
                }
                else if (IsTypedDir(strArr[0])) //dir command
                {
                    DirFlow(strArr, drives, drInfo);
                }
                //else if (IsTypedHelp(strArr[0])) //help command
                else if (strArr[0].Equals("help", StringComparison.OrdinalIgnoreCase))
                {
                    HelpFlow();
                }
                else
                {
                    Console.WriteLine("\'{0}\'은<는> 내부 또는 외부 명령, 실행할 수 있는 프로그램, 또는 배치 파일이 아닙니다.", strArr[0]);
                }
            }
        }

        public Boolean IsTypedDir(string paramStr)
        {
            return (paramStr.Equals("dir", StringComparison.OrdinalIgnoreCase) || paramStr.Substring(0, 3).Equals("dir", StringComparison.OrdinalIgnoreCase));
        }

        public Boolean IsTypedCd(string paramStr)
        {
            return (paramStr.Equals("cd", StringComparison.OrdinalIgnoreCase) || paramStr.Substring(0, 2).Equals("cd", StringComparison.OrdinalIgnoreCase));
        }

        public Boolean IsTypedCls(string paramStr)
        {
            return (Regex.IsMatch(paramStr, @"\b[c|C][l|L][s|s]\.") || paramStr.Equals("cls", StringComparison.OrdinalIgnoreCase));
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

        public void HelpFlow()
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

        public void CDFlow(string[] paramStrArr, DriveInfo[] paramDrives, ref DirectoryInfo paramDrInfo)
        {
            paramStrArr = RemoveSpace(paramStrArr);

            switch (paramStrArr.Length)
            {
                case 1:
                    {
                        CDCommand(paramStrArr[0], paramDrives, ref paramDrInfo);
                        break;
                    }
                case 2:
                    {
                        CDCommand(paramStrArr[1], ref paramDrInfo);
                        break;
                    }
                default:
                    Console.WriteLine("지정된 경로를 찾을 수 없습니다.\n");
                    break;
            }
        }

        public void CDCommand(string paramStr, DriveInfo[] paramDrives, ref DirectoryInfo drInfo)
        {
            DirectoryInfo rDrInfo = null;

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
            else
            {
                Console.WriteLine("지정된 경로를 찾을 수 없습니다.");
            }
            //if the length of paramStr is over 2, you need to consider whether 


            Console.WriteLine();
        }

        public void CDCommand(string paramStr, ref DirectoryInfo drInfo)
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
                rDrInfo = new DirectoryInfo(paramStr);
                drInfo = (rDrInfo.Exists) ? rDrInfo : drInfo;
            }
        }

        public int CountDots(string paramStr)
        {
            int rv = 0;


            return 1;
        }

        public DirectoryInfo SearchRoot(DirectoryInfo paramDrInfo)
        {
            DriveInfo[] dArr = DriveInfo.GetDrives();
            DirectoryInfo rd = null;

            foreach (DriveInfo element in dArr)
            {
                if (element.ToString().Equals(paramDrInfo.Root.ToString()))
                {
                    rd = element.RootDirectory;
                }
            }
            return rd;
        }

        public bool SearchDirectory(DirectoryInfo paramDrInfo)
        {
            bool isExists = false;
            string dirStr = paramDrInfo.ToString();
            DirectoryInfo rootDir = SearchRoot(paramDrInfo);

            isExists = SearchSubDirectory(paramDrInfo);

            return isExists;
        }

        public bool SearchSubDirectory(DirectoryInfo paramDrInfo)
        {
            bool isExists = false;



            return isExists;
        }

        public void DirFlow(string[] paramStrArr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            switch (paramStrArr.Length)
            {
                case 1:
                    {
                        DirCommand(paramDrives, paramDrInfo);
                        break;
                    }
                case 2:
                    DirCommand(paramDrives, new DirectoryInfo(paramStrArr[1]));
                    break;
                default:
                    ShowDriveInfo(paramDrives, paramDrInfo);
                    Console.WriteLine("파일을 찾을 수 없습니다.\n");
                    break;
            }
        }

        public DriveInfo ShowDriveInfo(DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            int i = 0;
            string serialNumber = string.Empty;
            for (i = 0; i < paramDrives.Length; i++)
            {
                if (paramDrives[i].RootDirectory.ToString().Equals(paramDrInfo.Root.ToString(), StringComparison.OrdinalIgnoreCase))
                {
                    serialNumber = GetVolumeSerialNumber(paramDrives[i].ToString().Substring(0, 1));
                    Console.WriteLine("{0}", (paramDrives[i].VolumeLabel.Equals("") ? (" " + paramDrives[i] + " 드라이브의 볼륨에는 이름이 없습니다.") : (paramDrives[i].VolumeLabel.ToString())));
                    Console.WriteLine(" 볼륨 일련 번호: " + serialNumber.Insert(4, "-"));
                    Console.WriteLine("\n {0} 디렉터리\n", paramDrInfo);
                    break;
                }
            }
            return paramDrives[i];
        }

        public FileSystemInfo GetDirectoryFileSystemInfos(DirectoryInfo drInfo)
        {
            DirectoryInfo parentDir = drInfo.Parent;
            FileSystemInfo[] fsInfos = null;
            FileSystemInfo dirInfo = null;

            if (parentDir != null)
            {
                fsInfos = parentDir.GetFileSystemInfos();
                foreach (FileSystemInfo fsInfo in fsInfos)
                {
                    if (fsInfo.ToString().Equals(drInfo.ToString()))
                    {
                        dirInfo = fsInfo;
                        break;
                    }
                }
            }
            else
            {
                DriveInfo driveInfo = new DriveInfo(drInfo.ToString());
                //fsInfos = drInfo.GetFileSystemInfos();
                //foreach (FileSystemInfo fsInfo in fsInfos)
                //{
                //    if (fsInfo.ToString().Equals(drInfo.ToString()))
                //    {
                //        dirInfo = fsInfo;
                //        break;
                //    }
                //}
            }

            return dirInfo;
        }

        public void DirCommand(DriveInfo[] drives, DirectoryInfo drInfo)
        {

            DriveInfo drive = null;

            FileInfo[] fileList = null;
            FileSystemInfo[] fsList = null;
            long fileLen = 0, fileLenSum = 0;
            int fileCnt = 0, dirCnt = 0;
            string tempStr = null;
            FileSystemInfo currentDir = null;
            FileSystemInfo parentDir = null;
            drive = ShowDriveInfo(drives, drInfo);

            fsList = drInfo.GetFileSystemInfos();
            fileList = drInfo.GetFiles();

            //currentDir = GetDirectoryFileSystemInfos(drInfo);
            //parentDir = GetDirectoryFileSystemInfos(drInfo.Parent);

            //Console.Write(fInfo.LastWriteTime.ToString("yyyy-MM-dd  tt hh:mm"));
            //Console.Write("    <DIR>          ");
            //Console.WriteLine(" " + fInfo);

            foreach (FileSystemInfo fInfo in fsList)
            {

                if (!fInfo.Attributes.ToString().Contains("Hidden"))
                {
                    if (fInfo.Attributes.ToString().Contains("Directory"))//when directories or files are not hidden
                    {

                        Console.Write(fInfo.LastWriteTime.ToString("yyyy-MM-dd  tt hh:mm"));
                        Console.Write("    <DIR>         ");

                        dirCnt++;
                    }
                    else if (fInfo.Attributes.ToString().Contains("Archive"))
                    {
                        for (int i = 0; i < fileList.Length; i++)
                        {
                            if (fileList[i].Name.Equals(fInfo.ToString()))
                            {
                                fileLen = fileList[i].Length;
                                fileLenSum += fileLen;
                                break;
                            }
                        }
                        Console.Write(fInfo.LastWriteTime.ToString("yyyy-MM-dd  tt hh:mm"));
                        tempStr = string.Format("{0:N0}", fileLen);
                        Console.Write(tempStr.PadLeft(18, ' '));
                        fileCnt++;
                    }
                    Console.WriteLine(" " + fInfo);
                }
            }

            tempStr = string.Format("{0:N0}", fileLenSum);
            Console.WriteLine(fileCnt.ToString().PadLeft(16, ' ') + "개 파일 " + tempStr.PadLeft(19, ' ') + " 바이트");
            tempStr = string.Format("{0:N0}", drive.AvailableFreeSpace);
            Console.WriteLine(dirCnt.ToString().PadLeft(16, ' ') + "개 디렉터리" + tempStr.PadLeft(19, ' ') + " 바이트 남음");
            Console.WriteLine();
        }

        private String GetVolumeSerialNumber(String Drive)
        {
            String strVolumeSerialNumber = String.Empty;
            ObjectQuery objQuery = new ObjectQuery("SELECT VolumeSerialNumber FROM Win32_LogicalDisk WHERE Name='" + Drive + ":'");
            ManagementObjectSearcher mobjSearcher = new ManagementObjectSearcher(objQuery);
            try
            {
                foreach (ManagementObject obj in mobjSearcher.Get())
                {
                    strVolumeSerialNumber = obj["VolumeSerialNumber"].ToString();
                    break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                strVolumeSerialNumber = String.Empty;
            }
            return strVolumeSerialNumber;
        }

    }
}
