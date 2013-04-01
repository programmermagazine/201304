## 從 C# 看作業系統-(2) 競爭情況、鎖定與生產者/消費者問題 (作者：陳鍾誠)

## 競爭情況 (Race Condition)

至此，我們已經用 C# 實作了作業系統中的 Thread 與 Deadlock 這兩種慨念，但事實上、這兩個概念之間是有關係的，要理解 Thread 與死結之間的關係，就必須從 Race Condition (競爭情況) 這個問題談起。

在多 Thread (或多 CPU) 的情況之下，兩個 thread 可以共用某些變數，但是共用變數可能造成一個嚴重的問題，那就是當兩個 thread 同時修改一個變數時，這種修改會造成變數的值可能錯誤的情況，以下是一個範例。

Thread 1                Thread 2                Thread1+2 (第 1 種情況)    Thread1+2 (第 2 種情況)
----------------        ----------------        ------------------------   ------------------------ 
counter = 0                                                                     
...                                                                        
R1 = counter            R1 = counter            T1:R1 = counter            T1:R1 = counter
R1 = R1 + 1             R1 = R1 - 1             T2:R1 = counter            T1:R1 = R1+1
counter = R1            counter = R1            T2:R1 = R1-1               T2:R1 = counter
                                                T2:counter=R1              T2:R1 = R1-1
                                                T1:R1 = R1+1               T2:counter = R1
                                                T1:counter = R1            T1:counter = R1
                                                ...                        ...
                                                執行結果 : counter = 0     執行結果 : counter = -1

這種情況並非只在多 CPU 的系統裡會發生，在單 CPU 的多線程系統裡也會發生，因為一個高階語言指令在翻譯成機器碼時，通常會變成很多個指令，這讓修改的動作無法在單一指令內完成，如果這些修改動作執行到一半的時候，線程被切換了，就會造成上述的競爭情況。

高階語言            組合語言                對應動作
----------------    ----------------        ----------------
counter ++		    LD  R1, counter         R1 = counter
					ADD R1, R1, 1           R1 = R1 + 1
					ST  R1, counter         counter = R1

這種競爭情況對程式設計者而言是無法接受的，如果程式的執行結果無法確保，那所有的程式都將陷入一團混亂，連 counter++ 這樣的動作都有問題的話，那任何多線程的程式都將無法正確運作。

為了避免這樣的問題產生，一個可能的解決方法是採用鎖定 (lock) 的方式，當我們執行共用變數的修改時，先進行鎖定，讓其他的線程無法同時修改該變數，等到修改完畢後解索後，其他的線程才能修改該變數，這樣就能避免掉競爭情況的問題了。

但是、一但我們能夠進行鎖定的動作，那就可能會造成上述的死結情況，這也正是 Thread 與死結之間的關係，讓我們用一句話總結如下：

	因為多線程的程式會有競爭情況，為了避免該情況而引入了鎖定機制，
    但是鎖定機制用得不好就會造成死結。

讓我們先用程式來驗證競爭情況的存在，以下是一個 C# 的競爭情況範例 (當然這種競爭情況是我們刻意造成的)。

檔案：RaceConditon.cs

```CS
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
```

執行結果

```
D:\Dropbox\Public\cs\code>RaceCondition
inc:R1 = counter     R1:0 counter:0
dec:R1 = counter     R1:0 counter:0
inc:R1 = R1 + 1      R1:0 counter:0
inc:counter = R1     R1:0 counter:0
dec:R1 = R1 - 1      R1:0 counter:0
dec:counter = R1     R1:0 counter:0

D:\Dropbox\Public\cs\code>RaceCondition
inc:R1 = counter     R1:0 counter:0
inc:R1 = R1 + 1      R1:0 counter:0
dec:R1 = counter     R1:0 counter:0
inc:counter = R1     R1:-1 counter:0
dec:R1 = R1 - 1      R1:-1 counter:0
dec:counter = R1     R1:-1 counter:-1
```
	
要解決以上的競爭情況，必須採用一些協調 (Cooperation) 方法，C# 當中所提供的主要協調方法是 lock 這個語句。簡單來說，C# 的 lock 的實作方式就是採用作業系統教科書中所說的 Monitor 之方法，只是在 lock 的開始加入 Monitor.Enter() 語句，然後在 lock 的結束加入 Monitor.Exit() 語句而已，其方法如下所示。

C# lock						Monitor 語句
-----------------------		---------------------------
lock (_locker) {			Monitor.Enter(_locker);
   ...						   ...					
   critical();                 critical();
   ...                         ...
}							Monitor.Exit(_locker);

使用 lock 的方式，我們可以很輕易的解決上述程式的競爭情況，以下是該程式加入 lock 機制後的程式碼與執行結果。

檔案：RaceConditonLock.cs

```CS
using System;
using System.Threading;
using System.Collections.Generic;

class RaceConditionLock
{
    static int counter = 0, R1 = 0;
	static String counterLocker = "counterLocker";
	
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
		lock (counterLocker) {
			R1 = counter;
			waitAndSee("inc:R1 = counter");
			R1 = R1 + 1;
			waitAndSee("inc:R1 = R1 + 1 ");
			counter = R1;
			waitAndSee("inc:counter = R1");
		}
    }

    static void dec()
    {
		lock (counterLocker) {
			R1 = counter;
			waitAndSee("dec:R1 = counter");
			R1 = R1 - 1;
			waitAndSee("dec:R1 = R1 - 1 ");
			counter = R1;
			waitAndSee("dec:counter = R1");
		}
    }
}

```

執行結果

```
D:\Dropbox\Public\cs\code>csc RaceConditionLock.cs
適用於 Microsoft (R) .NET Framework 4.5 的
Microsoft (R) Visual C# 編譯器版本 4.0.30319.17929
Copyright (C) Microsoft Corporation. 著作權所有，並保留一切權利。


D:\Dropbox\Public\cs\code>RaceConditionLock
inc:R1 = counter     R1:0 counter:0
inc:R1 = R1 + 1      R1:1 counter:0
inc:counter = R1     R1:1 counter:1
dec:R1 = counter     R1:1 counter:1
dec:R1 = R1 - 1      R1:0 counter:1
dec:counter = R1     R1:0 counter:0

D:\Dropbox\Public\cs\code>RaceConditionLock
inc:R1 = counter     R1:0 counter:0
inc:R1 = R1 + 1      R1:1 counter:0
inc:counter = R1     R1:1 counter:1
dec:R1 = counter     R1:1 counter:1
dec:R1 = R1 - 1      R1:0 counter:1
dec:counter = R1     R1:0 counter:0

D:\Dropbox\Public\cs\code>RaceConditionLock
inc:R1 = counter     R1:0 counter:0
inc:R1 = R1 + 1      R1:1 counter:0
inc:counter = R1     R1:1 counter:1
dec:R1 = counter     R1:1 counter:1
dec:R1 = R1 - 1      R1:0 counter:1
dec:counter = R1     R1:0 counter:0
```

## 號誌 (Semaphore) 與生產者/消費者問題


```CS
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
```	

執行結果

```
D:\Dropbox\Public\pmag\201304\code>ProducerConsumer
Produce: 7
Consume: 7
Produce: 4
Produce: 12
Consume: 4
Produce: 16
Produce: 1
Consume: 12
Produce: 8
Consume: 16
Produce: 15
Produce: 7
Consume: 1
Produce: 18
Consume: 8
Produce: 0
Consume: 15
Consume: 7
Consume: 18
Consume: 0
```

另外、在作業系統中有個多行程的經典問題稱為 Dining Philospher (哲學家用餐) 問題，也可以採用 lock 的方法解決，由於這個問題實務上比較不那麼常用，本文中就不再詳細探討此一問題，有興趣的朋友可以看看網路上的解決方法，像是以下這篇 java2s 當中的程式就用 C# 實作解決了此一問題。

* <http://www.java2s.com/Tutorial/CSharp/0420__Thread/DiningPhilosopher.htm>

在本章中，我們購過透過實作的方式，讓讀者感受作業系統當中的 Thread、Deadlock、Race Condition、與 Semaphore 等概念，希望這樣的說明方式對讀者會有所幫助。

## 參考文獻
* Threading in C#, Joseph Albahari (超讚！)
	* <http://www.albahari.com/threading/>
* Agile and Domain-Driven in C#:Multithreaded Producer/Consumer using Semaphore and Mutex in C# 2.0
	* <http://agilelover.blogspot.tw/2006/09/multithreaded-producerconsumer-using.html>
