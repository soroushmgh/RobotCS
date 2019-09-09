using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RobotCS;

namespace RobotCsTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestFunctions()
        {
            MyRobot myRobot = new MyRobot();

            myRobot.ProcessRequest("PlaCE 0,0,NOrtH");
            myRobot.ProcessRequest("    MovE");
            Assert.AreEqual(myRobot.ProcessReportReq(), "Output: 0,1,NORTH");

            myRobot.ProcessRequest("    PLACE 0,0,NORTH");
            myRobot.ProcessRequest("left   ");
            Assert.AreEqual(myRobot.ProcessReportReq(), "Output: 0,0,WEST");

            myRobot.ProcessRequest("PLACE 1,2,EAST    ");
            myRobot.ProcessRequest("MOVE");
            myRobot.ProcessRequest("MOVE");
            myRobot.ProcessRequest("LEFT");
            myRobot.ProcessRequest("MOVE");
            Assert.AreEqual(myRobot.ProcessReportReq(), "Output: 3,3,NORTH");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestExceptions1()
        {
            MyRobot myRobot = new MyRobot();

            myRobot.ProcessRequest("MOVE");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestExceptions2()
        {
            MyRobot myRobot = new MyRobot();

            myRobot.ProcessRequest("LEFT");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestExceptions3()
        {
            MyRobot myRobot = new MyRobot();

            myRobot.ProcessRequest("RIGHT");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestExceptions4()
        {
            MyRobot myRobot = new MyRobot();

            myRobot.ProcessRequest("REPORT");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void TestExceptions5()
        {
            MyRobot myRobot = new MyRobot();

            myRobot.ProcessRequest("PLACE      1   ,   2   ,    EAST");
        }
    }
}
