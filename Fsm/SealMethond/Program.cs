using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SealMethond
{
    class Program
    {
        static void Main(string[] args)
        {
            Parent p = new Parent();
            p.GetString();

            //p.GetString();
            //p.
            //Parent ps = new Son(1);
            //ps.GetString();
     
            Son s = new Son(2);
            s.GetString();
            Console.ReadKey();
        }


       
    }
    public  class Parent
    {
        public Parent() { Console.WriteLine("Parnet Contruct Method"); }
        //public Parent() { }
     // public Parent(int i) { }
        public void GetString()
        {
            Console.WriteLine("Parent。。。。");
        }
        protected  void GetInt() { }

    }

    public class Son:Parent
    {
        int i = 0;
        public Son(int i) { this.i = i; }
        public new void GetString()
        {

            Console.WriteLine("Son。。。。" + i);
           
        }
      

      
    }
}
