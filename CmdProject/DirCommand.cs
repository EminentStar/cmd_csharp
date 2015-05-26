using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace CmdProject
{
    public class DirCommand
    {
        InfoExtraction ie = new InfoExtraction();

        private static DirCommand dirCommand = new DirCommand();

        private DirCommand()
        {
        }

        public static DirCommand GetInstance()
        {
            return dirCommand;
        }

        public Boolean IsTyped(string paramStr)
        {
            if (paramStr.Length < 3)
                return false;
            else
                return (paramStr.Equals("dir", StringComparison.OrdinalIgnoreCase) || paramStr.Substring(0, 3).Equals("dir", StringComparison.OrdinalIgnoreCase));
        }

        public void Flow(string[] paramStrArr, DriveInfo[] paramDrives, DirectoryInfo paramDrInfo)
        {
            switch (paramStrArr.Length)
            {
                case 1:
                    {
                        Command(paramDrives, paramDrInfo);
                        break;
                    }
                case 2:
                    Command(paramDrives, new DirectoryInfo(paramStrArr[1]));
                    break;
                default:
                    ie.ShowDriveInfo(paramDrives, paramDrInfo);
                    Console.WriteLine("파일을 찾을 수 없습니다.\n");
                    break;
            }
            Console.WriteLine();
        }

        public void Command(DriveInfo[] drives, DirectoryInfo drInfo)
        {

            DriveInfo drive = null;

            FileSystemInfo[] fsList = null;
            long fileLen = 0, fileLenSum = 0;
            int fileCnt = 0, dirCnt = 0;
            string tempStr = null;
            drive = ie.ShowDriveInfo(drives, drInfo);

            fsList = drInfo.GetFileSystemInfos();

            //the current directory
            Console.Write(drInfo.LastWriteTime.ToString("yyyy-MM-dd  tt hh:mm"));
            Console.Write("    <DIR>          ");
            Console.WriteLine("" + ".");
            dirCnt++;

            //the parent directory of the current directory
            Console.Write(drInfo.LastWriteTime.ToString("yyyy-MM-dd  tt hh:mm"));
            Console.Write("    <DIR>          ");
            Console.WriteLine("" + "..");
            dirCnt++;

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


    }
}
