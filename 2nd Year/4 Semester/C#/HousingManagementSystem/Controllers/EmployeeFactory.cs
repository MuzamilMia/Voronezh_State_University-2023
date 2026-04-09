using System.Threading;

namespace HousingManagementSystem
{
    public static class EmployeeFactory
    {
        public static Employee CreateEmployee(string employeeType, int id, string name)
        {
            switch (employeeType.ToLower())
            {
                case "plumber":
                    return new Plumber(id, name);
                case "electrician":
                    return new Electrician(id, name);
                case "janitor":
                    return new Janitor(id, name);
                case "dispatcher":
                    return new Dispatcher(id, name);
                default:
                    throw new HousingServiceException($"Unknown employee type: {employeeType}");
            }
        }
    }
}