using System;
using System.Windows.Forms;
using Vp9Encode.Actions;

namespace FfmpegEncode
{
  public static class Program
  {
    [STAThread]
    public static void Main(string[] args)
    {
      Actions.SelectAction(args);
      MessageBox.Show("OK");
    }
  }
}