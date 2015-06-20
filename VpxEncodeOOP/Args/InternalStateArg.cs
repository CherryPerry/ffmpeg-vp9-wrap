using System.Text;
using System.Collections.Generic;

namespace Vp9Encode.Args
{
  public class InternalStateArg : Arg
  {
    public override int Priority { get { return int.MaxValue; } }

    private Dictionary<string, object> Values = new Dictionary<string,object>();

    public InternalStateArg() : base("_NO_VALUE_") { }

    public InternalStateArg(params object[] vals) : this() 
    {
      string name = null;
      for (int i = 0; i < vals.Length; i++)
      {
        if ((i & 0x1) == 1)
          Values[name] = vals[i];
        else
          name = vals[i].ToString();
      }
    }

    public void Set(string name, object value)
    {
      Values[name] = value;
    }

    public object Get(string name)
    {
      return Values[name];
    }

    public IReadOnlyDictionary<string, object> GetValues()
    {
      return Values;
    }

    public override StringBuilder ApplyArg(StringBuilder args, Arg.ApplyTo to)
    {
      // Does not apply, used to store data
      return args;
    }
  }
}
