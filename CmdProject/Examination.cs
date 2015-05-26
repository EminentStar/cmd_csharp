using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdProject
{
    public class Examination
    {
        private static Examination exam = new Examination();

        private Examination()
        {
        }

        public static Examination GetInstance()
        {
            return exam;
        }

        public Boolean WantToOverlapp(string paramTargetPath)
        {
            Boolean want = true;
            string answer = string.Empty;
            DirectoryInfo targetInfo = new DirectoryInfo(paramTargetPath);

            if (targetInfo.Attributes.ToString().Contains("Archive"))
            {
                Console.Write("{0}을(를) 덮어쓰시겠습니까? (Yes/No/All): ", paramTargetPath);
                answer = Console.ReadLine();
                if (!(answer.Equals("Yes", StringComparison.OrdinalIgnoreCase) || answer.Equals("All", StringComparison.OrdinalIgnoreCase)))
                {
                    want = false;
                }
            }
            return want;
        }

        public Boolean CheckCorrectDirectory(string paramStr)
        {
            Boolean rv = true;
            DirectoryInfo drInfo = null;

            if (paramStr[paramStr.Length - 1] == '\\')
            {
                drInfo = new DirectoryInfo(paramStr);

                if (!drInfo.Exists)
                {
                    rv = false;
                }
            }
            return rv;
        }

        public FileInfo[] ReturnFileName(string paramStr, DirectoryInfo paramDrInfo)
        {
            FileInfo[] fileInfo = null;

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

        public Boolean IsContainedPath(string paramStr)
        {
            return paramStr.Contains("\\");
        }

        public int CheckMovable(string paramStr, DirectoryInfo paramDrInfo)
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

        public Boolean CheckParentDirectory(DirectoryInfo paramDrInfo)
        {
            Boolean rv = false;

            if (paramDrInfo.Parent.Exists)
            {
                rv = true;
            }

            return rv;
        }

        public Boolean IsContainedDot(string paramStr)
        {
            return paramStr.Contains(".");
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

    }
}
