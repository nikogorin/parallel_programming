using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data_Sharing_And_Sync
{
    class LockRecursionDemo
    {
        static void Main(string[] args)
        {
            LockRecursion(5);

            Console.ReadKey();
            Console.WriteLine("All done here.");
        }



        // true = exception, false = deadlock
        static SpinLock sl = new SpinLock(true);

        private static void LockRecursion(int x)
        {
            // lock recursion is being able to take the same lock multiple times
            bool lockTaken = false;
            try
            {
                sl.Enter(ref lockTaken);
            }
            catch (LockRecursionException e)
            {
                Console.WriteLine("Exception: " + e);
            }
            finally
            {
                if (lockTaken)
                {
                    Console.WriteLine($"Took a lock, x = {x}.");
                    LockRecursion(x - 1);
                    sl.Exit();
                }
                else
                {
                    Console.WriteLine($"Failed to take a lock, x = {x}");
                }
            }
        }
    }
}
