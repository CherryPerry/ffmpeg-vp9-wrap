using System;
using System.Collections.Generic;
using System.Linq;
using Vp9Encode.Args;

namespace Vp9Encode.Groups
{
  public class DefaultsGroup : ArgGroup
  {
    public override Type[] AfterGroup { get { return null; } }

    public override void ApplyGroupRule(ICollection<Arg> args)
    {
      InternalStateArg state = args.FirstOrDefault(x => x is InternalStateArg) as InternalStateArg;

      if (args.FirstOrDefault(x => x is DefaultAudioArg) == null)
        args.Add(new DefaultAudioArg());

      if (args.FirstOrDefault(x => x is ReplaceArg) == null)
        args.Add(new ReplaceArg());

      if (args.FirstOrDefault(x => x is OpusArg) == null)
        args.Add(new OpusArg("80"));

      if (args.FirstOrDefault(x => x is DefaultVideoArg) == null)
        args.Add(new DefaultVideoArg((int)state.Get("Pass"), (long)state.Get("Code")));

      if (args.FirstOrDefault(x => x is LimitArg) == null)
        args.Add(new LimitArg(10240));

      if (args.FirstOrDefault(x => x is ScaleArg) == null)
        args.Add(new ScaleArg("-1:540"));
    }
  }
}
