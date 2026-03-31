using System;
using System.Collections.Generic;

namespace FifoScheduler.View
{
    public interface IMainView
    {
        void UpdateReadyQueue(List<string> processes);
        void UpdateBlockedQueue(List<string> processes);
        void UpdateRunningProcess(string name, int remaining);
        void UpdateStats(string stats);

        event EventHandler StartClicked;
        event EventHandler StopClicked;
    }
}
