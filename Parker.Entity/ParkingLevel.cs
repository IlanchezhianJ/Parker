using Parker.Entity.CustomType;
using System;
using System.Collections.Generic;

namespace Parker.Entity
{
    [Serializable]
    public class ParkingLevel
    {
        private Dictionary<VehicleType, int> _vehicleMaxLimit
            = new Dictionary<VehicleType, int>();
        private Dictionary<VehicleType, int> _vehicleCount
            = new Dictionary<VehicleType, int>();


        public string FloorNumber { get; set; }

        public Dictionary<VehicleType, int> VehicleMaxLimit 
        {
            get { return _vehicleMaxLimit; }
            set { _vehicleMaxLimit = value; }
        }

        public Dictionary<VehicleType, int> VehicleCount 
        {
            get { return _vehicleCount; }
            set { _vehicleCount = value; }
        }
    }
}
