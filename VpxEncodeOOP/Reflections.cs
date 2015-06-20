using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vp9Encode.Args.Attributes;
using System.Reflection;

namespace Vp9Encode
{
  public static class Reflections
  {
    public static string GetInputSequence(Type t)
    {
      InputSequenceAttribute attr = t.GetCustomAttribute<InputSequenceAttribute>();
      if (attr == null)
        return null;
      return attr.InputSequence;
    }
  }
}
