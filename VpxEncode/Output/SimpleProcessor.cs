using System;

namespace VpxEncode.Output
{
  public sealed class SimpleProcessor : OutputProcessor
  {
    protected override void pu_OnDataReceived(object sender, ProcessingUnitDataReceivedEventArgs e)
    {
      string data = e.Data;
      if (data != null && data.Length == Console.WindowWidth)
        Console.Write(data);
      else
        Console.WriteLine(data);
    }
  }
}
