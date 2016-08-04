using Parker.Business;
using Parker.Entity;
using Parker.Interface;
using System.Collections.Generic;
using System.ServiceModel.Activation;
using System.Web;

namespace Parker.Service
{
    [AspNetCompatibilityRequirements(
        RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Parking : IParking
    {
        public List<string> GetAvailableFloors(string vehicleType)
        {
            HttpContext.Current.Server.MapPath(".");
            return new Worker().GetAvailableFloors(vehicleType);
        }

        public ParkingToken Checkin(string vehicleType, string parkingFloor, string vehicleInfo = null)
        {
            HttpContext.Current.Server.MapPath(".");
            return new Worker().Checkin(vehicleType, parkingFloor, vehicleInfo);
        }

        public bool Checkout(string vehicleType, string parkingFloor, string vehicleInfo = null)
        {
            HttpContext.Current.Server.MapPath(".");
            return new Worker().Checkout(vehicleType, parkingFloor, vehicleInfo);
        }
    }
}
