using System;
using System.Text;
using System.Text.RegularExpressions;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-scale")]
  public class ScaleArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    public ScaleArg(string value)
      : base(value)
    {
      Regex regex = new Regex(@"(no|-1:\d+|\d+:-1|\d+:\d+)");
      if (!regex.IsMatch(value))
        throw new ArgumentException();
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      if (to == ApplyTo.Video && Value != "no") {
        if (args.Length != 0)
          args.Append(',');
        args.AppendFormat("scale={0}", Value);
      }
      return args;
    }
  }
}
