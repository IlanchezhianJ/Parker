using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parker.Data;
using Parker.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Parker.Test.MS.Parker.Test.Data
{
    [TestClass]
    public class TestXmlOperation
    {
        XmlOperation TargetClass;

        [TestInitialize]
        public void Initialize()
        {
            
        }

        [TestCleanup]
        public void Finalize()
        {
            TargetClass = null;
        }

        #region Positive
        /// <summary>
        /// Test the constructor is properly loading the xml file
        /// </summary>
        [TestMethod]
        public void TestConstructor()
        {
            TargetClass = new XmlOperation();
            XmlDocument OriginalXml = TargetClass.StorageXmlDocument;

            File.Copy(@"InputFiles/ValidFile.xml", TargetClass.XmlPath, true);
            TargetClass = new XmlOperation();
            XmlDocument NewXml = TargetClass.StorageXmlDocument;

            Assert.AreNotSame(OriginalXml, NewXml);
        }

        [TestMethod]
        public void TestGetCurrentParkingLevels()
        {
            TargetClass = new XmlOperation();
            File.Copy(@"InputFiles/ValidFile.xml", TargetClass.XmlPath, true);
            TargetClass = new XmlOperation();

            var ParkingLevels = TargetClass.GetCurrentParkingLevels();
            Assert.AreNotEqual(ParkingLevels, null);

            Assert.AreEqual(ParkingLevels.Count, 3);

            Assert.AreEqual(ParkingLevels.Count(x => x.FloorNumber == "Floor1"), 1);
            Assert.AreEqual(ParkingLevels.Count(x => x.FloorNumber == "Floor2"), 1);
            Assert.AreEqual(ParkingLevels.Count(x => x.FloorNumber == "Floor3"), 1);

            Assert.AreEqual(ParkingLevels.Where(x => x.FloorNumber == "Floor1").First().VehicleCount[Entity.CustomType.VehicleType.Bike], 120);
            Assert.AreEqual(ParkingLevels.Where(x => x.FloorNumber == "Floor1").First().VehicleCount[Entity.CustomType.VehicleType.Van], 20);
            Assert.AreEqual(ParkingLevels.Where(x => x.FloorNumber == "Floor1").First().VehicleCount[Entity.CustomType.VehicleType.Car], 15);

            Assert.AreEqual(ParkingLevels.Where(x => x.FloorNumber == "Floor1").First().VehicleMaxLimit[Entity.CustomType.VehicleType.Bike], 300);
            Assert.AreEqual(ParkingLevels.Where(x => x.FloorNumber == "Floor1").First().VehicleMaxLimit[Entity.CustomType.VehicleType.Van], 50);
            Assert.AreEqual(ParkingLevels.Where(x => x.FloorNumber == "Floor1").First().VehicleMaxLimit[Entity.CustomType.VehicleType.Car], 100);
        }

        [TestMethod]
        public void TestGetSupportedVehicles()
        {
            TargetClass = new XmlOperation();
            File.Copy(@"InputFiles/ValidFile.xml", TargetClass.XmlPath, true);
            TargetClass = new XmlOperation();

            var SupportedVehicles = TargetClass.GetSupportedVehicles();
            Assert.AreNotEqual(SupportedVehicles, null);

            Assert.AreEqual(SupportedVehicles.Count, 3);
            Assert.IsTrue(SupportedVehicles.Contains("bike"));
            Assert.IsTrue(SupportedVehicles.Contains("car"));
            Assert.IsTrue(SupportedVehicles.Contains("van"));
        }

        [TestMethod]
        public void TestCheckin()
        {
            TargetClass = new XmlOperation();
            File.Copy(@"InputFiles/ValidFile.xml", TargetClass.XmlPath, true);
            TargetClass = new XmlOperation();

            string vehicleType = "car";
            string floorName = "Floor1";
            int token = TargetClass.Checkin(vehicleType, floorName);
            Assert.AreNotEqual(token, -1);

            vehicleType = "bike";
            floorName = "Floor2";
            token = TargetClass.Checkin(vehicleType, floorName);
            Assert.AreNotEqual(token, -1);

            vehicleType = "van";
            floorName = "Floor3";
            token = TargetClass.Checkin(vehicleType, floorName);
            Assert.AreNotEqual(token, -1);
        }

        [TestMethod]
        public void TestCheckOut()
        {
            TargetClass = new XmlOperation();
            File.Copy(@"InputFiles/ValidFile.xml", TargetClass.XmlPath, true);
            TargetClass = new XmlOperation();

            string vehicleType = "car";
            string floorName = "Floor1";
            bool token = TargetClass.Checkout(vehicleType, floorName);
            Assert.IsTrue(token);

            vehicleType = "bike";
            floorName = "Floor2";
            token = TargetClass.Checkout(vehicleType, floorName);
            Assert.IsTrue(token);

            vehicleType = "van";
            floorName = "Floor3";
            token = TargetClass.Checkout(vehicleType, floorName);
            Assert.IsTrue(token);
        }
        #endregion

        #region Exception
        /// <summary>
        /// Test the constructor for invalid files
        /// </summary>
        [TestMethod]
        [ExpectedException(typeof(InvalidParkingStorageException))]
        public void TestConstructor_InvalidXml()
        {
            TargetClass = new XmlOperation();
            XmlDocument OriginalXml = TargetClass.StorageXmlDocument;
            File.Copy(@"InputFiles/InvalidFile.xml", TargetClass.XmlPath, true);
            TargetClass = new XmlOperation();
            Assert.AreSame(TargetClass.StorageXmlDocument, OriginalXml);

            File.Copy(@"InputFiles/TextFile.xml", TargetClass.XmlPath, true);
            TargetClass = new XmlOperation();
            Assert.AreSame(TargetClass.StorageXmlDocument, OriginalXml);
        }
        #endregion
    }
}

