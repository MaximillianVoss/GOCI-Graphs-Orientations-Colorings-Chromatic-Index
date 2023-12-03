using System;
using System.IO;

namespace GraphOrientations.Writers
{
    internal class FileWriterCustom : IWriter
    {
        private readonly string _fileName;

        public FileWriterCustom(string FileName)
        {
            this._fileName = FileName ?? throw new ArgumentNullException(nameof(FileName));

            if (File.Exists(this._fileName))
            {
                File.WriteAllText(this._fileName, "");
            }
            else
            {
                using (FileStream fs = File.Create(this._fileName))
                {
                };
            }
        }

        public void Write(string s)
        {
            File.AppendAllText(this._fileName, s);
        }

        public void Write(int value)
        {
            File.AppendAllText(this._fileName, value.ToString());
        }

        public void WriteLine(string s)
        {
            File.AppendAllText(this._fileName, s + Environment.NewLine);
        }

        public void WriteLine()
        {
            File.AppendAllText(this._fileName, Environment.NewLine);
        }
    }
}
