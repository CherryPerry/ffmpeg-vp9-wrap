using System;
using System.Text;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-file")]
  public class VideoFileArg : Arg
  {
    public override int Priority { get { return PRIORITY_HIGH + 1; } }

    public VideoFileArg(string value)
      : base(value)
    {
      if (String.IsNullOrWhiteSpace(value))
        throw new ArgumentException();
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      if (to == ApplyTo.Video)
        args.AppendFormat(" -i \"{0}\" ", Value);
      return args;
    }
  }
}
