﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Data_Sharing_And_Sync
{
    public class BankAccount_SP
    {

        private int balance;
        public int Balance 
        {
            get { return balance; }
            private set { balance = value; } 
        }

        public void Deposit(int amount)
        {
            balance += amount;
        }

        public void Withdraw(int amount)
        {
            balance -= amount;
        }
    }
    class SpinLockDemo
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Program Start...");
            var tasks = new List<Task>();
            var ba = new BankAccount_SP();

            SpinLock sl = new SpinLock();

            for (int i = 0; i < 10; i++)
            {
                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Deposit(100);
                            
                        }
                        finally
                        {
                            if (lockTaken) sl.Exit();
                        }
                    }
                }));

                tasks.Add(Task.Factory.StartNew(() =>
                {
                    for (int j = 0; j < 1000; j++)
                    {
                        var lockTaken = false;
                        try
                        {
                            sl.Enter(ref lockTaken);
                            ba.Withdraw(100);

                        }
                        finally
                        {
                            if (lockTaken) sl.Exit();
                        }
                    }
                }));

            }

            Task.WaitAll(tasks.ToArray());

            Console.WriteLine($"Final balance is {ba.Balance}.");

            Console.WriteLine("All done here.");
        }
    }
}
