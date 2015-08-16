using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using VpxEncode;

namespace UnitTest
{
  [TestClass]
  public class LinearCrfLookupTest
  {
    [TestMethod]
    public void TestValid()
    {
      // kb sizes
      int[] values = new int[63];
      for (int i = 0; i < 63; i++)
        values[i] = 1000 + (64 - i) * 300 + 100;

      // test scope

      double limit = 10;
      double delta = 240 / 1024d;
      ushort startCrf = 10;
      LinearCrfLookup bl = new LinearCrfLookup(limit - delta / 2, startCrf); // in mb
      ushort newCrf = 0;

      double size = 0;
      while (!(limit - size < delta && size < limit))
      {
        newCrf = bl.GetTarget();
        if (newCrf == 0)
          break;
        size = values[newCrf] / 1024d; // in mb
        bl.AddPoint(newCrf, size);
      }

      Assert.AreEqual(newCrf, 34);
    }

    [TestMethod]
    public void TestNotFound()
    {
      // kb sizes
      int[] values = new int[63];
      for (int i = 0; i < 63; i++)
        values[i] = 1000 + (64 - i) * 300 - 10;

      // test scope

      double limit = 10;
      double delta = 240 / 10240d;
      ushort startCrf = 10;
      LinearCrfLookup bl = new LinearCrfLookup(limit - delta / 2, startCrf); // in mb
      ushort newCrf = 0;

      double size = 0;
      while (!(limit - size < delta && size < limit))
      {
        newCrf = bl.GetTarget();
        if (newCrf == 0)
          break;
        size = values[newCrf] / 1024d; // in mb
        bl.AddPoint(newCrf, size);
      }

      Assert.AreEqual(newCrf, 0);
    }
  }
}
