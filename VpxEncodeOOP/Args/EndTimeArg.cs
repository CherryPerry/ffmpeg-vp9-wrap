using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-to")]
  internal class EndTimeArg : TimeArg
  {
    public override int TimeArgPriority { get { return PRIORITY_MEDUIM; } }

    public EndTimeArg(string value)
      : base(value) { Format = "-to {0}"; }
  }
}
