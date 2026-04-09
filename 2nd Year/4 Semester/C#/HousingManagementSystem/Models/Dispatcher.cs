using HousingManagementSystem.Models;

namespace HousingManagementSystem
{
    public class Dispatcher : Employee
    {
        public Dispatcher(int id, string name) : base(id, name, "Диспетчер") { }

        public override bool CanHandleRequest(Request request)
        {
            return false; // Dispatcher doesn't handle requests directly
        }
    }
}