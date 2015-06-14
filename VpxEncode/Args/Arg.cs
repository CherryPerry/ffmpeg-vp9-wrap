using System.Text;
using FfmpegEncode;
using System;

namespace Vp9Encode.Args
{
  public abstract class Arg
  {
    public enum ApplyTo { Audio, Video }
    public const int PRIORITY_HIGH = 1, PRIORITY_MEDUIM = 10, PRIORITY_LOW = 100;

    public abstract int Priority { get; }

    public string Value { get; private set; }

    protected bool ValidState { get; set; }

    protected string Check = "", Format = "";

    public abstract StringBuilder ApplyArg(StringBuilder args, ApplyTo to);

    public Arg(string value)
    {
      Value = value;
    }

    protected virtual bool CheckArgs(StringBuilder args)
    {
      if (args.Contains(Check))
      {
        Console.WriteLine("{0} is already applied", GetType());
        return false;
      }
      if (!ValidState)
      {
        Console.WriteLine("{0} invalid arg: {1}", GetType(), Value);
        return false;
      }
      return true;
    }
  }
}
