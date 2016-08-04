using Parker.Data;
using Parker.Entity;
using Parker.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Parker.Business
{
    public class Worker
    {

        public List<string> GetAvailableFloors(string vehicleType)
        {
            List<string> floors = new List<string>();

            if (!isValidVehicle(vehicleType))
                throw new NonSupportedVehicleException(vehicleType);

            var vehicle = XmlOperation.Instance.GetVehicleType(vehicleType);

            foreach(var floor in GetParkingDetails())
            {
                //If that vehicle type is permitted in that floor
                if ((floor.VehicleMaxLimit.ContainsKey(vehicle) && floor.VehicleCount.ContainsKey(vehicle)) 
                    && (floor.VehicleMaxLimit[vehicle] > floor.VehicleCount[vehicle]))
                {
                    floors.Add(string.Format("Floor {0} is available.",floor.FloorNumber));
                }
            }

            if (floors.Count == 0)
                floors.Add("All parkings are full for this vehicle type");

            return floors;
        }


        public ParkingToken Checkin(string vehicleType, string floor, string vehicleInfo = null)
        {
            if (!isValidVehicle(vehicleType))
                throw new NonSupportedVehicleException(vehicleType);

            if (!ParkingStillExists(vehicleType, floor))
                throw new NoParkingExistsException(vehicleType, floor);

            var parkingNumber = XmlOperation.Instance.Checkin(vehicleType, floor);

            return new ParkingToken() 
            { 
                CheckinTime = DateTime.Now, 
                OwnerInfo = vehicleInfo,
                ParkingNumber = parkingNumber,
                VehicleType = XmlOperation.Instance.GetVehicleType(vehicleType)
            };
        }

        public bool Checkout(string vehicleType, string parkingFloor, string vehicleInfo = null)
        {
            if (!isValidVehicle(vehicleType))
                throw new NonSupportedVehicleException(vehicleType);

            return XmlOperation.Instance.Checkout(vehicleType, parkingFloor);
        }


        #region Private methods
        /// <summary>
        /// Check whether the vehicle type is supported or not
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <returns></returns>
        bool isValidVehicle(string vehicleType)
        {
            return XmlOperation.Instance.GetSupportedVehicles().Contains(vehicleType.ToLower());
        }

        List<ParkingLevel> GetParkingDetails()
        {
            return XmlOperation.Instance.GetCurrentParkingLevels();
        }

        private bool ParkingStillExists(string vehicleType, string floor)
        {
            var parkingDetails = GetParkingDetails();
            var parkingLevel = parkingDetails.Where(x => x.FloorNumber == floor).FirstOrDefault();
            if (parkingLevel == null)
                throw new InvalidFloorException(floor);

            var vehicle = XmlOperation.Instance.GetVehicleType(vehicleType);

            return (parkingLevel.VehicleMaxLimit[vehicle] > parkingLevel.VehicleCount[vehicle]);
        }
        #endregion

        
    }
}
