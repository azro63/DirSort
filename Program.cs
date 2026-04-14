using System;
using System.IO;

namespace MediaDirSort
{
    class Program
    {
        static string GetFileName(string file)
        {
            string fileName = "";
            int i = file.Length - 1;

            while (file[i] != '\\' && i > 0) {fileName = file[i] + fileName; i--;}

            return fileName;
        }

        static string InsertBeforeExt(string s1, string s2)
        {
            string res = "";
            int extIdx = -1;

            int i = s1.Length - 1;
            while (s1[i] != '.') i--;
            extIdx = i - 1;
            string leftPart = "";
            string rightPart = "";
            if (s1[extIdx] != ')')
            {
                for(i = 0; i <= extIdx; i++) leftPart += s1[i];
                for(i = extIdx + 1; i < s1.Length; i++) rightPart += s1[i];
                res = leftPart + $"({s2})" + rightPart;
            }
            else
            {
                int right = extIdx + 1;
                int left = extIdx;
                while (s1[left] != '(' && left >= 0) left--;
                for(i = 0; i < left; i++) leftPart += s1[i];
                for(i = right; i < s1.Length; i++) rightPart += s1[i];
                res = leftPart + $"({s2})" + rightPart;
            }

            return res;
        }

        static bool GroupFilesByData(string path)
        {
            bool result = false;

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                foreach (string file in files)
                {
                    string creationDate = Directory.GetCreationTime(file).Date.ToShortDateString();
                    Console.WriteLine($"{GetFileName(file)} - {creationDate}");
                    string newDirName = path + $@"\\{creationDate}";
                    if (!Directory.Exists(newDirName))
                    {
                        DirectoryInfo newDir = Directory.CreateDirectory(newDirName);
                        Console.WriteLine($"Created dir: {newDir.FullName}");
                    }
                    else
                    {
                        Console.WriteLine(newDirName);
                        Console.WriteLine("Directory already exists");
                    }

                    string newFileDir = newDirName + @$"\\{GetFileName(file)}";
                    
                    if (File.Exists(file))
                    {
                        if (!File.Exists(newFileDir))
                        {
                            File.Move(file, newFileDir);
                            Console.WriteLine("File moved");
                        }
                        else
                        {
                            int a = 1;
                            while (File.Exists(newFileDir))
                            {
                                newFileDir = InsertBeforeExt(newFileDir, $"{a++}");
                            }
                            if(!File.Exists(newFileDir)) File.Move(file, newFileDir);
                            else Console.WriteLine("File can't be moved");
                        }
                    }
                    else
                    {
                        Console.WriteLine("File not found");
                    }
                }
                result = true;
            }

            return result;
        }

        static string RemoveK(string s)
        {
            string res = "";

            for(int i = 0; i < s.Length; i++) if(s[i] != '"') res += s[i];

            return res;
        }
        
        static void Main(string[] args)
        {
            string testPath = @$"{RemoveK(Console.ReadLine())}";
            if (GroupFilesByData(testPath))
            {
                Console.WriteLine("Grouped");
            }
            else
            {
                Console.WriteLine("Not Grouped");
            }
        }
    }
}