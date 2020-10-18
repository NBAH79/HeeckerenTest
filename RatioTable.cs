using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace HeeckerenTest
{
    class RatioTable
    {
        int[,] t;

        public RatioTable(int size){
            t=new int[size,size];
        }

        public void Add(int k,int v)=> t[k,v]++;

        public int Get(int k,int v)=> t[k,v];

        //how many people are victims in pairs
        public int GetLevel(int k){
            int Level = 0;
            for(int n=0;n<t.GetLongLength(0);n++) {
                int diff=t[k,n]-t[n,k]; //kill - death
                if (diff>0) Level++;
                if (diff<0) Level--;
                }
            return Level;
        }

        public int GetRelations(int k){
            int r = 0;
            for(int n=0;n<t.GetLongLength(0);n++) if (t[k,n]>0 || t[n,k]>0) r++;
            return r;
        }
            

        public int GetKills(int k){
            int res=0;
            for (int n = 0; n < t.GetLongLength(0); n++) res+=t[k,n];
            return res;
        }

        public long Total(){
            long k=0;
            foreach(int i in t) k+=i;
            return k;
        }
    }
}
