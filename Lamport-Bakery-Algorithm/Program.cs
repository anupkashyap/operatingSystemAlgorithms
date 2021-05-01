using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LamportBakeryAlgo
{
    class Program
    {
        private const int threadCount = 5;
        private static int incrementVariable = 0;
        private static int[] tokens = new int[threadCount];
        private static bool[] choosing = new bool[threadCount];

        private static Mutex mutex = new Mutex();
        static void Main(string[] args)
        {
            var bag = new List<Task>();
            for (int i = 0; i < threadCount-1; i++)
            {
                bag.Add(Task.Run(() => RunIncrement(i)));
            }
            Task.WaitAll(bag.ToArray());
            Console.WriteLine(incrementVariable);
            Console.ReadKey();
        }

        static void RunIncrement(int threadNumber)
        {

            for (int i = 0; i < 100000; i++)
            {
                //Lock
                Lock(threadNumber);
                //Critical Section
                incrementVariable++;
                //Unlock
                UnLock(threadNumber);

            }
        }

        private static void Lock(int threadNumber)
        {
            choosing[threadNumber] = true;
            tokens[threadNumber] = tokens.Max() + 1;
            choosing[threadNumber] = false;
            for (var t = 0; t < tokens.Length; t++)
            {
                // Wait until thread t gets a token
                while (choosing[t]) ;
                // Wait for the current thread's turn to access CS
                while (tokens[t] != 0 && CompareToken(tokens[t], t, tokens[threadNumber], threadNumber)) ;
            }
        }
        private static void UnLock(int threadNumber)
        {
            //Release the token
            tokens[threadNumber] = 0;
        }



        private static bool CompareToken(int token1, int index1, int token2, int index2)
        {
            if (token1 < token2 || ((token1 == token2) && index1 < index2))
                return true;
            return false;
        }
    }
}
