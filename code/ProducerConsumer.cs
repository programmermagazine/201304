using System;
using System.Threading;
using System.Collections.Generic;

class ProducerConsumer
{
    static readonly int BUFFER_SIZE = 3;
    static Queue<int> queue = new Queue<int>(BUFFER_SIZE);
    static Semaphore filled = new Semaphore(0, BUFFER_SIZE);
    static Semaphore unfilled = new Semaphore(BUFFER_SIZE, BUFFER_SIZE);
    static Mutex mutex = new Mutex(false);

    static void Main(string[] args)
    {
        Thread producer = new Thread(new ThreadStart(produce));
        Thread consumer = new Thread(new ThreadStart(consume));
        producer.Start();
        consumer.Start();
        producer.Join();
        consumer.Join();
    }

    static Random random = new Random();

    private static void produce()
    {
        while (true)
        {
            Thread.Sleep(random.Next(0, 500));
            int produceNumber = random.Next(0, 20);
            Console.WriteLine("Produce: {0}", produceNumber);

            unfilled.WaitOne(); 	// wait(unfilled)
            mutex.WaitOne();				// wait(mutex)
            
            queue.Enqueue(produceNumber);

            mutex.ReleaseMutex();           // signal(mutex)
            filled.Release();		// signal(filled)

            if (produceNumber == 0)
                break;
        }
    }

    private static void consume()
    {
        while (true)
        {
            filled.WaitOne(); 		// wait(filled)
            mutex.WaitOne();				// wait(mutex)
            int number = queue.Dequeue();
            Console.WriteLine("Consume: {0}", number);

            mutex.ReleaseMutex();       	// signal(mutex)
            unfilled.Release();	// signal(unfilled)
            if (number == 0)
                break;
            Thread.Sleep(random.Next(0, 1000));
        }
    }
}