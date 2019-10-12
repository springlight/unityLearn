using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace stringtest
{
    class Program
    {
        static void Main(string[] args)
        {
            StringData s1 = new StringData();
            s1.str = "111";
            StringData s2 = new StringData();
            s2.str = s1.str;
            string tmp = s1.str;
            Console.WriteLine("s1.hashcode --{0},s1.hashcode---{1},tmp--{2}", s1.str.GetHashCode(), s2.GetHashCode(),tmp.GetHashCode());
            Console.WriteLine("s1.str -{0}/ s2.str-{1}/tmp-{2}",s1.str,s2.str,tmp);
            tmp = "222";
            Console.WriteLine("------------------------");
            Console.WriteLine("s1.str -{0}/ s2.str-{1}/tmp-{2}", s1.str, s2.str, tmp);
            Console.WriteLine("s1.hashcode --{0},s1.hashcode---{1},tmp--{2}", s1.str.GetHashCode(), s2.GetHashCode(), tmp.GetHashCode());
            Console.ReadKey();

        }
    }

    public class StringData
    {
        public string str;
    }
}
