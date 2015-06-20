namespace Vp9Encode.Args
{
  internal class LengthTimeArg : TimeArg
  {
    public override int TimeArgPriority { get { return PRIORITY_MEDUIM; } }

    public LengthTimeArg(string value)
      : base(value) { Format = "-t {0}"; }
  }
}
