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
    public void Parse()
    {
      ArgumentParser parser = new ArgumentParser();
      parser.Parse(new string[] { 
      "-ma", "1",
      "-ss", "10",
      "-to", "20",
      "-or", "128",
      "-af", "file.ogg"
      });
      StringBuilder sb = new StringBuilder();
      parser.Apply(sb, Arg.ApplyTo.Audio);

      int brkpnt = 0;
    }
  }
}
