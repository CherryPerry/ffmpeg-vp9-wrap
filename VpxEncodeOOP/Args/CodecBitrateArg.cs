using System.Text;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-br")]
  public class CodecBitrateArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    public CodecBitrateArg(uint bitrate)
      : base(bitrate.ToString()) { }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      if (to == ApplyTo.Video)
        args.AppendFormat("-c:v vp9 -b:v {0}K", Value);
      return args;
    }
  }
}
