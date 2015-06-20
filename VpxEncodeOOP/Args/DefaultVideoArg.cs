using System.Text;

namespace Vp9Encode.Args
{
  public class DefaultVideoArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    private int Pass;
    private long Code;

    public DefaultVideoArg(int pass, long code)
      : base("_DEFAULT_")
    {
      Pass = pass;
      Code = code;
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      if (to == ApplyTo.Video)
        return args.Append(' ')
          .AppendFormat("-tile-columns 6 -frame-parallel 1 -speed {0} -threads 8 -an -sn -lag-in-frames 25 -auto-alt-ref 1 -quality good -pass {1} -passlogfile temp_{2}",
          Pass == 1 ? 4 : 1, Pass, Code)
          .Append(' ');
      return args;
    }
  }
}
