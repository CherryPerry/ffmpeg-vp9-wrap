using System;
using System.Text;

namespace Vp9Encode.Args
{
  internal class AudioFileArg : Arg
  {
    public override int Priority { get { return PRIORITY_HIGH + 1; } }

    public AudioFileArg(string value)
      : base(value)
    {
      ValidState = !String.IsNullOrWhiteSpace(value);
      Check = "-i ";
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
