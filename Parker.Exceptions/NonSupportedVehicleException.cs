using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parker.Exceptions
{
    public class NonSupportedVehicleException : Exception
    {
        public NonSupportedVehicleException(string vehicleType) 
            : base(string.Format("{0} cannot be parked in this parking",vehicleType)) 
        { 
        }
    }
}
