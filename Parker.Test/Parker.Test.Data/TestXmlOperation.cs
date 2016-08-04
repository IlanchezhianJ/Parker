using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Parker.Data;

namespace Parker.Test.Parker.Test.Data
{
    [TestFixture]
    public class TestXmlOperation
    {
        XmlOperation TargetClass;

        [OneTimeSetUp]
        public void Initialize()
        {
            TargetClass = new XmlOperation();
        }
    }
}
