using System.Collections.Generic;

namespace FifoScheduler.Model
{
    public interface IScheduler
    {
        void RequestBlock(Process process);
        void RequestUnblock(Process process);
        void ProcessFinished(Process process);
        bool TryGetCpu(Process process);
        void ReturnCpu(Process process);
    }
}
