using System;
using System.Collections.Generic;
using Vp9Encode.Args;

namespace Vp9Encode.Groups
{
  public abstract class ArgGroup
  {
    public abstract Type[] AfterGroup { get; }
    public abstract void ApplyGroupRule(ICollection<Arg> args);
  }
}
