using HousingManagementSystem.Models;

namespace HousingManagementSystem
{
    public class Janitor : Employee
    {
        public Janitor(int id, string name) : base(id, name, "Дворник") { }

        public override bool CanHandleRequest(Request request)
        {
            return request.Type == RequestType.Cleaning || request.Type == RequestType.YardWork;
        }
    }
}