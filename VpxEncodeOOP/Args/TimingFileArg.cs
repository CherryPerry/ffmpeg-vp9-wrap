using System;
using System.IO;
using System.Text;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-t")]
  public class TimingFileArg : Arg
  {
    public override int Priority
    {
      get { return PRIORITY_LOW; }
    }

    public TimingFileArg(string file)
      : base(file)
    {
      if (String.IsNullOrWhiteSpace(file))
        throw new ArgumentException();
      FileInfo fileInfo = new FileInfo(file);
      if (!fileInfo.Exists)
        throw new ArgumentException();
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      return args;
    }
  }
}
