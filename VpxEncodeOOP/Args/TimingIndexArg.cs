using System.Text;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-ti")]
  public class TimingIndexArg : Arg
  {
    public override int Priority
    {
      get { return PRIORITY_LOW; }
    }

    public TimingIndexArg(string index)
      : base(index)
    {
      ushort.Parse(index);
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      return args;
    }
  }
}
