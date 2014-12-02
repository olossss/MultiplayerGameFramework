using MultiplayerFramework;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnitTests
{
    class Helpers
    {
        public static void Sleep(int seconds = 1)
        {
            Thread.Sleep(seconds * 1000);
        }

        public static void CheckValue(int Actual, int Expected)
        {
            if (Expected != Actual)
                throw new ServerException("Expecting: " + Expected + " Actual: " + Actual);
                //Assert("Expecting: "+Expected+" Actual: " + Actual);
        }
    }
}
