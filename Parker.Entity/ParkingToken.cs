using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parker.Entity
{
    [Serializable]
    public class ParkingToken : Vehicle
    {
        public int ParkingNumber { get; set; }

        public DateTime CheckinTime { get; set; }
    }
}
