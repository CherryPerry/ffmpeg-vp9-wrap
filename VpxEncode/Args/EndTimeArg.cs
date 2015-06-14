namespace Vp9Encode.Args
{
  internal class EndTimeArg : TimeArg
  {
    public override int TimeArgPriority { get { return PRIORITY_MEDUIM; } }

    public EndTimeArg(string value)
      : base(value) 
    {
      Check = "-to ";
      Format = Check + "{0}";
    }
  }
}
