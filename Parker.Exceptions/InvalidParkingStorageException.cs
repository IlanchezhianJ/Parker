using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parker.Exceptions
{
    public class InvalidParkingStorageException : Exception
    {
        public InvalidParkingStorageException() 
            : base(string.Format("Invalid or missing Xml storage file.")) 
        { 
        }
    }
}
