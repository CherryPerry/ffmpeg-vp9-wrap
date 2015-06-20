using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Vp9Encode.Args;
using System.Text;

namespace VpxTest
{
  [TestClass]
  public class ArgumentParserTest
  {
    [TestMethod]
    public void GroupTest()
    {
      ArgumentParser parser = new ArgumentParser();
      parser.Parse(new string[] { 
      "-ss", "10",
      "-to", "20",
      "-subs", "subs.ass",
      "-scale", "-1:540",
      "-file", "1.mkv"
      },
      new Arg[] {
        new InternalStateArg()
      });
      StringBuilder sb = new StringBuilder(), sb2 = new StringBuilder();
      parser.Apply(sb, Arg.ApplyTo.Audio);
      parser.Apply(sb2, Arg.ApplyTo.Video);

      int brkpnt = 0;
    }
  }
}
