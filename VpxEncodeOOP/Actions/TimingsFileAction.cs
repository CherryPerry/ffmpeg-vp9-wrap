using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Vp9Encode.Args;

namespace Vp9Encode.Actions
{
  public class TimingsFileAction : Action
  {
    private string t, ti;

    public override bool TryAction(string[] args)
    {
      t = Reflections.GetInputSequence(typeof(TimingFileArg));
      ti = Reflections.GetInputSequence(typeof(TimingIndexArg));

      if (args.Contains(t))
      {
        ArgumentParser p = new ArgumentParser();
        IReadOnlyCollection<Arg> a = p.Parse(args, new Arg[] { });
        TimingFileArg tfa = a.First(x => x is TimingFileArg) as TimingFileArg;
        TimingIndexArg tia = a.FirstOrDefault(x => x is TimingIndexArg) as TimingIndexArg;

        string[] lines = File.ReadAllLines(tfa.Value);

        if (tia != null)
        {
          ushort index = ushort.Parse(tia.Value);
          if (index < lines.Length)
          {
            Process(args, lines[index]);
          }
        }
        else
        {
          Parallel.ForEach(lines, new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount }, (val) =>
          {
            Process(args, val);
          });
        }
      }

      return false;
    }

    private void Process(string[] args, string line)
    {
      string[] split = line.Split(' ');
      List<string> list = new List<string>(args);
      int i1 = list.IndexOf(t), i2 = list.IndexOf(ti);
      list.RemoveAt(i1);
      list.RemoveAt(i1 + 1);
      if (i2 > 0)
      {
        list.RemoveAt(i2);
        list.RemoveAt(i2 + 1);
      }
      list.Add(Reflections.GetInputSequence(typeof(StartTimeArg)));
      list.Add(split[0]);
      list.Add(Reflections.GetInputSequence(typeof(EndTimeArg)));
      list.Add(split[1]);
      Actions.SelectAction(list.ToArray());
    }
  }
}
