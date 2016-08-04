using System;

namespace Parker.Exceptions
{
    public class InvalidFloorException : Exception 
    {
        public InvalidFloorException(string floor) 
            : base(string.Format("Floor {0} is invalid.", floor)) 
        { 
        }
    }
}