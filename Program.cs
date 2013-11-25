using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XCatTool
{
    class Program
    {
        const int BUFFER_SIZE = 1024 * 64;

        static void Main(string[] args)
        {
            new Program(args);
        }

        public Program(string[] args)
        {
            if (args.Length != 1)
            {
                Console.WriteLine("Usage: XCatTool.exe <catfilename>");
                return;
            }

            var inputFile = Path.GetFileNameWithoutExtension(args[0]);
            var catFile = String.Concat(inputFile, ".cat");
            var datFile = String.Concat(inputFile, ".dat");

            if (!File.Exists(catFile) || !File.Exists(datFile))
            {
                Console.WriteLine("Could not locate the {0} or {1} files", catFile, datFile);
                return;
            }

            Extract(catFile, datFile);
        }

        private void Extract(string catFile, string datFile)
        {
            var catFileContents = File.ReadAllLines(catFile);
            using (var datFileStream = File.OpenRead(datFile))
            {
                var buffer = new byte[BUFFER_SIZE];

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

                        using (var outputFile = File.OpenWrite(pieces[0]))
                        {
                            int remBytes = int.Parse(pieces[1]);

                            while (remBytes > 0)
                            {
                                int br = datFileStream.Read(buffer, 0, Math.Min(BUFFER_SIZE, remBytes));
                                outputFile.Write(buffer, 0, br);
                                remBytes -= br;
                            }
                        }                        
                    }
                    else
                    {
                        Console.WriteLine("Invalid line, contents: " + line);
                    }
                }
            }
        }
    }
}
