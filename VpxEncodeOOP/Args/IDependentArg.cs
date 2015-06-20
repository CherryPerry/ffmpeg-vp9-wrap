using System.Collections.Generic;

namespace Vp9Encode.Args
{
  public interface IDependentArg
  {
    void SetDependency(ICollection<Arg> args);
  }
}
