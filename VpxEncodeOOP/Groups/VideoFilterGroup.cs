using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Vp9Encode.Args;

namespace Vp9Encode.Groups
{
  public class VideoFilterGroup : ArgGroup
  {
    public override Type[] AfterGroup { get { return null; } }

    public override void ApplyGroupRule(ICollection<Arg> args)
    {
      StringBuilder sb = new StringBuilder();
      foreach (Arg a in args.Where(x => x is SubtitlesArg || x is ScaleArg).ToList())
      {
        if (a is IDependentArg)
          (a as IDependentArg).SetDependency(args);
        a.ApplyArg(sb, Arg.ApplyTo.Video);
        args.Remove(a);
      }
      if (sb.Length > 0)
      {
        VideoFilterArg arg = new VideoFilterArg(sb.ToString());
        args.Add(arg);
      }
    }
  }
}
