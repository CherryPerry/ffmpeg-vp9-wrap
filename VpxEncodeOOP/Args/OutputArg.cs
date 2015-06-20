using System;
using System.Text;

namespace Vp9Encode.Args
{
  public class OutputArg : Arg
  {
    public override int Priority { get { return PRIORITY_LOW; } }

    public OutputArg(string value) : base(value)
    {
      if (String.IsNullOrWhiteSpace(value))
        throw new ArgumentException();
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      return args.Append(' ').Append(Value);
    }
  }
}
