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

                if (Regex.IsMatch(strArr[0], @"\b[c|C][l|L][s|s]\.") || strArr[0].Equals("cls", StringComparison.OrdinalIgnoreCase)) //cls command
                {
                    Console.Clear();
                }
                else if (Regex.IsMatch(strArr[0], @"\bcd\b") || Regex.IsMatch(strArr[0], @"\bcd"))//cd command
                {
                    CDFlow(strArr, drives, drInfo);
                }
                else if (Regex.IsMatch(strArr[0], @"\bdir\b")) //dir command
                {
                    DirFlow(strArr, drives, drInfo);
                }
                else if (strArr.Length == 1 && strArr[0].Equals(""))// just press enter
                {
                }
                else
                {
                    Console.WriteLine("\'{0}\'은<는> 내부 또는 외부 명령, 실행할 수 있는 프로그램, 또는 배치 파일이 아닙니다.", strArr[0]);
                }
            }
        }

        public string[] RemoveSpace(string[] paramStrArr)
        {
            string[] newStr = null;
            List<string> tempList = new List<string>();

            foreach(string str in paramStrArr)
            {
                if (!str.Equals(""))
                    tempList.Add(str);
            }
            newStr = tempList.ToArray<string>();

            return newStr;
        }

        public void CDFlow(string[] paramStrArr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            paramStrArr = RemoveSpace(paramStrArr);

            switch (paramStrArr.Length)
            {
                case 1:
                    {
                        //CDCommand(paramDrives, paramDrInfo);
                        break;
                    }
                case 2:
                    {
                        CDCommand(paramStrArr[1], paramDrInfo);
                        break;
                    }
                default:
                    Console.WriteLine("지정된 경로를 찾을 수 없습니다.\n");
                    break;
            }
        }

        public void DirFlow(string[] paramStrArr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo )
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

        public void CDCommand(string paramStr, DirectoryInfo drInfo)
        {
            
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
        //String strVolumeSerial = GetVolumeSerialNumber("C"); //C드라이브의 시리얼 번호 가져오기
    }
}
