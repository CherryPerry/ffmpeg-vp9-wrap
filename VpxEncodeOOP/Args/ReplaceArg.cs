using System.Text;

namespace Vp9Encode.Args
{
  public class ReplaceArg : Arg
  {
    public override int Priority { get { return PRIORITY_HIGH - 2; } }

    public ReplaceArg() : base("_NO_VALUE_") { }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to) { return args.Append(" -y "); }
  }
}
