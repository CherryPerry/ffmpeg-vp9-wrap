using System.Collections.Generic;
using System.Linq;
using Vp9Encode.Args;

namespace Vp9Encode.Groups
{
  public class TimingGroup : ArgGroup
  {
    public override System.Type[] AfterGroup { get { return null; } }

    public override void ApplyGroupRule(ICollection<Arg> args)
    {
      // Recalculate -ss -to -> -ss -t
      StartTimeArg ss = args.FirstOrDefault(x => x is StartTimeArg) as StartTimeArg;
      EndTimeArg to = args.FirstOrDefault(x => x is EndTimeArg) as EndTimeArg;
      LengthTimeArg length = new LengthTimeArg((to.Time - ss.Time).ToString(TimeArg.TIME_FORMAT));
      args.Remove(to);
      args.Add(length);
    }
  }
}
