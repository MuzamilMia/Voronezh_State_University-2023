using HousingManagementSystem.Models;

namespace HousingManagementSystem
{
    public class Plumber : Employee
    {
        public Plumber(int id, string name) : base(id, name, "Сантехник") { }

        public override bool CanHandleRequest(Request request)
        {
            return request.Type == RequestType.Plumbing;
        }
    }
}