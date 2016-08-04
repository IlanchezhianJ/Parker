using Microsoft.VisualStudio.TestTools.UnitTesting;
using Parker.Business;
using Parker.Data;
using Parker.Exceptions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parker.Test.MS.Parker.Test.Business
{
    [TestClass]
    public class TestWorker
    {
        Worker TargetClass;

        XmlOperation DataOperation;

        [TestInitialize]
        public void Initialize()
        {
            TargetClass = new Worker();
        }

        [TestCleanup]
        public void Finalize()
        {
            TargetClass = null;
        }

        #region Postive
        [TestMethod]
        public void TestGetAvailableFloors()
        {
            DataOperation = new XmlOperation();
            File.Copy(@"InputFiles/ValidFile.xml", DataOperation.XmlPath, true);
            DataOperation = new XmlOperation();

            var floors = TargetClass.GetAvailableFloors("car");
            Assert.AreNotEqual(floors, null);

            Assert.AreEqual(floors.Count, 3);
            Assert.IsTrue(floors.Contains("Floor Floor1 is available."));
            Assert.IsTrue(floors.Contains("Floor Floor2 is available."));
            Assert.IsTrue(floors.Contains("Floor Floor3 is available."));
        }

        [TestMethod]
        public void TestCheckin()
        {
            DataOperation = new XmlOperation();
            File.Copy(@"InputFiles/ValidFile.xml", DataOperation.XmlPath, true);
            DataOperation = new XmlOperation();

            string vehicleType = "car";
            string floorName = "Floor1";
            var token = TargetClass.Checkin(vehicleType, floorName);
            Assert.AreNotEqual(token.ParkingNumber, -1);

            vehicleType = "bike";
            floorName = "Floor2";
            token = TargetClass.Checkin(vehicleType, floorName);
            Assert.AreNotEqual(token.ParkingNumber, -1);

            vehicleType = "van";
            floorName = "Floor3";
            token = TargetClass.Checkin(vehicleType, floorName);
            Assert.AreNotEqual(token.ParkingNumber, -1);
        }

        [TestMethod]
        public void TestCheckOut()
        {
            DataOperation = new XmlOperation();
            File.Copy(@"InputFiles/ValidFile.xml", DataOperation.XmlPath, true);
            DataOperation = new XmlOperation();

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

        #region Neative
        [TestMethod]
        [ExpectedException(typeof(NonSupportedVehicleException))]
        public void TestNonSupportedVehicle()
        {
            DataOperation = new XmlOperation();
            File.Copy(@"InputFiles/FullParking.xml", DataOperation.XmlPath, true);
            DataOperation = new XmlOperation();

            var floors = TargetClass.GetAvailableFloors("truck");
            Assert.AreEqual(floors, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NonSupportedVehicleException))]
        public void TestCheckinInvalidVehicle()
        {
            DataOperation = new XmlOperation();
            File.Copy(@"InputFiles/FullParking.xml", DataOperation.XmlPath, true);
            DataOperation = new XmlOperation();

            var floors = TargetClass.GetAvailableFloors("truck");
            Assert.AreEqual(floors, null);
        }

        #endregion

        #region Exception

        [TestMethod]
        [ExpectedException(typeof(FileNotFoundException))]
        public void TestNonExistingFile()
        {
            try
            {
                DataOperation = new XmlOperation();
                File.Delete(DataOperation.XmlPath);
                DataOperation = new XmlOperation();
            }
            finally
            {
                File.Copy(@"InputFiles/ValidFile.xml", DataOperation.XmlPath, true);
            }
            DataOperation = new XmlOperation();
            
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidFloorException))]
        public void TestInvalidFloor()
        {
            DataOperation = new XmlOperation();
            File.Copy(@"InputFiles/ValidFile.xml", DataOperation.XmlPath, true);
            DataOperation = new XmlOperation();
            var floors = TargetClass.Checkout("car", "InvalidFloor");
            Assert.AreEqual(floors, null);
        }

        #endregion
    }
}
