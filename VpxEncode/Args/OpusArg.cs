using System.Text;

namespace Vp9Encode.Args
{
  internal class OpusArg : Arg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    public OpusArg(string value)
      : base(value)
    {
      int result;
      ValidState = int.TryParse(Value, out result);
      Check = "-c:a opus -b:a ";
      Format = Check + "{0}K";
    }

    public override StringBuilder ApplyArg(StringBuilder args, ApplyTo to)
    {
      if (to == ApplyTo.Audio && CheckArgs(args))
        return args.Append(' ').AppendFormat(Format, Value).Append(' ');
      return args;
    }
  }
}
