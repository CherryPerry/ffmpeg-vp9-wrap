using System;
using System.Collections.Generic;
using System.Diagnostics;
using Vp9Encode.Args;

namespace Vp9Encode.Actions
{
  public abstract class Action
  {
    public abstract bool TryAction(string[] args);

    protected void ExecuteFFMPEG(string args)
    {
      Process proc = new Process();
      proc.StartInfo.FileName = "ffmpeg.exe";
      proc.StartInfo.Arguments = args;
      proc.StartInfo.UseShellExecute = false;
      proc.StartInfo.RedirectStandardOutput = true;
      proc.StartInfo.RedirectStandardError = true;
      proc.ErrorDataReceived += DataReceived;
      proc.OutputDataReceived += DataReceived;
      Console.WriteLine("\n\n" + args + "\n\n");
      proc.Start();
      proc.PriorityClass = ProcessPriorityClass.Idle;
      proc.BeginOutputReadLine();
      proc.BeginErrorReadLine();
      proc.WaitForExit();
      proc.Close();
    }

    private void DataReceived(object sender, DataReceivedEventArgs data)
    {
      if (data.Data != null && data.Data.Length == Console.WindowWidth)
        Console.Write(data.Data);
      else
        Console.WriteLine(data.Data);
    }
  }
}
