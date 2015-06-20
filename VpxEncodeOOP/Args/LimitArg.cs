using System.Text;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-limit")]
  public class LimitArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    public uint IntValue { get; private set; }

    public LimitArg(uint limit)
      : base(limit.ToString())
    {
      IntValue = limit;
    }

    public LimitArg(string limit)
      : base(limit)
    {
      IntValue = uint.Parse(limit);
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      return args;
    }
  }
}
