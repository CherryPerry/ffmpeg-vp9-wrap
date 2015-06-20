using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Vp9Encode.Actions
{
  public static class Actions
  {
    private static HashSet<Type> ActionTypes;

    static Actions()
    {
      Type groupType = typeof(Action);
      ActionTypes = new HashSet<Type>(
        Assembly.GetExecutingAssembly().DefinedTypes.Where(t => t != groupType
          && groupType.IsAssignableFrom(t)));
    }

    public static void SelectAction(string[] args)
    {
      foreach (Type t in ActionTypes)
      {
        Action a = Activator.CreateInstance(t) as Action;
        if (a != null)
          if (a.TryAction(args))
            break;
      }
    }
  }
}
