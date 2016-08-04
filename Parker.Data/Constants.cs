using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parker.Data
{
    public class Constants
    {
        public class FilePaths
        {
            public static string STORAGE_XML_FILE_NAME = @"Storage\ParkingData.xml";
            public static string STORAGE_XSD_FILE_NAME = @"Storage\ParkingData.xsd";
        }

        public class XMLElements
        {
            public static string FLOORS = "Floors";
            public static string FLOOR = "Floor";
            public static string FLOOR_NUMBER = "FloorNumber";
            public static string VEHICLE_MAX_LIMIT = "VehicleMaxLimit";
            public static string VEHICLE_COUNT = "VehicleCount";
        }
    }
}
