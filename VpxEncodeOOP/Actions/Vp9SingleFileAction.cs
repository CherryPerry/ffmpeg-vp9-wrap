using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Vp9Encode.Args;

namespace Vp9Encode.Actions
{
  public class Vp9SingleFileAction : Action
  {
    public override bool TryAction(string[] args)
    {
      if (args.Contains(Reflections.GetInputSequence(typeof(StartTimeArg)))
        && args.Contains(Reflections.GetInputSequence(typeof(EndTimeArg))))
      {
        long code = DateTime.Now.ToFileTimeUtc();
        string audioFile = String.Format("{0}.ogg", code);

        ArgumentParser ap = new ArgumentParser();
        InternalStateArg inter = new InternalStateArg("Code", code, "AudioSize", -1, "Pass", 1);

        // Audio
        ap.Parse(args, new Arg[] { new OutputArg(audioFile), inter });
        StringBuilder start = new StringBuilder();
        ap.Apply(start, Vp9Encode.Args.Arg.ApplyTo.Audio);
        ExecuteFFMPEG(start.ToString());

        // Video
        string videoFile = String.Format("{0}.webm", code);
        inter.Set("AudioSize", (int)((new FileInfo(audioFile)).Length / 1024));
        start.Clear();

        ap.Parse(args, new Arg[] { new OutputArg(videoFile), inter });
        ap.Apply(start, Vp9Encode.Args.Arg.ApplyTo.Video);
        ExecuteFFMPEG(start.ToString());

        inter.Set("Pass", 2);
        start.Clear();
        IReadOnlyList<Arg> list = ap.Parse(args, new Arg[] { new OutputArg(videoFile), inter });
        ap.Apply(start, Vp9Encode.Args.Arg.ApplyTo.Video);
        ExecuteFFMPEG(start.ToString());

        // Concat
        VideoFileArg vfa = list.First(x=>x is VideoFileArg) as VideoFileArg;
        AudioFileArg afa = list.First(x=>x is AudioFileArg) as AudioFileArg;
        ExecuteFFMPEG(String.Format("-y -i \"{0}\" -i \"{1}\" -c copy \"{0}\"", vfa.Value, afa.Value));

        // Delete
        File.Delete(afa.Value);
        File.Delete(String.Format("temp_{0}-0.log", code));

        return true;
      }
      return false;
    }
  }
}
