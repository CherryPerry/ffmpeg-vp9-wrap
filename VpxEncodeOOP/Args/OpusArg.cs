using System.Text;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-or")]
  internal class OpusArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    public OpusArg(string value)
      : base(value)
    {
      ushort.Parse(Value);
    }

    public override StringBuilder ApplyArg(StringBuilder args, ApplyTo to)
    {
      if (to == ApplyTo.Audio)
        return args.AppendFormat(" -c:a opus -b:a {0}K ", Value);
      return args;
    }
  }
}
