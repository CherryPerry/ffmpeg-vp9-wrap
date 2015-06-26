using System;
using System.Diagnostics;
using System.Text;

namespace VpxEncode
{
  public sealed class FfprobeExecuter
  {
    private StringBuilder builder;

    public FfprobeExecuter()
    {
      builder = new StringBuilder();
    }

    public string ExecuteFfprobe(string args)
    {
      builder.Clear();
      Process proc = new Process();
      proc.StartInfo.FileName = "ffprobe.exe";
      proc.StartInfo.Arguments = args;
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
      return builder.ToString();
    }

    void ProcessString(string str)
    {
      if (str.Length == Console.WindowWidth)
        Console.Write(str);
      else
        Console.WriteLine(str);
      builder.Append(str);
    }
  }
}
