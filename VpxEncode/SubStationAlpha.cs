using System;
using System.IO;
using System.Text;

namespace VpxEncode
{
  /// <summary>
  /// Class for replacing fonts in .ass to Arial
  /// </summary>
  class SubStationAlpha
  {
    const string FONTNAME = "Arial";

    string[] Content;

    public static bool IsAcceptable(string path) => path.EndsWith(".ass");

    public SubStationAlpha(string path)
    {
      Content = File.ReadAllLines(path);
    }

    public void ChangeFontAndSave(string path)
    {
      ChangeFont();
      Save(path);
    }

    void ChangeFont()
    {
      bool styleStart = false;
      int fontIndex = -1;
      for (int i = 0; i < Content.Length; i++)
      {
        string line = Content[i];
        if (line.StartsWith("[V")) { styleStart = true; continue; }
        if (styleStart && line.StartsWith("Format: "))
        {
          string[] split = line.Split(',');
          for (int j = 1; j < split.Length; j++)
          {
            string s = split[j].Trim();
            if (s == "Fontname")
              fontIndex = j;
          }
          continue;
        }
        if (fontIndex != -1 && line.StartsWith("Style"))
        {
          string[] split = line.Split(',');
          if (fontIndex < split.Length)
          {
            split[fontIndex] = FONTNAME;
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < split.Length; k++)
            {
              sb.Append(split[k]);
              if (k != split.Length - 1)
                sb.Append(',');
            }
            Console.WriteLine($"{Content[i]} -> {sb.ToString()}");
            Content[i] = sb.ToString();
          }
          continue;
        }
        if (line.StartsWith("[Events]"))
          break;
      }
    }

    void Save(string path) => File.WriteAllLines(path, Content);
  }
}
