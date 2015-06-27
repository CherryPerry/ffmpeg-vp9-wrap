using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace VpxEncode.Output
{
  public sealed class PercentedProcessor : OutputProcessor
  {
    private Regex TimeFromLine = new Regex(@".*time=(\d{2,}:\d{2}:\d{2}.\d{2}).*");

    private double TotalSeconds;

    public PercentedProcessor(double totalSeconds)
    {
      TotalSeconds = totalSeconds;
    }

    protected override void pu_OnDataReceived(object sender, ProcessingUnitDataReceivedEventArgs e)
    {
      if (e.Data == null)
        return;

      Match match = TimeFromLine.Match(e.Data);
      if (match.Success)
      {
        string value = match.Groups[1].Value;
        double d = TimeSpan.ParseExact(value, "hh\\:mm\\:ss\\.ff", CultureInfo.InvariantCulture).TotalSeconds;
        double perc = 100 * d / TotalSeconds;

        ProcessingUnit pu = sender as ProcessingUnit;
        Console.SetCursorPosition(0, units.IndexOf(pu));
        string str = String.Format("{0:0.00}%", perc);
        Console.WriteLine(str);
      }
    }
  }
}
