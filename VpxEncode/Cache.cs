using System.Collections.Concurrent;
using System.IO;

namespace VpxEncode
{
  internal sealed class Cache
  {
    public const int CACHE_AUDIO = 1, CACHE_FIRSTPASS = 2, CACHE_STRINGS = 3;

    public struct FPCKey
    {
      public string Key { get; private set; }
      public FPCKey(string file, string vf, string start, string t) { Key = file + vf + start + t; }
      public override int GetHashCode() { return Key.GetHashCode(); }
      public override bool Equals(object obj) { if (obj == null) return false; return GetHashCode().Equals(obj.GetHashCode()); }
    }

    public struct ACKey
    {
      public string Key { get; private set; }
      public ACKey(string file, string af, string start, string t) { Key = file + af + start + t; }
      public ACKey(string args) { Key = args; }
      public override int GetHashCode() { return Key.GetHashCode(); }
      public override bool Equals(object obj) { if (obj == null) return false; return GetHashCode().Equals(obj.GetHashCode()); }
    }

    private static Cache cache = new Cache();
    public static Cache Instance { get { return cache; } }

    private ConcurrentDictionary<int, ConcurrentDictionary<object, object>> Map = new ConcurrentDictionary<int, ConcurrentDictionary<object, object>>();

    private Cache() { }

    public void Put(int mode, object key, object value) { Map[mode][key] = value; }

    public T Get<T>(int mode, object key)
    {
      if (Map[mode].ContainsKey(key))
        return (T)Map[mode][key];
      return default(T);
    }

    public bool CreateIfPossible(ACKey key, string fullFilePath)
    {
      if (!Map[CACHE_AUDIO].ContainsKey(key))
        return false;
      byte[] arr = Get<byte[]>(CACHE_AUDIO, key);
      WriteFile(arr, fullFilePath);
      return true;
    }

    public bool CreateIfPossible(FPCKey key, string fullFilePath)
    {
      if (!Map[CACHE_FIRSTPASS].ContainsKey(key))
        return false;
      byte[] arr = Get<byte[]>(CACHE_FIRSTPASS, key);
      WriteFile(arr, fullFilePath);
      return true;
    }

    private void WriteFile(byte[] bytes, string path)
    {
      using (FileStream fs = File.Create(path))
        fs.Write(bytes, 0, bytes.Length);
    }

    public void Save(FPCKey key, string fullFilePath)
    {
      Map[CACHE_FIRSTPASS][key] = File.ReadAllBytes(fullFilePath);
    }

    public void Save(ACKey key, string fullFilePath)
    {
      Map[CACHE_AUDIO][key] = File.ReadAllBytes(fullFilePath);
    }
  }
}
