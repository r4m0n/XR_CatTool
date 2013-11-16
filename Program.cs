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
            string[] cat = File.ReadAllLines(args[0] + ".cat");
            FileStream dat = File.OpenRead(args[0] + ".dat");
            byte[] buff = new byte[BUFFSIZE];

            foreach (string l in cat)
            {
                List<string> pcs = l.Split(' ').ToList();
                if (pcs.Count >= 4)
                {
                    while (pcs.Count > 4)
                    {
                        pcs[0] = pcs[0] + pcs[1];
                        pcs.RemoveAt(1);
                    }
                    if (!string.IsNullOrWhiteSpace(Path.GetDirectoryName(pcs[0])) && !Directory.Exists(Path.GetDirectoryName(pcs[0])))
                    {
                        Directory.CreateDirectory(Path.GetDirectoryName(pcs[0]));
                    }
                    FileStream of = File.OpenWrite(pcs[0]);
                    int remBytes = int.Parse(pcs[1]);
                    while (remBytes > 0)
                    {
                        int br = dat.Read(buff, 0, Math.Min(BUFFSIZE, remBytes));
                        of.Write(buff, 0, br);
                        remBytes -= br;
                    }
                    of.Close();
                }
                else
                {
                    Console.WriteLine("Invalid line, contents: " + l);
                }
            }
            dat.Close();
        }
    }
}
