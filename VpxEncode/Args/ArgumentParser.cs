using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vp9Encode.Args
{
  public class ArgumentParser
  {
    SortedDictionary<string, Type> inputArgs = new SortedDictionary<string, Type>()
    {
      { "-ma", typeof(MapAudioArg) },
      { "-ss", typeof(StartTimeArg) },
      { "-to", typeof(EndTimeArg) },
      { "-or", typeof(OpusArg) },
      { "-af", typeof(AudioFileArg) }
    };

    List<Arg> applicableArgs = new List<Arg>();

    public void Parse(string[] args)
    {
      bool waitForParam = false;
      Type last = null;

      for (int i = 0; i < args.Length; i++)
      {
        if (inputArgs.ContainsKey(args[i]))
        {
          // No parameters at last arg
          if (last != null)
            applicableArgs.Add((Arg)Activator.CreateInstance(last, null));

          waitForParam = true;
          last = inputArgs[args[i]];
        }
        else if (last != null && waitForParam)
        {
          applicableArgs.Add((Arg)Activator.CreateInstance(last, args[i]));
          last = null;
          waitForParam = false;
        }
      }

      AddDefaults();
    }

    private void AddDefaults()
    {
      // If no params, add some defualts.
      StartTimeArg ss = applicableArgs.FirstOrDefault(x => x is StartTimeArg) as StartTimeArg;
      EndTimeArg to = applicableArgs.FirstOrDefault(x => x is EndTimeArg) as EndTimeArg;
      if (ss == null) { }
      if (to == null) { }

      // Recalculate -ss -to -> -ss -t
      LengthTimeArg length = new LengthTimeArg((to.Time - ss.Time).ToString(TimeArg.TIME_FORMAT));
      applicableArgs.Remove(to);
      applicableArgs.Add(length);
    }

    public void Apply(StringBuilder sb, Arg.ApplyTo applyTo)
    {
      foreach (Arg a in applicableArgs.OrderBy(x => x.Priority))
        a.ApplyArg(sb, applyTo);
    }
  }
}
