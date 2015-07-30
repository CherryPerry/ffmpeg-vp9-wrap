using System.Collections.Concurrent;

namespace VpxEncode
{
  internal sealed class Cache
  {
    private static Cache cache = new Cache();
    public static Cache Instance { get { return cache; } }

    private ConcurrentDictionary<string, object> Map = new ConcurrentDictionary<string, object>();

    private Cache() { }

    public void Put(string key, object value) { Map[key] = value; }

    public string Get(string key)
    {
      if (Map.ContainsKey(key))
        return Map[key] as string;
      return null;
    }

    public byte[] GetBytes(string key)
    {
      if (Map.ContainsKey(key))
        return Map[key] as byte[];
      return null;
    }
  }
}
