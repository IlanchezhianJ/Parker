using Parker.Entity;
using Parker.Entity.CustomType;
using Parker.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace Parker.Data
{
    public class XmlOperation
    {

        #region Properties
        private string AssemblyDirectory
        {
            get
            {
                string codeBase = Assembly.GetExecutingAssembly().CodeBase;
                UriBuilder uri = new UriBuilder(codeBase);
                string path = Uri.UnescapeDataString(uri.Path);
                return Path.GetDirectoryName(path);
            }
        }

        private string XsdPath
        {
            get
            {
                return Path.Combine(AssemblyDirectory, Constants.FilePaths.STORAGE_XSD_FILE_NAME);
            }
        }

        public string XmlPath
        {
            get
            {
                return Path.Combine(AssemblyDirectory, Constants.FilePaths.STORAGE_XML_FILE_NAME);
            }
        }



        private XmlDocument _xmlDocument = new XmlDocument();
        public XmlDocument StorageXmlDocument 
        {
            get { return _xmlDocument; }
            set { _xmlDocument = value; }
        }

        /// <summary>
        /// Singleton instance for XmlOperation
        /// </summary>
        private static XmlOperation _instance;
        public static XmlOperation Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new XmlOperation();
                }
                return _instance;
            }
        }
        #endregion

        #region Constructor
        public XmlOperation()
        {

            if (!ValidStorageFile())
                throw new InvalidParkingStorageException();

            LoadData();
        }
        #endregion

        #region Public methods

        public List<ParkingLevel> GetCurrentParkingLevels()
        {
            var currentParkingLevels = new List<ParkingLevel>();

            string floorPath = string.Format("/{0}/{1}"
                , Constants.XMLElements.FLOORS
                , Constants.XMLElements.FLOOR);

            var floors = StorageXmlDocument.SelectNodes(floorPath);

            foreach (XmlNode floor in floors)
            {
                currentParkingLevels.Add(new ParkingLevel() 
                { 
                    FloorNumber = GetFloorNumber(floor),
                    VehicleCount = GetVehicleCount(floor),
                    VehicleMaxLimit = GetVehicleMaxLimit(floor)
                });
            }

            return currentParkingLevels;
        }



        /// <summary>
        /// Get the supported vehicles
        /// </summary>
        /// <returns></returns>
        public List<string> GetSupportedVehicles()
        {
            string vehicleMaxLimitPath = string.Format("/{0}/{1}/{2}"
                ,Constants.XMLElements.FLOORS
                ,Constants.XMLElements.FLOOR
                ,Constants.XMLElements.VEHICLE_MAX_LIMIT);

            var vehicleMaxLimits = StorageXmlDocument.SelectNodes(vehicleMaxLimitPath);
            var supportedVehicles = new List<string>();

            foreach (XmlNode vehicleMaxLimit in vehicleMaxLimits)
            {
                foreach(XmlNode vehicle in vehicleMaxLimit.ChildNodes)
                {
                    supportedVehicles.Add(vehicle.Name.ToLower());
                }
            }

            return supportedVehicles.Distinct().ToList();
        }

        public int Checkin(string vehicleType, string floorName)
        {
            return UpdateXml(vehicleType, floorName, 1);
        }

        public bool Checkout(string vehicleType, string floorName)
        {
            UpdateXml(vehicleType, floorName, -1);
            return true;
        }
        #endregion

        #region Private methods
        private void LoadData()
        {
            StorageXmlDocument.Load(XmlPath);
        }

        private int UpdateXml(string vehicleType, string floorName, int value)
        {
            string parkingNo = string.Empty;
            bool validFloor = false;

            string floorsPath = string.Format("/{0}/{1}/{2}/{3}"
                , Constants.XMLElements.FLOORS
                , Constants.XMLElements.FLOOR
                , Constants.XMLElements.VEHICLE_COUNT
                , vehicleType);

            var floors = StorageXmlDocument.SelectNodes(floorsPath);

            foreach (XmlNode vehicle in floors)
            {
                if (GetFloorNumber(vehicle.ParentNode.ParentNode) == floorName)
                {
                    parkingNo = vehicle.InnerText = (Convert.ToInt32(vehicle.InnerText) + value).ToString(); //Increase the parking
                    validFloor = true;
                    break;
                }
            }

            // save the XmlDocument back to disk
            if (validFloor)
                StorageXmlDocument.Save(XmlPath);
            else
                throw new InvalidFloorException(floorName);

            return Convert.ToInt32(parkingNo);
        }

        private XmlNodeList GetFloors()
        {
            string floorPath = string.Format("/{0}/{1}"
                , Constants.XMLElements.FLOORS
                , Constants.XMLElements.FLOOR);

            return StorageXmlDocument.SelectNodes(floorPath);
        }

        private string GetFloorNumber(XmlNode floorNode)
        {
            return floorNode[Constants.XMLElements.FLOOR_NUMBER].InnerText;
        }

        private Dictionary<VehicleType, int> GetVehicleMaxLimit(XmlNode floorNode)
        {
            Dictionary<VehicleType, int> maxLimit = new Dictionary<VehicleType, int>();

            string vehicleMaxLimitPath = string.Format("/{0}"
                , Constants.XMLElements.VEHICLE_MAX_LIMIT);

            XmlNode vehicleMaxLimit = floorNode[Constants.XMLElements.VEHICLE_MAX_LIMIT];
            foreach (XmlNode vehicle in vehicleMaxLimit.ChildNodes)
            {
                maxLimit.Add(
                    GetVehicleType(vehicle.Name)
                    , GetItemCount(vehicle)
                );
            }

            return maxLimit;
        }

        private Dictionary<VehicleType, int> GetVehicleCount(XmlNode floorNode)
        {
            Dictionary<VehicleType, int> vehicleCount = new Dictionary<VehicleType, int>();

            XmlNode vehicleMaxCount = floorNode[Constants.XMLElements.VEHICLE_COUNT];
            foreach (XmlNode vehicle in vehicleMaxCount.ChildNodes)
            {
                vehicleCount.Add(
                    GetVehicleType(vehicle.Name)
                    , GetItemCount(vehicle)
                );
            }

            return vehicleCount;
        }

        private int GetItemCount(XmlNode vehicleType)
        {
            return Convert.ToInt32(vehicleType.InnerText);
        }

        /// <summary>
        /// If a new vehicle type is added, update this method
        /// </summary>
        /// <param name="vehicleType"></param>
        /// <returns></returns>
        public VehicleType GetVehicleType(string vehicleType)
        {
            switch (vehicleType.ToLower().Trim())
            {
                case "car": return VehicleType.Car;
                case "bike": return VehicleType.Bike;
                case "van": return VehicleType.Van;
                default: throw new NonSupportedVehicleException(vehicleType);
            }

        }

        /// <summary>
        /// Validate the XML file
        /// </summary>
        /// <returns></returns>
        private bool ValidStorageFile()
        {
            bool errors = true;
            XmlSchemaSet schemas = new XmlSchemaSet();
            schemas.Add("", XsdPath);
            XDocument document = XDocument.Load(XmlPath);
            document.Validate(schemas, (SchemaObject, eventHandler) => { errors = false; });
            return errors;
        }
        #endregion
    }
}
