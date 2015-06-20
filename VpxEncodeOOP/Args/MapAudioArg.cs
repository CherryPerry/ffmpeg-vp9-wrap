using System.Text;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-ma")]
  internal class MapAudioArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    public MapAudioArg(string value)
      : base(value)
    {
      byte.Parse(Value);
    }

    public override StringBuilder ApplyArg(StringBuilder args, ApplyTo to)
    {
      if (to == ApplyTo.Audio)
        return args.AppendFormat(" -map 0:{0} ", Value);
      return args;
    }
  }
}
