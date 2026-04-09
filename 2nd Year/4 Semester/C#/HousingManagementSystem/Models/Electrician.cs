using HousingManagementSystem.Models;

namespace HousingManagementSystem
{
    public class Electrician : Employee
    {
        public Electrician(int id, string name) : base(id, name, "Электрик") { }

        public override bool CanHandleRequest(Request request)
        {
            return request.Type == RequestType.Electrical;
        }
    }
}