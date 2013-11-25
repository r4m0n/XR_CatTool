using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XCatTool
{
    class Program
    {
        const int BUFFSIZE = 1024 * 64;

        static void Main(string[] args)
        {
            var catFileContents = File.ReadAllLines(args[0] + ".cat");
            var datFileStream = File.OpenRead(args[0] + ".dat");
            var buffer = new byte[BUFFSIZE];

            foreach (var line in catFileContents)
            {
                var pieces = line.Split(' ').ToList();
                if (pieces.Count >= 4)
                {
                    while (pieces.Count > 4)
                    {
                        pieces[0] = pieces[0] + pieces[1];
                        pieces.RemoveAt(1);
                    }
                    if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(pieces[0])) && !Directory.Exists(Path.GetDirectoryName(pieces[0])))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(pieces[0]));
                    }
                    FileStream of = File.OpenWrite(pieces[0]);
                    int remBytes = int.Parse(pieces[1]);
                    while (remBytes > 0)
                    {
                        int br = datFileStream.Read(buffer, 0, Math.Min(BUFFSIZE, remBytes));
                        of.Write(buffer, 0, br);
                        remBytes -= br;
                    }
                    of.Close();
                }
                else
                {
                    Console.WriteLine("Invalid line, contents: " + line);
                }
            }
            datFileStream.Close();
        }
    }
}
