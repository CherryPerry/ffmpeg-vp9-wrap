using FfmpegEncode;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Vp9Encode.Args.Attributes;
using Vp9Encode.Groups;

namespace Vp9Encode.Args
{
  public class ArgumentParser
  {
    private SortedDictionary<string, Type> inputArgs = new SortedDictionary<string, Type>();
    private List<Arg> applicableArgs = new List<Arg>();

    public ArgumentParser()
    {
      Type groupType = typeof(Arg);
      HashSet<Type> types = new HashSet<Type>(
        Assembly.GetExecutingAssembly().DefinedTypes.Where(t => t != groupType
          && groupType.IsAssignableFrom(t)));
      foreach (Type t in types)
      {
        InputSequenceAttribute attr = t.GetCustomAttribute<InputSequenceAttribute>(false);
        if (attr != null)
        {
          string name = attr.InputSequence;
          if (name != null)
            inputArgs.Add(name, t);
        }
      }
    }

    public IReadOnlyList<Arg> Parse(string[] args, Arg[] argss)
    {
      applicableArgs.Clear();

      if (argss != null)
        applicableArgs.AddRange(argss);

      bool waitForParam = false;
      Type last = null;

      for (int i = 0; i < args.Length; i++)
      {
        if (inputArgs.ContainsKey(args[i]))
        {
          // No parameters at last arg
          if (last != null)
            try { applicableArgs.Add(Activator.CreateInstance(last, null) as Arg); }
            catch { waitForParam = false; continue; }

          waitForParam = true;
          last = inputArgs[args[i]];
        }
        else if (last != null && waitForParam)
        {
          try { applicableArgs.Add(Activator.CreateInstance(last, args[i]) as Arg); }
          catch { }
          last = null;
          waitForParam = false;
        }
      }

      ApplyGroups();

      return applicableArgs;
    }

    private void ApplyGroups()
    {
      Type groupType = typeof(ArgGroup);
      HashSet<Type> types = new HashSet<Type>(
        Assembly.GetExecutingAssembly().DefinedTypes.Where(t => t != groupType
          && groupType.IsAssignableFrom(t)));
      HashSet<Type> appliedTypes = new HashSet<Type>();
      IEnumerator<Type> numerator = types.GetEnumerator();
      while (numerator.MoveNext())
      {
        Type t = numerator.Current;
        ArgGroup ag = Activator.CreateInstance(t) as ArgGroup;
        Type[] after = ag.AfterGroup;
        if (after == null)
        {
          ag.ApplyGroupRule(applicableArgs);
          types.Remove(t);
          appliedTypes.Add(t);
          numerator.Dispose();
          numerator = types.GetEnumerator();
        }
        else
        {
          bool all = true;
          foreach (Type a in after)
            all &= appliedTypes.Contains(a);
          if (all)
          {
            ag.ApplyGroupRule(applicableArgs);
            types.Remove(t);
            appliedTypes.Add(t);
            numerator.Dispose();
            numerator = types.GetEnumerator();
          }
        }
      }
    }

    public void Apply(StringBuilder sb, Arg.ApplyTo applyTo)
    {
      foreach (Arg a in applicableArgs.OrderBy(x => x.Priority))
      {
        if (a is IDependentArg)
          (a as IDependentArg).SetDependency(applicableArgs);
        a.ApplyArg(sb, applyTo);
      }
      sb.TrimSpaces();
    }
  }
}
