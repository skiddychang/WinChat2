using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace WinChat2
{
    class FileHelper
    {
        public static void SaveLine(String Filename, String Line)
        {
            File.AppendAllText(Filename, Line + Environment.NewLine);

        }

        public static Dictionary<String, Object> GetDictionary(String Filename)
        {
            Dictionary<String, Object> Output = new Dictionary<String, Object>();
            String[] Lines;
            try
            {
                Lines = File.ReadAllLines(Filename);
            }
            catch { return null; }
            foreach (String Line in Lines)
            {
                String Key = Line.Trim().Split('=')[0].Trim().ToLower();
                Object Obj = Line.Trim().Split('=')[1].Trim();

                Output[Key] = Obj;
            }
            return Output;
        }

        public static void WriteToFile(String Filename, String Key, String Value)
        {
            String[] FileData = System.IO.File.ReadAllLines(Filename);
            StringBuilder Builder = new StringBuilder();
            Boolean Found = false;

            foreach (String Line in FileData)
            {
                String NewLine = Line;
                if (Line.Contains(Key))
                {
                    Found = true;
                    try
                    {
                        NewLine = Line.Replace(Line.Substring(Line.IndexOf("=") + 1, Line.Length - Line.IndexOf("=") - 1), Value);
                    }
                    catch
                    {
                        NewLine = Line;
                    }
                }

                Builder.Append(NewLine);
                Builder.Append(Environment.NewLine);
            }

            if (!Found)
                Builder.AppendFormat("{0}={1}", Key, Value);

            System.IO.File.WriteAllText(Filename, Builder.ToString());

        }

    }
}
