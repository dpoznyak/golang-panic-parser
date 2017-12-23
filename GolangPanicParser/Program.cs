using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace GolangPanicParser
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var instream = File.OpenText(args[0]))
            {
                var d = new Dictionary<string, int>();

                var goroutineHeaderRx = new Regex(@"^goroutine");
                var maskRx = new Regex(@"0x\w+");
                string aggLine = null;
                while (!instream.EndOfStream)
                {
                    var line = instream.ReadLine();
                    if (goroutineHeaderRx.IsMatch(line))
                    {
                        //Console.WriteLine("header: {0}", line);
                        var key = maskRx.Replace(aggLine, "***");
                        if (!d.TryGetValue(key, out var c))
                        {
                            c = 0;
                        }
                        d[ key] = c + 1;

                        aggLine = null;
                    }
                    else
                    {
                        aggLine = (aggLine != null ? aggLine + "\n" : "") + line;
                    }
                }

                //var regex = new Regex("(goroutine.*?\n(.(?!goroutine))+)+", RegexOptions.Singleline );

                //var dict = regex.Matches(instream.ReadToEnd()).ToLookup(x => x.Groups[2].Value)
                //    .ToDictionary(x => x.Key, x => x.Count());

                new JsonSerializer() { Formatting =  Formatting.Indented}.Serialize(Console.Out, d.Where(x => x.Value > 1));
            }
        }
    }
}
