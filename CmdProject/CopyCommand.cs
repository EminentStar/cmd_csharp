using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CmdProject
{
    public class CopyCommand
    {
        Examination exam = Examination.GetInstance();
        private static CopyCommand copyCommand = new CopyCommand();
           
        private CopyCommand()
        {
        }

        public static CopyCommand GetInstance()
        {
            return copyCommand;
        }

        public Boolean IsTyped(string paramStr)
        {
            if (paramStr.Length < 4)
                return false;
            else
                return paramStr.Equals("copy", StringComparison.OrdinalIgnoreCase);
        }

        public void Flow(string[] paramStrArr, DirectoryInfo paramDrInfo)
        {
            switch (paramStrArr.Length)
            {
                case 1:
                    Console.WriteLine("명령 구문이 올바르지 않습니다.");
                    break;
                case 2:
                    CheckFile(paramStrArr[1], paramDrInfo);
                    break;
                case 3:
                    Command(paramStrArr, paramDrInfo);
                    break;
                default:
                    break;
            }
            Console.WriteLine();
        }

        public void CheckFile(string paramStr, DirectoryInfo paramDrInfo)
        {
            FileInfo[] fileInfo = null;
            int fileCnt = exam.CheckMovable(paramStr, paramDrInfo);
            //DirectoryInfo drInfo = null;

            if (fileCnt == 0)
            {
                Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
            }
            else
            {
                Console.WriteLine("같은 파일로 복사할 수 없습니다.");
                Console.WriteLine("\t0개 파일이 복사되었습니다.");
            }
        }

        public void Command(string[] paramStrArr, DirectoryInfo paramDrInfo)
        {
            FileInfo[] fileInfo = null;
            DirectoryInfo checkDrInfo = null;
            int fileCnt = exam.CheckMovable(paramStrArr[1], paramDrInfo);
            int cnt = 0;
            //if no original file,  cancel
            if (fileCnt == 0)
            {
                Console.WriteLine("지정된 파일을 찾을 수 없습니다.");
            }
            else//original files exists
            {
                fileInfo = exam.ReturnFileName(paramStrArr[1], paramDrInfo);

                if (exam.IsContainedPath(paramStrArr[2]))//target path contains path
                {
                    checkDrInfo = new DirectoryInfo(paramStrArr[2]);
                    if (!exam.CheckCorrectDirectory(paramStrArr[2])) //Non-existing directory
                    {
                        Console.WriteLine("파일 이름, 디렉터리 이름 또는 볼륨 레이블 구문이 잘못되었습니다.");
                    }
                    //else if (!checkDrInfo.Exists)
                    else if (checkDrInfo.Attributes.ToString().Contains("-1"))
                    {
                        Console.WriteLine("지정된 경로를 찾을 수 없습니다.");
                    }
                    else
                    {
                        foreach (FileInfo element in fileInfo)
                        {
                            if (!exam.WantToOverlapp(paramStrArr[2]))
                            {
                                break;
                            }
                            element.CopyTo(@paramStrArr[2], true);
                            cnt++;
                        }
                    }
                }
                else//put the path of the current directory
                {
                    string path = Path.Combine(paramDrInfo.FullName, paramStrArr[2]);
                    checkDrInfo = new DirectoryInfo(path);

                    //checkDrInfo.Attributes.HasFlag();

                    foreach (FileInfo element in fileInfo)
                    {
                        if (!exam.WantToOverlapp(path))
                            break;

                        if (!(checkDrInfo.Attributes.ToString().Contains("Directory")))
                        {
                            element.CopyTo(path, true);
                        }
                        else
                        {
                            path += "\\" + element.Name;
                            if (!exam.WantToOverlapp(path))
                                break;

                            element.CopyTo(path, true);
                        }

                        cnt++;
                    }
                }
                Console.WriteLine("\t{0}개 파일이 복사되었습니다.", cnt);
            }
        }
    }
}
