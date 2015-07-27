using System.Collections.Generic;

namespace VpxEncode
{
  internal sealed class Cache
  {
    private static Cache cache = new Cache();
    public static Cache Instance { get { return cache; } }

    private Dictionary<string, string> Map = new Dictionary<string, string>();

    private Cache() { }

    public string this[string key] { get { return Get(key); } }

    public void Put(string key, string value) { Map[key] = value; }

    public string Get(string key)
    {
      if (Map.ContainsKey(key))
        return Map[key];
      return null;
    }
  }
}
