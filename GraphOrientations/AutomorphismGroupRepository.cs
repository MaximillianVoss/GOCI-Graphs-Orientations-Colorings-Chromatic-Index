using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;

namespace GraphOrientations
{
    internal class AutomorphismGroupRepository
    {
        public int GetNextAutomorphismGroupSize(string graphG6)
        {
            var process = new Process();
            process.StartInfo.FileName = "pickg.exe";
            process.StartInfo.Arguments = $"-V --a" + Environment.NewLine;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardError = true;
            process.Start();
            process.StandardInput.WriteLine(graphG6 + '\n');
            var result = process.StandardOutput.ReadLine();
            var err = process.StandardError.ReadLine();
            while (!err.Contains('='))
            {
                err = process.StandardError.ReadLine();
            }
            process.WaitForExit();
            return int.Parse(err.Split('=').Last().TakeWhile(x => char.IsDigit(x)).ToArray());
        }
    }
}
