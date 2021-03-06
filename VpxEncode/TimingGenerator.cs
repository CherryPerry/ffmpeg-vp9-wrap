﻿using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace VpxEncode
{
  /// <summary>
  /// Generates timings from meta by parsing ffprobe result with regexp
  /// </summary>
  class TimingGenerator
  {
    public string File { get; private set; }
    public string[] Timings { get; set; }

    private string output;
    private string OutFile;

    public TimingGenerator(string file, string output)
    {
      File = file;
      if (output == null)
        OutFile = File + "-timing.txt";
      else
        OutFile = output;
    }

    public void Generate(bool toFile)
    {
      output = new Executer().Execute($"-hide_banner \"{File}\"");
      GetTimings(toFile);
    }

    void GetTimings(bool toFile)
    {
      Regex reg = new Regex(@"Chapter #\d+:\d+: start (\d+.\d+), end (\d+.\d+)");
      MatchCollection res = reg.Matches(output);
      StringBuilder sb = new StringBuilder();
      LinkedList<string> asList = new LinkedList<string>();
      for (int i = 0; i < res.Count; i++)
      {
        Match r = res[i];
        string timing = $"{r.Groups[1]} {r.Groups[2]}";
        sb.AppendIfPrev("\r\n").AppendForPrev(timing);
        asList.AddLast(timing);
      }
      Timings = asList.ToArray();
      if (sb.Length > 0 && toFile)
        System.IO.File.WriteAllText(OutFile, sb.ToString());
    }
  }
}
