using System;
using System.Threading;

public class Writer
{
    private static int nextId = 1;
    private readonly Thread thread;
    private readonly BufferQueue buffer;
    private volatile bool isRunning = false;
    private volatile bool isPaused = false;
    private readonly Action<string> logger;
    private readonly Action<Writer, BufferQueue, string> onStopped;
    private readonly Action<string, string, string> updateWorkerStatus;
    public int Id { get; }
    private readonly Random rnd = new Random();

    public Writer(BufferQueue buffer, Action<string> logger, Action<Writer, BufferQueue, string> onStopped, Action<string, string, string> updateWorkerStatus)
    {
        Id = Interlocked.Increment(ref nextId);
        this.buffer = buffer;
        this.logger = logger ?? (_ => { });
        this.onStopped = onStopped;
        this.updateWorkerStatus = updateWorkerStatus;
        thread = new Thread(Run) { IsBackground = true, Name = $"Writer-{Id}" };
    }

    public void Start()
    {
        isRunning = true;
        thread.Start();
        updateWorkerStatus?.Invoke(Id.ToString(), "Writer", "Running");
    }

    public void Pause()
    {
        isPaused = true;
        updateWorkerStatus?.Invoke(Id.ToString(), "Writer", "Paused");
    }

    public void Resume()
    {
        isPaused = false;
        updateWorkerStatus?.Invoke(Id.ToString(), "Writer", "Running");
    }

    public void Stop()
    {
        isRunning = false;
        updateWorkerStatus?.Invoke(Id.ToString(), "Writer", "Stopping");
    }

    private void Run()
    {
        logger?.Invoke($"Writer #{Id} started for buffer #{buffer.Id}.");
        try
        {
            while (isRunning)
            {
                if (isPaused)
                {
                    Thread.Sleep(100);
                    continue;
                }

                string data = $"W{Id}-{DateTime.Now:HHmmssfff}";
                //bool ok = buffer.Put(data);

                if (buffer.Put(data))
                {
                    logger?.Invoke($"Writer #{Id}: положил '{data}' в буфер #{buffer.Id}.");
                    updateWorkerStatus?.Invoke(Id.ToString(), "Writer", $"LastPut: Buf#{buffer.Id}");
                }

                Thread.Sleep(rnd.Next(200, 800));
            }

            buffer.WaitUntilEmpty();
            logger?.Invoke($"Writer #{Id}: буфер пуст, завершение работы.");
        }
        catch (ThreadInterruptedException) { }
        catch (Exception ex)
        {
            logger?.Invoke($"Writer #{Id} exception: {ex}");
        }
        finally
        {
            logger?.Invoke($"Writer #{Id} finished.");
            updateWorkerStatus?.Invoke(Id.ToString(), "Writer", "Stopped");
            onStopped?.Invoke(this, buffer, "Completed");
        }
    }
}