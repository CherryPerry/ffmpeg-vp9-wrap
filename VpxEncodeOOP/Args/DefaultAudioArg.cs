using System.Text;

namespace Vp9Encode.Args
{
  public class DefaultAudioArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    public DefaultAudioArg() : base("_DEFAULT_") { }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      if (to == ApplyTo.Audio)
        return args.Append(" -ac 2 -vn -sn ");
      return args;
    }
  }
}
