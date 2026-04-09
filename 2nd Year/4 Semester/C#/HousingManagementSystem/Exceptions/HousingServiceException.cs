using System;

namespace HousingManagementSystem
{
    public class HousingServiceException : Exception
    {
        public HousingServiceException(string message) : base(message) { }
    }
}