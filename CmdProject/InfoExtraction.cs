using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace CmdProject
{
    public class InfoExtraction
    {
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
            }

            return dirInfo;
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
    }
}
