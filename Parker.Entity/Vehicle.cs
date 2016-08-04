using Parker.Entity.CustomType;
using System;

namespace Parker.Entity
{
    [Serializable]
    public class Vehicle
    {
        public string OwnerInfo { get; set; }

        public VehicleType VehicleType { get; set; }
    }
}
