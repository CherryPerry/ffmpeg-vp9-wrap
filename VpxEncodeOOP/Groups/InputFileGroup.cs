using System.Collections.Generic;
using System.Linq;
using Vp9Encode.Args;

namespace Vp9Encode.Groups
{
  public class InputFileGroup : ArgGroup
  {
    public override System.Type[] AfterGroup { get { return null; } }

    public override void ApplyGroupRule(ICollection<Arg> args)
    {
      VideoFileArg video = args.FirstOrDefault(x => x is VideoFileArg) as VideoFileArg;
      AudioFileArg audio = args.FirstOrDefault(x => x is AudioFileArg) as AudioFileArg;
      if (audio == null)
      {
        audio = new AudioFileArg(video.Value);
        args.Add(audio);
      }
    }
  }
}
