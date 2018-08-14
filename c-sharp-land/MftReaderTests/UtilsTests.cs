using Microsoft.VisualStudio.TestTools.UnitTesting;
using MftReader.tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MftReader.tests.Tests
{
    [TestClass()]
    public class UtilsTests
    {
        [TestMethod()]
        public void extractExtensionTest()
        {

            String[] fileNameArray  = new String[5];
            String[] fileExtArray   = new String[5];

            fileNameArray[0] = "test.txt";
            fileNameArray[1] = "test..txt";
            fileNameArray[2] = "test.jpeg";
            fileNameArray[3] = "te.s.t.jpeg";
            fileNameArray[4] = "test";

            fileExtArray[0] = ".txt";
            fileExtArray[1] = ".txt";
            fileExtArray[2] = ".jpeg";
            fileExtArray[3] = ".jpeg";
            fileExtArray[4] = null;

            for (int i = 0; i < 5; i++)
            {
                
                String extension = Utils.Instance.ExtractExtension(fileNameArray[i]);             
                Assert.AreEqual(fileExtArray[i], extension);

            }


        }
    }
}