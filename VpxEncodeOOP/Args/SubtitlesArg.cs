using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vp9Encode.Args.Attributes;

namespace Vp9Encode.Args
{
  [InputSequence("-subs")]
  public class SubtitlesArg : Arg, IDependentArg
  {
    public override int Priority { get { return PRIORITY_MEDUIM; } }

    private ICollection<Arg> Other;

    public SubtitlesArg(string value)
      : base(value)
    {
      if (String.IsNullOrWhiteSpace(value))
        throw new ArgumentException();
      Value = Value.Replace("[", "\\[").Replace("]", "\\]");
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      if (Other == null)
        throw new ArgumentException("IDependentArg SetDependency not called");

      if (to == ApplyTo.Video)
      {
        LengthTimeArg length = Other.FirstOrDefault(x => x is LengthTimeArg) as LengthTimeArg;

        if (args.Length != 0)
          args.Append(',');
        string type = Value.EndsWith("ass") || Value.EndsWith("ssa") ? "ass" : "subtitles";
        // 0 - time, 1 - ass,subtitles, 2 - path
        args.AppendFormat("setpts=PTS+{0:0.######}/TB,{1}=\"{2}\",setpts=PTS-STARTPTS", length.Time.TotalSeconds, type, Value);
      }
      return args;
    }

    public void SetDependency(ICollection<Arg> args)
    {
      Other = args;
    }
  }
}
