using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Documents;
using System.Text.RegularExpressions;

namespace HeeckerenTest
{
    public class Parser
    {
        public delegate void Statistic(string s);
        public delegate void Progress(double d);

        public Parser() { }

        public List<SingleKill> Parse(List<String> strings, Statistic s, Progress p, CancellationToken ct)
        {
            s("Processing " + strings.Count + " lines");

            List<SingleKill> ret = new List<SingleKill>();

            double progress = 0.0;
            double step = 25.0 / (double)strings.Count;

            foreach (string str in strings)
            {
                string[] chunks = str.Split(' ');
                if (chunks.Count() != 4)
                {
                    s("Bad format!");
                    return null;
                }

                ret.Add(new SingleKill
                {
                    time = Convert.ToDateTime(chunks[0] + " " + chunks[1]),
                    killer = chunks[2],
                    victim = chunks[3]
                });

                //s(sk.time.ToString() + " " + sk.killer + " " + sk.victim);

                p(progress+=step);
                if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
            }
            return ret;
        }

        public List<string> Load(FileInfo file, Statistic s, CancellationToken ct)
        {
            s("Loading " + file.FullName);
            List<string> ret = new List<string>();
            try
            {
                using (StreamReader sr = new StreamReader(file.FullName, Encoding.Default))
                {
                    while (!sr.EndOfStream)
                    {
                        ret.Add(sr.ReadLine());
                        if (ct.IsCancellationRequested) ct.ThrowIfCancellationRequested();
                    }
                }
            }
            catch (IOException e) { s("Error opening file! " + e.Message); }
            return ret;
        }
    }
}
