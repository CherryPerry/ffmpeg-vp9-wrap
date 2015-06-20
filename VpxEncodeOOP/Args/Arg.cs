using System.Text;

namespace Vp9Encode.Args
{
  public abstract class Arg
  {
    public enum ApplyTo { Audio, Video }
    public const int PRIORITY_HIGH = 10, PRIORITY_MEDUIM = 100, PRIORITY_LOW = 1000;

    public abstract int Priority { get; }

    public string Value { get; protected set; }

    public abstract StringBuilder ApplyArg(StringBuilder args, ApplyTo to);

    public Arg(string value)
    {
      Value = value;
    }
  }
}
