namespace Vp9Encode.Args
{
  internal class StartTimeArg : TimeArg
  {
    public override int TimeArgPriority { get { return PRIORITY_HIGH; } }

    public StartTimeArg(string value)
      : base(value)
    {
      Check = "-ss ";
      Format = Check + "{0}";
    }
  }
}
