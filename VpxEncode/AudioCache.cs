using System.Collections.Concurrent;
using System.IO;

namespace VpxEncode
{
  internal class AudioCache
  {
    public struct ACKey
    {
      public string Key { get; private set; }
      public ACKey(string file, string af, string start, string t) { Key = file + af + start + t; }
      public ACKey(string args) { Key = args; }
      public override int GetHashCode() { return Key.GetHashCode(); }
      public override bool Equals(object obj)
      {
        if (obj == null) return false;
        return GetHashCode().Equals(obj.GetHashCode());
      }
    }

    private static AudioCache cache = new AudioCache();
    public static AudioCache Instance { get { return cache; } }

    private ConcurrentDictionary<ACKey, byte[]> Map = new ConcurrentDictionary<ACKey, byte[]>();

    private AudioCache() { }

    public void Put(ACKey key, byte[] value) { Map[key] = value; }

    public byte[] Get(ACKey key)
    {
      if (Map.ContainsKey(key))
        return Map[key];
      return null;
    }

    public bool CreateIfPossible(ACKey key, string fullFilePath)
    {
      if (!Map.ContainsKey(key))
        return false;
      byte[] arr = Map[key];
      using (FileStream fs = File.Create(fullFilePath))
        fs.Write(arr, 0, arr.Length);
      return true;
    }

    public void Save(ACKey key, string fullFilePath)
    {
      Map[key] = File.ReadAllBytes(fullFilePath);
    }
  }
}
