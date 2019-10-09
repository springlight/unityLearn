using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StaticStructureMethod
{
    class Program
    {
        static void Main(string[] args)
        {
           // Console.WriteLine("Main-----" + UserPreferences.name);
            UserPreferences u = new UserPreferences();
            Console.WriteLine("22222----" + u.age);
            Console.WriteLine("u hascode1111---" + u.GetHashCode());
            u.Add(10);
            Console.WriteLine("3333----" + u.age);
            Console.ReadKey();
        }
    }
    public static class Ex
    {
        public static void Add(this UserPreferences u,int i)
        {
            Console.WriteLine("u hascode2222---" + u.GetHashCode());
            u.age += i;
        }
    }
    public class UserPreferences
    {
        //只读属性只能在构造器中复制
        //const字段只能在声明时初始化
        public static  readonly string name;
        public  int age = 1;
        //静态构造函数不需要修饰符,只能访问静态字段
        static UserPreferences()
        {
            Console.WriteLine("调用了静态构造函数");
            DateTime now = DateTime.Now;
            if(now.DayOfWeek == DayOfWeek.Saturday || now.DayOfWeek == DayOfWeek.Sunday)
            {
                name = "浪子111";
             
            }
            else
            {
                name = "浪子222";
            }
        }

        public UserPreferences()
        {
           // age = 10;
            Console.WriteLine("调用了实例构造造函数");
            //name = "浪子333";
        }
    }
    //结构不能包含无惨构造函数
    public struct Di
    {
        public int age ;
        public Di(int i)
        {
            age = i;
        }
    }
}
