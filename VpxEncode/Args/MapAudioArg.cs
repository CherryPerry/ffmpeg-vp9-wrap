using FfmpegEncode;
using System;
using System.Text;

namespace Vp9Encode.Args
{
  internal class MapAudioArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    public MapAudioArg(string value)
      : base(value)
    {
      int result;
      ValidState = int.TryParse(Value, out result);
      Check = "-map 0:";
      Format = Check + "{0}";
    }

    public override StringBuilder ApplyArg(StringBuilder args, ApplyTo to)
    {
      if (to == ApplyTo.Audio && CheckArgs(args))
        return args.Append(' ').AppendFormat(Format, Value).Append(' ');
      return args;
    }
  }
}
