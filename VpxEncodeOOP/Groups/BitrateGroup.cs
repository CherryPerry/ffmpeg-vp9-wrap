using System;
using System.Collections.Generic;
using System.Linq;
using Vp9Encode.Args;

namespace Vp9Encode.Groups
{
  public class BitrateGroup : ArgGroup
  {
    public override Type[] AfterGroup { get { return new Type[] { typeof(TimingGroup) }; } }

    public override void ApplyGroupRule(ICollection<Arg> args)
    {
      if (args.FirstOrDefault(x => x is CodecBitrateArg) == null)
      {
        LimitArg limit = args.FirstOrDefault(x => x is LimitArg) as LimitArg;
        LengthTimeArg length = args.FirstOrDefault(x => x is LengthTimeArg) as LengthTimeArg;
        InternalStateArg state = args.FirstOrDefault(x => x is InternalStateArg) as InternalStateArg;
        uint bitrate = (uint)((limit.IntValue - (int)state.Get("AudioSize")) * 8 * 0.95 / length.Time.TotalSeconds);
        CodecBitrateArg cba = new CodecBitrateArg(bitrate);
        args.Remove(limit);
        args.Add(cba);
      }
    }
  }
}
