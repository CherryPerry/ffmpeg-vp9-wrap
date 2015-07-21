using System;

namespace VpxEncode.Output
{
  public sealed class SimpleProcessor : OutputProcessor
  {
    protected override void pu_OnDataReceived(object sender, ProcessingUnitDataReceivedEventArgs e)
    {
      if (e.Data?.Length == Console.WindowWidth)
        Console.Write(e.Data);
      else
        Console.WriteLine(e.Data);
    }
  }
}
