using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest
{
  [TestClass]
  public class LinearBitrateLookupTest
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
    public void TestValid()
    {
      // Kylobytes array
      int[] values = ProduceCorrectData();

      // Program.BitrateLookupEncode
    }

    [TestMethod]
    public void TestNotFound()
    {
      // Kylobytes array
      int[] values = ProduceNotFoundData();

      // Program.BitrateLookupEncode
    }
  }
}
