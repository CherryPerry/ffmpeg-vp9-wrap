using System;
using System.Globalization;
using System.Text;

namespace Vp9Encode.Args
{
  internal abstract class TimeArg : Arg
  {
    public const string TIME_FORMAT = "hh\\:mm\\:ss\\.fff";

    public abstract int TimeArgPriority { get; }
    public override int Priority { get { return TimeArgPriority; } }
    public TimeSpan Time { get; set; }

    public TimeArg(string value)
      : base(value) 
    {
      try
      {
        Time = ParseToTimespan(Value);
        ValidState = true;
      }
      catch { ValidState = false; }
    }

    internal static TimeSpan ParseToTimespan(string str)
    {
      try { return TimeSpan.ParseExact(str, "hh\\:mm\\:ss\\.fff", CultureInfo.InvariantCulture); }
      catch (FormatException)
      {
        try { return TimeSpan.ParseExact(str, "mm\\:ss\\.fff", CultureInfo.InvariantCulture); }
        catch (FormatException) { return TimeSpan.FromSeconds(Double.Parse(str, new CultureInfo("en"))); }
      }
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      if (CheckArgs(args))
        return args.Append(' ').AppendFormat(Format, Value).Append(' ');
      return args;
    }

    public string TimeToString() { return Time.ToString(TIME_FORMAT); }
  }
}
