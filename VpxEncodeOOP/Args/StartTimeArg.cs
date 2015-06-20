using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-ss")]
  internal class StartTimeArg : TimeArg
  {
    public override int TimeArgPriority { get { return PRIORITY_HIGH; } }

    public StartTimeArg(string value)
      : base(value) { Format = "-ss {0}"; }
  }
}
