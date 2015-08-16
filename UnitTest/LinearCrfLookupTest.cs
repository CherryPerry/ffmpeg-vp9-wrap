using Microsoft.VisualStudio.TestTools.UnitTesting;
using VpxEncode;
using System.Collections.Generic;

namespace UnitTest
{
  [TestClass]
  public class LinearCrfLookupTest
  {
    public static int[] ProduceCorrectData()
    {
      int[] values = new int[63];
      for (int i = 0; i < 63; i++)
        values[i] = 1000 + (64 - i) * 300 + 100;
      return values;
    }

    public static int[] ProduceNotFoundData()
    {
      int[] values = new int[63];
      for (int i = 0; i < 63; i++)
        values[i] = 1000 + (64 - i) * 300 - 10;
      return values;
    }

    [TestMethod]
    public void TestMinCRF()
    {
      TestValid(4);
      TestNotFound(4);
    }

    [TestMethod]
    public void TestMaxCRF()
    {
      TestValid(62);
      TestNotFound(62);
    }

    [TestMethod]
    public void TestMultipleCRF()
    {
      ushort[] crf = new ushort[] { 10, 20, 30, 40, 50 };
      foreach (ushort c in crf)
      {
        TestValid(c);
        TestNotFound(c);
      }
    }

    private void TestValid(ushort startCrf)
    {
      // Kylobytes array
      int[] values = ProduceCorrectData();

      ushort resultCrf = Program.CrfLookupEncode(startCrf,
        (crf) =>
        {
          return crf.ToString();
        },
        (str) =>
        {
          ushort crf = ushort.Parse(str);
          return values[crf] / 1024d;
        });

      Assert.AreEqual(resultCrf, 34);
    }

    private void TestNotFound(ushort startCrf)
    {
      // Kylobytes array
      int[] values = ProduceNotFoundData();

      // Check if correct one was done
      HashSet<ushort> set = new HashSet<ushort>();

      ushort resultCrf = Program.CrfLookupEncode(startCrf,
        (crf) =>
        {
          set.Add(crf);
          return crf.ToString();
        },
        (str) =>
        {
          ushort crf = ushort.Parse(str);
          return values[crf] / 1024d;
        });

      Assert.AreEqual(resultCrf, 0);
      Assert.IsTrue(set.Contains(34));
    }
  }
}
