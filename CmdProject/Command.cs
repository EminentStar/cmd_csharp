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

            //DirectoryInfo[] dirList = null;
            //FileInfo[] fileList = null;
            //FileSystemInfo[] fsList = null;
            ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia"); //put System.Management assembly
            string[] strArr = null;
            string cmdLine = null;
            //string tempStr = null;
            //string SerialNo = null;
            //long fileLen = 0, fileLenSum = 0, freespace = 0;
            //int fileCnt = 0, dirCnt = 0;

            // char[] delimiters = { ' ', '(', ')', ':', ';', ',' };

            while (true)
            {
                Console.Write("{0}>", drInfo.FullName);
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
                else if (strArr[0].Equals("move", StringComparison.OrdinalIgnoreCase))
                {
                    MoveFlow(strArr, drives, drInfo);
                }
                else if (strArr[0].Equals("help", StringComparison.OrdinalIgnoreCase))//help command
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
            if (paramStr.Length < 3)
                return false;
            else
                return (paramStr.Equals("dir", StringComparison.OrdinalIgnoreCase) || paramStr.Substring(0, 3).Equals("dir", StringComparison.OrdinalIgnoreCase));
        }

        public Boolean IsTypedCd(string paramStr)
        {
            if (paramStr.Length < 2)
                return false;
            else
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
            Console.WriteLine();
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
            else if (paramStr.Equals("cd/"))
            {
                drInfo = (drInfo.Parent != null) ? drInfo.Root : drInfo;
            }
            else
            {
                Console.WriteLine("지정된 경로를 찾을 수 없습니다.");
            }
            //if the length of paramStr is over 2, you need to consider whether 
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
            Console.WriteLine();
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

            FileSystemInfo[] fsList = null;
            long fileLen = 0, fileLenSum = 0;
            int fileCnt = 0, dirCnt = 0;
            string tempStr = null;
            drive = ShowDriveInfo(drives, drInfo);

            fsList = drInfo.GetFileSystemInfos();

            //currentDir = GetDirectoryFileSystemInfos(drInfo);
            //parentDir = GetDirectoryFileSystemInfos(drInfo.Parent);

            //Console.Write(fInfo.LastWriteTime.ToString("yyyy-MM-dd  tt hh:mm"));
            //Console.Write("    <DIR>          ");
            //Console.WriteLine(" " + fInfo);

            foreach (FileSystemInfo fInfo in fsList)
            {
                if (!fInfo.Attributes.HasFlag(FileAttributes.Hidden))//when directories or files are not hidden
                {
                    if (fInfo.Attributes.HasFlag(FileAttributes.Directory))
                    {
                        Console.Write(fInfo.LastWriteTime.ToString("yyyy-MM-dd  tt hh:mm"));
                        Console.Write("    <DIR>         ");

                        dirCnt++;
                    }
                    else // if(fInfo.Attributes.HasFlag(FileAttributes.Archive))
                    {
                        fileLen = ((FileInfo)fInfo).Length;
                        fileLenSum += fileLen;

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
        }

        public String GetVolumeSerialNumber(String Drive)
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

        public void MoveFlow(string[] paramStrArr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            //first of all, we need to check whether the file name (paramStrArr[1]) which will be moved exists or not in current directory or typed directory
            //so we need to check typed string has full path 

            switch (paramStrArr.Length)
            {
                case 1:
                    {
                        Console.WriteLine("명령 구문이 올바르지 않습니다.");
                        break;
                    }
                case 2:
                    {
                        MoveCommand(paramStrArr[1], paramDrives, paramDrInfo);
                        break;
                    }
                case 3:
                    {
                        MoveCommand(paramStrArr, paramDrives, paramDrInfo);
                        break;
                    }
                default:
                    break;
            }
            Console.WriteLine();
        }

        public void MoveCommand(string paramStr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            int fileCnt = CheckMovable(paramStr, paramDrives, paramDrInfo);

            if (fileCnt == 0)
            {
                Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
            }
            else
            {
                Console.WriteLine("\t{0}개 파일을 이동했습니다.", fileCnt);
            }
        }

        public void MoveCommand(string[] paramStrArr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            int fileCnt = CheckMovable(paramStrArr[1], paramDrives, paramDrInfo);
            //case 1: wrong original file  ("지정된 파일을 찾을 수 없습니다. ")
            if (fileCnt == 0)
            {
                Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
            }
            else
            {
                SubMoveProcess(paramStrArr, paramDrives, paramDrInfo);
                //case 2: correct original file -> wrong path

                //case 3: correct original file -> correct path

                //case 4: correct original file -> correct path with file
                //case 5: correct original file -> wrong path with file

            }


            //case 6: correct original file -> correct path
            //case 7: correct original file -> correct path
            //case 8: correct original file -> correct path
        }

        //https://msdn.microsoft.com/en-us/library/vstudio/cc148994(v=vs.100).aspx

        public void SubMoveProcess(string[] paramStrArr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            FileInfo[] sourceFile = ReturnFileName(paramStrArr[1], paramDrives, paramDrInfo);
            string targetPath = string.Empty;

            //case 2: correct original file -> wrong path

            //case 3: correct original file -> wrong path with file

            //case 4: correct original file -> correct path

            //case 5: correct original file -> correct path with file

            //when there is no path which refer to current directory
            if (!IsContainedPath(paramStrArr[2])) 
            {
                foreach(FileInfo f in sourceFile)
                {
                    //File.Copy();
                }
            }
            //when there is the path which means that you need check whether the directory or file exists



        }
        public FileInfo[] ReturnFileName(string paramStr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            FileInfo[] fileInfo = null;
            int fileCnt = 0;
            //Check paramStr is including path
            if (IsContainedPath(paramStr))
            {
                int lastBackSlash = paramStr.LastIndexOf("\\");

                DirectoryInfo drInfo = new DirectoryInfo(paramStr.Substring(0, lastBackSlash));
                string searchedFile = paramStr.Substring(lastBackSlash + 1, paramStr.Length - lastBackSlash - 1);

                if (drInfo.Exists)
                    fileInfo = drInfo.GetFiles(searchedFile);
            }
            else
            {
                fileInfo = paramDrInfo.GetFiles(paramStr);
            }

            return fileInfo;
        }


        public int CheckMovable(string paramStr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            FileInfo[] fileInfo = null;
            int fileCnt = 0;
            //Check paramStr is including path
            if (IsContainedPath(paramStr))
            {
                int lastBackSlash = paramStr.LastIndexOf("\\");

                DirectoryInfo drInfo = new DirectoryInfo(paramStr.Substring(0, lastBackSlash));
                string searchedFile = paramStr.Substring(lastBackSlash + 1, paramStr.Length - lastBackSlash - 1);

                if (drInfo.Exists)
                    fileInfo = drInfo.GetFiles(searchedFile);
            }
            else
            {
                fileInfo = paramDrInfo.GetFiles(paramStr);
            }
            if (!(fileInfo == null || fileInfo.Count() == 0))
            {
                fileCnt = fileInfo.Count();
            }

            return fileCnt;
        }

        public Boolean IsContainedPath(string paramStr)
        {
            return paramStr.Contains("\\");
        }

        public FileInfo[] GetSearchedFileInfo(string paramStr)
        {
            int lastBackSlash = paramStr.LastIndexOf("\\");
            DirectoryInfo drInfo = new DirectoryInfo(paramStr.Substring(0, lastBackSlash));
            string searchedFile = paramStr.Substring(lastBackSlash + 1, paramStr.Length - lastBackSlash - 1);
            FileInfo[] fileInfo = drInfo.GetFiles(searchedFile);

            if (fileInfo != null)
            {
                return fileInfo;
            }
            else
            {
                return fileInfo = new FileInfo[1];
            }
        }

        public Boolean IsExistsFile(string paramFile, DirectoryInfo paramDrInfo)
        {
            Boolean isExists = false;
            FileSystemInfo[] fsInfoList = paramDrInfo.GetFileSystemInfos();

            foreach (FileSystemInfo element in fsInfoList)
            {
                if (element.Attributes.HasFlag(FileAttributes.Archive) && element.Name.ToString().Equals(paramFile))
                {
                    isExists = true;
                    break;
                }
            }
            return isExists;
        }

        //public DirectoryInfo SearchRoot(DirectoryInfo paramDrInfo)
        //{
        //    DriveInfo[] dArr = DriveInfo.GetDrives();
        //    DirectoryInfo rd = null;

        //    foreach (DriveInfo element in dArr)
        //    {
        //        if (element.ToString().Equals(paramDrInfo.Root.ToString()))
        //        {
        //            rd = element.RootDirectory;
        //        }
        //    }
        //    return rd;
        //}

        //public Boolean SearchDirectory(DirectoryInfo paramDrInfo)
        //{
        //    Boolean isExists = false;
        //    string dirStr = paramDrInfo.ToString();
        //    DirectoryInfo rootDir = SearchRoot(paramDrInfo);

        //    isExists = SearchSubDirectory(paramDrInfo);

        //    return isExists;
        //}

        //public Boolean SearchSubDirectory(DirectoryInfo paramDrInfo)
        //{
        //    Boolean isExists = false;
        //    DirectoryInfo[] subDrInfo = paramDrInfo.GetDirectories();

        //    foreach(DirectoryInfo element in subDrInfo)
        //    {

        //    }

        //    return isExists;
        //}

    }
}
