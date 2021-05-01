using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PetersonAlgorithm
{
    class Program
    {
        private static bool[] flag = new bool[2];
        private static int turn;
        private static int incrementVariable=0;
        static void Main(string[] args)
        {
            var task1=Task.Run(() => Thread1());
            var task2 = Task.Run(() => Thread2());
            task1.Wait();
            task2.Wait();
            Console.WriteLine(incrementVariable);
        }

        static void Thread1()
        {
            for(int i = 0; i < 100000; i++){
                flag[0] = true;
                turn = 1;
                while (flag[1] == true && turn == 1) ;
                incrementVariable++;
                flag[0] = false;
            }
           
        }
        static void Thread2()
        {
            for (int i = 0; i < 100000; i++){
                flag[1] = true;
                turn = 0;
                while (flag[0] == true && turn == 0) ;
                incrementVariable++;
                flag[1] = false;
            }

        }
    }
}
