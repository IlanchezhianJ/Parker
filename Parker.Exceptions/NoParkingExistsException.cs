using System;

namespace Parker.Exceptions
{
    public class NoParkingExistsException : Exception
    {
        public NoParkingExistsException(string vehicleType, string floor)
            : base(string.Format("Parking in floor {0} for vehicle type {1} is no longer exists.", floor, vehicleType))
        {
            
        }
    }
}
