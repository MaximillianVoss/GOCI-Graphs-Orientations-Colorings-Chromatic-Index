using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace GraphOrientations
{
    internal class NautyGraphOrientator 
    {
        private const string Etalon = "etalon.txt";
        private const string TempGraphsFileName = "temp_graphs.txt";
        private const string TempDirectGraphsFileName = "temp_derect_graphs.txt";
        private const string TrashFileName = "trash.txt";

        private static Encoding _encoding = GetEncoding(TempGraphsFileName);

        public IEnumerable<int> OrientWithoutGrahps(string graph6)
        {
            File.WriteAllText(TempGraphsFileName, graph6 + '\n', _encoding);

            using (var directg = new Process())
            {
                directg.StartInfo.FileName = "directg.exe";
                directg.StartInfo.Arguments = $"-o {TempGraphsFileName} {TempDirectGraphsFileName}";
                directg.StartInfo.UseShellExecute = false;
                directg.StartInfo.RedirectStandardOutput = true;
                directg.StartInfo.RedirectStandardInput = true;
                directg.StartInfo.RedirectStandardError = true;
                directg.Start();

                var current = directg.StandardError.ReadLine(); // этот нарцисс своё название в консоль пишет перед работой
                                                                // Еще в зеркало на себя подрочил бы

                for (; ; )
                {
                    current = directg.StandardOutput.ReadLine();
                   
                    if (string.IsNullOrEmpty(current))
                    {
                        break;
                    }
                }
            }

            using (var pickg = new Process())
            {
                pickg.StartInfo.FileName = "pickg.exe";
                pickg.StartInfo.Arguments = $"--a -V {TempDirectGraphsFileName} {TrashFileName}" + Environment.NewLine;
                pickg.StartInfo.UseShellExecute = false;
                pickg.StartInfo.RedirectStandardOutput = true;
                pickg.StartInfo.RedirectStandardInput = true;
                pickg.StartInfo.RedirectStandardError = true;
                pickg.Start();

                var current = pickg.StandardError.ReadLine(); // и этот туда же
                                                              
                var result = new List<string>();
                for (; ; )
                {
                    current = pickg.StandardError.ReadLine();
                    

                    if (string.IsNullOrEmpty(current))
                    {
                        break;
                    }

                    result.Add(current);
                }

                var res = new List<int>();
                for (int i = 0; i < result.Count; i++)
                {
                    var splited = result[i].Split('=');
                    if (splited.Length <= 1)
                    {
                        break;
                    }

                    res.Add(int.Parse(splited[^1]));
                }

                return res;
            }
        }

        private static Encoding GetEncoding(string filename)
        {
            // Read the BOM
            var bom = new byte[4];
            using (var file = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                file.Read(bom, 0, 4);
            }

            // Analyze the BOM
            if (bom[0] == 0x2b && bom[1] == 0x2f && bom[2] == 0x76) return Encoding.UTF7;
            if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf) return Encoding.UTF8;
            if (bom[0] == 0xff && bom[1] == 0xfe && bom[2] == 0 && bom[3] == 0) return Encoding.UTF32; //UTF-32LE
            if (bom[0] == 0xff && bom[1] == 0xfe) return Encoding.Unicode; //UTF-16LE
            if (bom[0] == 0xfe && bom[1] == 0xff) return Encoding.BigEndianUnicode; //UTF-16BE
            if (bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff) return new UTF32Encoding(true, true);  //UTF-32BE

            // We actually have no idea what the encoding is if we reach this point, so
            // you may wish to return null instead of defaulting to ASCII
            return Encoding.ASCII;
        }
    }
}
