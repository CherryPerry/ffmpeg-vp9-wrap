using System;
using System.Text;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-af")]
  internal class AudioFileArg : Arg
  {
    public override int Priority { get { return PRIORITY_HIGH + 1; } }

    public AudioFileArg(string value)
      : base(value)
    {
      if (String.IsNullOrWhiteSpace(value))
        throw new ArgumentException();
    }

    public override StringBuilder ApplyArg(StringBuilder args, ApplyTo to)
    {
      if (to == ApplyTo.Audio)
        return args.AppendFormat(" -i \"{0}\" ", Value);
      return args;
    }
  }
}
