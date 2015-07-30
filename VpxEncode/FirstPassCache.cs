using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VpxEncode
{
  internal class FirstPassCache
  {
    public struct FPCKey
    {
      public string Key { get; private set; }
      public FPCKey(string file, string vf, string start, string t)
      {
        Key = file + vf + start + t;
      }

      public override int GetHashCode()
      {
        return Key.GetHashCode();
      }

      public override bool Equals(object obj)
      {
        if (obj == null) return false;
        return GetHashCode().Equals(obj.GetHashCode());
      }
    }

    private static FirstPassCache cache = new FirstPassCache();
    public static FirstPassCache Instance { get { return cache; } }

    private ConcurrentDictionary<FPCKey, byte[]> Map = new ConcurrentDictionary<FPCKey, byte[]>();

    private FirstPassCache() { }

    public void Put(FPCKey key, byte[] value) { Map[key] = value; }

    public byte[] Get(FPCKey key)
    {
      if (Map.ContainsKey(key))
        return Map[key];
      return null;
    }

    public bool CreateIfPossible(FPCKey key, string fullFilePath)
    {
      if (!Map.ContainsKey(key))
        return false;
      byte[] arr = Map[key];
      using (FileStream fs = File.Create(fullFilePath))
        fs.Write(arr, 0, arr.Length);
      return true;
    }

    public void Save(FPCKey key, string fullFilePath)
    {
      Map[key] = File.ReadAllBytes(fullFilePath);
    }
  }
}
