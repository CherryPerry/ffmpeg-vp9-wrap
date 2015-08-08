using Newtonsoft.Json.Linq;
using System.Linq;

namespace VpxEncode
{
  internal static class Ffprober
  {
    public struct Result
    {
      public string Scale;
      public int HeightPix, WidthPix;
      public string EndTime;
      public string Framerate;
    }

    public static Result Probe(string filePath)
    {
      string args = $"-v quiet -print_format json -show_streams -show_format -hide_banner \"{filePath}\"";
      string json = Cache.Instance.Get<string>(Cache.CACHE_STRINGS, args);
      if (json == null)
      {
        json = new Executer(Executer.FFPROBE).Execute(args);
        Cache.Instance.Put(Cache.CACHE_STRINGS, args, json);
      }
      try
      {
        JObject root = JObject.Parse(json);
        var videoStream = root["streams"].Where(x => x["codec_type"].ToString() == "video").First();
        return new Result()
        {
          EndTime = root["format"]["duration"].ToString(),
          WidthPix = int.Parse(videoStream["width"].ToString()),
          HeightPix = int.Parse(videoStream["height"].ToString()),
          Scale = $"{videoStream["width"]}:{videoStream["height"]}",
          Framerate = videoStream["r_frame_rate"].ToString()
        };
      }
      catch { return default(Result); }
    }
  }
}
