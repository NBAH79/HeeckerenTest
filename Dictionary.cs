using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace HeeckerenTest
{
    public class Dictionary
    {
        List<string> names = new List<string>();

        public void AddName(string name) {
            if (FindName(name) == -1) names.Add(name);
        }

        public int FindName(string name)
        {
            for (int n = 0; n < names.Count; n++) if (String.Compare(name, names[n])==0) return n;
            return -1;
        }

        public Dictionary(List<SingleKill> kills)
        {
            foreach (SingleKill sk in kills)
            {
                AddName(sk.killer);
                AddName(sk.victim);
            }
        }

        public int Count { get { return names.Count; } }
        public string Name(int n) => names[n];
    }
}
