using System;
using System.Collections.Generic;
using System.Threading;
//Создать многопоточное приложение с одним потоком - писателем, который в случайные моменты
//времени помещает данные в буфер и сообщает об этом. Главный поток в случайные моменты времени
//порождает потоки - читатели, которые в случайные моменты времени удаляют данные из буфера с
//соответствующим сообщением. Каждый поток – читатель завершается после удаления заданного  числа данных.
//Все читатели и писатели используют один и тот же буфер.
public class BufferQueue
{
    private static int nextId = 1;
    private readonly Queue<object> queue;
    private readonly int maxSize;
    private readonly Semaphore emptySlots;
    private readonly Semaphore fullSlots;
    private readonly Mutex bufferMutex;

    public int Id { get; }
    public int MaxSize => maxSize;
    public event Action<BufferQueue> BufferChanged;

    public BufferQueue(int maxSize, Action<BufferQueue> onChanged = null)
    {
        Id = Interlocked.Increment(ref nextId);
        this.maxSize = Math.Max(1, maxSize);
        queue = new Queue<object>(this.maxSize);
        emptySlots = new Semaphore(this.maxSize, this.maxSize);
        fullSlots = new Semaphore(0, this.maxSize);
        bufferMutex = new Mutex();

        if (onChanged != null) BufferChanged += onChanged;
    }

    public int Count
    {
        get
        {
            bufferMutex.WaitOne();
            try { return queue.Count; }
            finally { bufferMutex.ReleaseMutex(); }
        }
    }

    public bool IsFull
    {
        get
        {
            bufferMutex.WaitOne();
            try { return queue.Count >= maxSize; }
            finally { bufferMutex.ReleaseMutex(); }
        }
    }

    public bool IsEmpty
    {
        get
        {
            bufferMutex.WaitOne();
            try { return queue.Count == 0; }
            finally { bufferMutex.ReleaseMutex(); }
        }
    }

    public bool Put(object data)
    {
        emptySlots.WaitOne();
        bufferMutex.WaitOne();
        try
        {
            queue.Enqueue(data);
            BufferChanged?.Invoke(this);
            return true;
        }
        finally
        {
            bufferMutex.ReleaseMutex();
            fullSlots.Release();
        }
    }

    public object Take()
    {
        fullSlots.WaitOne();
        bufferMutex.WaitOne();
        object item;
        try
        {
            item = queue.Dequeue();
            BufferChanged?.Invoke(this);
        }
        finally
        {
            bufferMutex.ReleaseMutex();
            emptySlots.Release();
        }
        return item;
    }

    public object Peek()
    {
        bufferMutex.WaitOne();
        try
        {
            return queue.Count > 0 ? queue.Peek() : null;
        }
        finally
        {
            bufferMutex.ReleaseMutex();
        }
    }

    public void WaitUntilEmpty()
    {
        while (true)
        {
            bufferMutex.WaitOne();
            try
            {
                if (queue.Count == 0) break;
            }
            finally
            {
                bufferMutex.ReleaseMutex();
            }
            Thread.Sleep(50);
        }
    }
}