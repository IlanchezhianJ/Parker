using Parker.Entity;
using System.Collections.Generic;
using System.ServiceModel;

namespace Parker.Interface
{
    [ServiceContract]
    public interface IParking
    {
        /// <summary>
        /// Get the available parking floors for the given vehicle type
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <returns></returns>
        [OperationContract]
        List<string> GetAvailableFloors(string vehicleType);

        /// <summary>
        /// vehicle checkin service
        /// </summary>
        /// <param name="vehicleType">Permitted types are car, van and bike</param>
        /// <param name="parkingFloor">Parking floor from the list of available parking</param>
        /// <param name="vehicleInfo">Optional info: like vehicle number, owner info, etc..</param>
        /// <returns></returns>
        [OperationContract]
        ParkingToken Checkin(string vehicleType, string parkingFloor, string vehicleInfo = null);

        /// <summary>
        /// Vehicle checkout
        /// </summary>
        /// <param name="ParkingID">ParkingNumber from parking token</param>
        /// <returns></returns>
        [OperationContract]
        bool Checkout(string vehicleType, string parkingFloor, string vehicleInfo = null);
    }
}
