using System;
using System.Collections.Generic;
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

    public TimingGenerator(string file)
    {
      File = file;
    }

    public void Generate(bool toFile)
    {
      output = new Executer().Execute(String.Format("\"{0}\"", File));
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
        string timing = String.Format("{0} {1}", r.Groups[1], r.Groups[2]);
        sb.AppendIfPrev("\r\n").AppendForPrev(timing);
        asList.AddLast(timing);
      }
      Timings = asList.ToArray();
      if (sb.Length > 0 && toFile)
        System.IO.File.WriteAllText(File + "-timing.txt", sb.ToString());
    }
  }
}
