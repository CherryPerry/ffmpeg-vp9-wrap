using System.Text;

namespace Vp9Encode.Args
{
  public class VideoFilterArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    public VideoFilterArg(string value)
      : base(value) { }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      if (to == ApplyTo.Video)
        args.AppendFormat(" -vf {0} ", Value);
      return args;
    }
  }
}
