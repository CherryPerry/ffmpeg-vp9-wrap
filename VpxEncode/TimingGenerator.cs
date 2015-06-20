using System;
using System.Collections.Generic;
using System.Diagnostics;
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

    private StringBuilder builder = new StringBuilder();

    public TimingGenerator(string file)
    {
      File = file;
    }

    public void Generate(bool toFile)
    {
      ExecuteFfprobe();
      GetTimings(toFile);
    }

    void ExecuteFfprobe()
    {
      Process proc = new Process();
      proc.StartInfo.FileName = "ffprobe.exe";
      proc.StartInfo.Arguments = String.Format("\"{0}\"", File);
      proc.StartInfo.UseShellExecute = false;
      proc.StartInfo.RedirectStandardOutput = true;
      proc.StartInfo.RedirectStandardError = true;
      DataReceivedEventHandler handler = (sender, data) =>
      {
        if (data.Data != null)
          ProcessString(data.Data);
      };
      proc.ErrorDataReceived += handler;
      proc.OutputDataReceived += handler;
      proc.Start();
      proc.PriorityClass = ProcessPriorityClass.Idle;
      proc.BeginOutputReadLine();
      proc.BeginErrorReadLine();
      proc.WaitForExit();
      proc.Close();
    }

    void ProcessString(string str)
    {
      if (str.Length == Console.WindowWidth)
        Console.Write(str);
      else
        Console.WriteLine(str);
      builder.Append(str);
    }

    void GetTimings(bool toFile)
    {
      Regex reg = new Regex(@"Chapter #\d:\d: start (\d+.\d+), end (\d+.\d+)");
      MatchCollection res = reg.Matches(builder.ToString());
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
