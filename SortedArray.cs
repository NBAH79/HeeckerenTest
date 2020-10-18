using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HeeckerenTest
{
    public class SortedArray
    {
        //number in dictionary | level | relations
        List<(int,int,int)> array=new List<(int, int, int)>();

        public SortedArray(int size){
            //array=new (int,int)[size];
        }

        public List<(int,int,int)> Array(){
            return array;
        }

        public void Add((int,int,int)v){
            int position=0;
            if (position == array.Count) { array.Add(v); return; }

            while (array[position].Item2>v.Item2) { if ((++position)==array.Count) break; }
            if (position == array.Count) { array.Add(v); return; }
            array.Insert(position,v);
        }

        public int Count { get { return array.Count; } }
    }
}
