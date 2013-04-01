using System;
using System.Threading;
using System.Collections.Generic;

class RaceCondition
{
    static int counter = 0, R1 = 0;
    static void Main(string[] args)
    {
        Thread thread1 = new Thread(inc);
        Thread thread2 = new Thread(dec);
        thread1.Start();
        thread2.Start();
        thread1.Join();
        thread2.Join();
    }

    static Random random = new Random();

    static void waitAndSee(String msg)
    {
        Thread.Sleep(random.Next(0, 10));
        Console.WriteLine(msg+"     R1:"+R1+" counter:"+counter);
    }

    static void inc()
    {
        R1 = counter;
        waitAndSee("inc:R1 = counter");
        R1 = R1 + 1;
        waitAndSee("inc:R1 = R1 + 1 ");
        counter = R1;
        waitAndSee("inc:counter = R1");
    }

    static void dec()
    {
        R1 = counter;
        waitAndSee("dec:R1 = counter");
        R1 = R1 - 1;
        waitAndSee("dec:R1 = R1 - 1 ");
        counter = R1;
        waitAndSee("dec:counter = R1");
    }
}