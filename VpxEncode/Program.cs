using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vp9Encode;

namespace FfmpegEncode
{
  partial class Arg
  {
    public const string TIMINGS_INDEX = "ti", FIX_SUBS = "fs",
                        SUBS_INDEX = "si", CRF = "crf",
                        OTHER_VIDEO = "ov", OTHER_AUDIO = "oa",
                        QUALITY = "quality", LIMIT = "limit",
                        FILE = "file", SUBS = "subs",
                        TIMINGS = "t", START_TIME = "ss",
                        END_TIME = "to", MAP_AUDIO = "ma",
                        SCALE = "scale", GENERATE_TIMING = "gent",
                        OPUS_RATE = "opusRate", NAME_PREFIX = "name",
                        QMIN = "qmin", QMAX = "qmax",
                        AUDIO_FILE = "af", AUTOLIMIT = "autolimit",
                        AUTOLIMIT_DELTA = "autolimitDelta", NO_FAST_SKIP = "noFastSkip";
  }

  static class ArgList
  {
    static Dictionary<string, Arg> ArgsDict = new Dictionary<string, Arg>()
      {
        { Arg.FILE, new Arg(Arg.FILE, null, "{string} файл") },
        { Arg.SUBS, new Arg(Arg.SUBS, null, "{string} сабы") },
        { Arg.TIMINGS, new Arg(Arg.TIMINGS, null, "{string} файл таймингов ({00:00.000|00:00:00.000|0} {00:00.000|00:00:00.000|0}\\n)") },
        { Arg.START_TIME, new Arg(Arg.START_TIME, "0", "{00:00.000|00:00:00.000|0} начало отрезка") },
        { Arg.END_TIME, new Arg(Arg.END_TIME, null, "{00:00.000|00:00:00.000|0} конец отрезка") },
        { Arg.MAP_AUDIO, new Arg(Arg.MAP_AUDIO, null, "{int} для смены аудиодорожки (эквивалент -map 0:{int})") },
        { Arg.SCALE, new Arg(Arg.SCALE, "-1:540", "no|{int:int} скейл изображения (default: -1:540)") },
        { Arg.OTHER_VIDEO, new Arg(Arg.OTHER_VIDEO, string.Empty, "{string} доп. параметры выходного файла видео \"-qmin 30\"") },
        { Arg.OTHER_AUDIO, new Arg(Arg.OTHER_AUDIO, string.Empty, "{string} доп. параметры выходного файла аудио \"-af=volume=3\"") },
        { Arg.QUALITY, new Arg(Arg.QUALITY, "good", "{best|good} качество видеокодирования") },
        { Arg.LIMIT, new Arg(Arg.LIMIT, "10240", "{int} лимит в KB (default: 10240)") },
        { Arg.OPUS_RATE, new Arg(Arg.OPUS_RATE, "80", "{int} битрейт аудио (Opus) в Kbps") },
        { Arg.NAME_PREFIX, new Arg(Arg.NAME_PREFIX, "", "префикс имени результата") },
        { Arg.TIMINGS_INDEX, new Arg(Arg.TIMINGS_INDEX, null, "индекс одного файла для обработки при работе с файлом таймингов") },
        { Arg.FIX_SUBS, new Arg(Arg.FIX_SUBS, null, "замена шрифтов в ass субтитрах на Arial (если ffmpeg не находит шрифт)", false) },
        { Arg.SUBS_INDEX, new Arg(Arg.SUBS_INDEX, null, "индекс субтитров, если в контейнере", ":si={0}") },
        { Arg.QMIN, new Arg(Arg.QMIN, null, "{int} режим качества, задается вместе с qmax", "-qmin {0}") },
        { Arg.QMAX, new Arg(Arg.QMAX, null, "{int} режим качества", "-qmax {0}") },
        { Arg.CRF, new Arg(Arg.CRF, null, "{int} режим качества", "-crf {0}") },
        { Arg.AUDIO_FILE, new Arg(Arg.AUDIO_FILE, null, "{string} внешняя аудиодорожка", "-map 0:{0}") },
        { Arg.GENERATE_TIMING, new Arg(Arg.GENERATE_TIMING, null, "сгенерировать timings.txt из ffprobe", false) },
        { Arg.AUTOLIMIT, new Arg(Arg.AUTOLIMIT, null, "подогнать под лимит", false) },
        { Arg.AUTOLIMIT_DELTA, new Arg(Arg.AUTOLIMIT_DELTA, "240", "{int} погрешность автоподгона в KB") },
        { Arg.NO_FAST_SKIP, new Arg(Arg.NO_FAST_SKIP, null, "{yes} не использовать setpts=PTS-STARTPTS для субтитров", false) }
      };

    public static void Parse(string[] args)
    {
      if (args == null || args.Length == 0)
        return;

      Arg lastArg = null;
      bool argHasValue = true;

      foreach (string arg in args)
      {
        if (argHasValue)
        {
          // Parameter name
          string name = arg.Substring(1);

          if (ArgsDict.ContainsKey(name))
            lastArg = ArgsDict[name];
          else
          {
            lastArg = new Arg(name, null);
            ArgsDict[lastArg.Name] = lastArg;
          }
          argHasValue = !lastArg.WaitForArg;
          if (argHasValue)
            lastArg.Value = "__EMPTY__";
        }
        else
        {
          // Parameter value
          lastArg.Value = arg;
          argHasValue = true;
        }
      }
    }

    public static Arg Get(string name)
    {
      if (ArgsDict.ContainsKey(name))
        return ArgsDict[name];
      return null;
    }

    public static void WriteDescription()
    {
      foreach (var pair in ArgsDict)
        Console.WriteLine(pair.Value.FullDescription);
    }
  }

  partial class Arg
  {
    public string Name { get; private set; }
    public string Value { get; set; }
    public string Descripton { get; private set; }
    public bool WaitForArg { get; private set; }
    public string Format { get; private set; }

    public Arg() { }

    public Arg(string name, string def)
    {
      Name = name;
      Value = def;
      WaitForArg = true;
    }

    public Arg(string name, string def, string desc) : this(name, def) { Descripton = desc; }

    public Arg(string name, string def, string desc, bool wait) : this(name, def, desc) { WaitForArg = wait; }

    public Arg(string name, string def, string desc, string format) : this(name, def, desc) { Format = format; }

    public static implicit operator bool(Arg arg) { return arg.Value != null; }

    public string AsString() { return Value; }

    public int AsInt() { return Int32.Parse(Value); }

    public TimeSpan AsTimeSpan() { return Program.ParseToTimespan(Value); }

    public string FullDescription { get { return String.Format("-{0}\t\t{1}", Name, Descripton); } }

    public string Command { get { if (Value != null) return String.Format(Format, Value); return String.Empty; } }
  }

  public static class Program
  {
    [STAThread]
    public static void Main(string[] args)
    {
      if (args == null || args.Length == 0)
      {
        ArgList.WriteDescription();
        return;
      }

      ArgList.Parse(args);

      string filePath = ArgList.Get(Arg.FILE).AsString();
      string subPath = ArgList.Get(Arg.SUBS).AsString();
      filePath = GetFullPath(filePath);

      if (ArgList.Get(Arg.GENERATE_TIMING))
      {
        TimingGenerator tg = new TimingGenerator(filePath);
        tg.Generate(true);
        return;
      }

      if (ArgList.Get(Arg.TIMINGS))
      {
        string timingsPath = GetFullPath(ArgList.Get(Arg.TIMINGS).AsString());
        string[] lines = File.ReadAllLines(timingsPath);
        Action<int, bool> startEncodeTiming = (index, threading) =>
        {
          Console.WriteLine("Start encode timing file {0} line", index);
          string[] splitted = lines[index].Split(' ');
          if (splitted.Length == 2)
            if (ArgList.Get(Arg.AUTOLIMIT))
              BitrateLookupEncode((newTarget) =>
              {
                return Encode(index, filePath, subPath, ParseToTimespan(splitted[0]), ParseToTimespan(splitted[1]), newTarget, threading);
              });
            else
              Encode(index, filePath, subPath, ParseToTimespan(splitted[0]), ParseToTimespan(splitted[1]), ArgList.Get(Arg.LIMIT).AsInt(), threading);
        };
        if (ArgList.Get(Arg.TIMINGS_INDEX))
        {
          int singleIndex = ArgList.Get(Arg.TIMINGS_INDEX).AsInt();
          if (lines.Length > singleIndex && singleIndex >= 0)
            startEncodeTiming(singleIndex, true);
        }
        else
        {
          ParallelOptions options = new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount - 1 };
          Parallel.For(0, lines.Length, options, (i) => { startEncodeTiming(i, false); });
        }
      }
      else if (ArgList.Get(Arg.START_TIME) && ArgList.Get(Arg.END_TIME))
      {
        if (ArgList.Get(Arg.AUTOLIMIT))
          BitrateLookupEncode((newTarget) =>
          {
            return Encode(DateTime.Now.ToFileTimeUtc(), filePath, subPath, ArgList.Get(Arg.START_TIME).AsTimeSpan(), ArgList.Get(Arg.END_TIME).AsTimeSpan(), newTarget, true);
          });
        else
          Encode(DateTime.Now.ToFileTimeUtc(), filePath, subPath, ArgList.Get(Arg.START_TIME).AsTimeSpan(), ArgList.Get(Arg.END_TIME).AsTimeSpan(), ArgList.Get(Arg.LIMIT).AsInt(), true);
      }

      MessageBox.Show("OK");
    }

    static void BitrateLookupEncode(Func<int, string> encodeFunc)
    {
      throw new NotImplementedException();

      int limit = ArgList.Get(Arg.LIMIT).AsInt();
      int delta = ArgList.Get(Arg.AUTOLIMIT_DELTA).AsInt();
      IBitrateLookup bl = new LinearBitrateLookup(limit);
      int size = 0;
      while (!(limit - size < delta && size < limit))
      {
        int newTarget = bl.GetTarget();
        string result = encodeFunc(newTarget);
        FileInfo fileInfo = new FileInfo(result);
        size = (int)(fileInfo.Length / 1024d);
        bl.AddPoint(newTarget, size);
      }
    }

    static string Encode(long i, string file, string subs, TimeSpan start, TimeSpan end, int sizeLimit, bool threading)
    {
      bool subsWereCopied = false;
      string subsFilename = Path.GetFileName(subs);
      if (ArgList.Get(Arg.FIX_SUBS) || GetFolder(subs) != Environment.CurrentDirectory)
      {
        string subsNew = Path.Combine(Environment.CurrentDirectory, subsFilename);
        if (ArgList.Get(Arg.FIX_SUBS) && SubStationAlpha.IsAcceptable(subs))
        {
          subsFilename += i.ToString() + "_ARIAL.ass";
          SubStationAlpha ssa = new SubStationAlpha(subs);
          ssa.ChangeFontAndSave(subsNew);
        }
        else
          File.Copy(subs, subsNew);
        subs = subsNew;
        subsWereCopied = true;
      }
      subs = subsFilename;

      string code = null;
      if (i < 10000)
        code = String.Format("{0}_{1}", i, DateTime.Now.ToFileTimeUtc());
      else
        code = i.ToString();

      string filePath = GetFolder(file),
             webmPath = Path.Combine(filePath, String.Format("temp_{0}.webm", code)),
             oggPath = Path.Combine(filePath, String.Format("temp_{0}.ogg", code)),
             finalPath = Path.Combine(filePath, String.Format("{1}{0}.webm", code, ArgList.Get(Arg.NAME_PREFIX).AsString()));

      TimeSpan timeLength = end - start;
      string startString = start.ToString("hh\\:mm\\:ss\\.fff"),
             timeLengthString = timeLength.ToString("hh\\:mm\\:ss\\.fff");

      // Audio settings
      string mapAudio = String.Empty;
      if (ArgList.Get(Arg.MAP_AUDIO))
        mapAudio = String.Format("-map 0:{0}", ArgList.Get(Arg.MAP_AUDIO).AsInt());
      int opusRate = ArgList.Get(Arg.OPUS_RATE).AsInt();
      string audioFile = ArgList.Get(Arg.AUDIO_FILE) ? GetFullPath(ArgList.Get(Arg.AUDIO_FILE).AsString()) : file;

      // Encode audio
      ExecuteFFMPEG(String.Format("-y -ss {1} -i \"{0}\" {5} -ac 2 -c:a opus -b:a {6}K -vbr on -vn -sn -t {2} {4} \"{3}\"",
        audioFile, startString, timeLengthString, oggPath, ArgList.Get(Arg.OTHER_AUDIO).AsString(), mapAudio, opusRate));

      // VideoFilter
      const string vfDefault = "-vf ";
      StringBuilder vf = new StringBuilder(vfDefault);
      string scale = ArgList.Get(Arg.SCALE).AsString();
      string si = ArgList.Get(Arg.SUBS_INDEX).AsString();
      if (subs != null)
      {
        string format = subs.EndsWith("ass") || subs.EndsWith("ssa") ? "ass=\"{0}\"{1}" : "subtitles=\"{0}\"{1}";
        format = String.Format(format, subs.Replace("[", "\\[").Replace("]", "\\]").Replace("'", "\\'"), ArgList.Get(Arg.SUBS_INDEX).Command);
        if (!ArgList.Get(Arg.NO_FAST_SKIP))
          format = String.Format(new CultureInfo("en"), "setpts=PTS+{0:0.######}/TB,{1},setpts=PTS-STARTPTS", start.TotalSeconds, format);
        vf.AppendForPrev(format);
      }
      if (scale != "no")
        vf.AppendIfPrev(",").AppendForPrev(String.Format("scale={0}", scale));
      if (vf.Length == vfDefault.Length)
        vf.Clear();

      // Encode 2-pass video
      string quality = ArgList.Get(Arg.QUALITY).AsString();
      FileInfo info = new FileInfo(oggPath);
      double audioSize = info.Length / 1024d;
      int bitrate = (int)((sizeLimit - audioSize) * 8 * 0.95 / timeLength.TotalSeconds);
      string bitrateString = ArgList.Get(Arg.QMAX) ? "" : String.Format("-b:v {0}K", bitrate);

      StringBuilder otherVideo = new StringBuilder();
      otherVideo.AppendForPrev(ArgList.Get(Arg.QMIN).Command).AppendIfPrev(" ").AppendForPrev(ArgList.Get(Arg.QMAX).Command).AppendIfPrev(" ");
      otherVideo.AppendForPrev(ArgList.Get(Arg.CRF).Command).AppendIfPrev(" ");
      otherVideo.AppendForPrev(ArgList.Get(Arg.OTHER_VIDEO).AsString()).AppendIfPrev(" ");

      string commandStart;
      if (ArgList.Get(Arg.NO_FAST_SKIP))
        commandStart = "-y -i \"{0}\" -ss {3} ";
      else
        commandStart = "-y -ss {3} -i \"{0}\" ";

      ExecuteFFMPEG(
        String.Format(commandStart + "-c:v vp9 {1} -tile-columns 6 -frame-parallel 1 -speed 4 -threads 8 -an {2} -t {4} -sn {7} -lag-in-frames 25 -pass 1 -auto-alt-ref 1 -passlogfile temp_{5} \"{6}\"",
                      file, bitrateString, vf, startString, timeLengthString, code, webmPath, otherVideo));

      ExecuteFFMPEG(
        String.Format(commandStart + "-c:v vp9 {1} -tile-columns 6 -frame-parallel 1 -speed 1 -threads 8 -an {2} -t {4} -sn {7} -lag-in-frames 25 -pass 2 -auto-alt-ref 1 -quality {8} -passlogfile temp_{5} \"{6}\"",
                      file, bitrateString, vf, startString, timeLengthString, code, webmPath, otherVideo, quality));

      // Concat
      ExecuteFFMPEG(String.Format("-y -i \"{0}\" -i \"{1}\" -c copy \"{2}\"", webmPath, oggPath, finalPath));

      // Delete
      if (subsWereCopied)
        File.Delete(subs);
      File.Delete(webmPath);
      File.Delete(oggPath);
      File.Delete(Path.Combine(filePath, String.Format("temp_{0}-0.log", code)));

      return finalPath;
    }

    static void ExecuteFFMPEG(string args)
    {
      Process proc = new Process();
      proc.StartInfo.FileName = "ffmpeg.exe";
      proc.StartInfo.Arguments = args;
      proc.StartInfo.UseShellExecute = false;
      proc.StartInfo.RedirectStandardOutput = true;
      proc.StartInfo.RedirectStandardError = true;
      proc.ErrorDataReceived += (sender, data) =>
      {
        if (data.Data != null && data.Data.Length == Console.WindowWidth)
          Console.Write(data.Data);
        else
          Console.WriteLine(data.Data);
      };
      Console.WriteLine("\n\n" + args + "\n\n");
      proc.Start();
      proc.PriorityClass = ProcessPriorityClass.Idle;
      proc.BeginOutputReadLine();
      proc.BeginErrorReadLine();
      proc.WaitForExit();
      proc.Close();
    }

    static string GetFullPath(string file)
    {
      return Path.Combine(GetFolder(file), Path.GetFileName(file));
    }

    static string GetFolder(string file)
    {
      string pathToFile = Path.GetDirectoryName(file);
      if (String.IsNullOrEmpty(pathToFile))
        pathToFile = Environment.CurrentDirectory;
      return pathToFile;
    }

    internal static TimeSpan ParseToTimespan(string str)
    {
      try { return TimeSpan.ParseExact(str, "hh\\:mm\\:ss\\.fff", CultureInfo.InvariantCulture); }
      catch (FormatException)
      {
        try { return TimeSpan.ParseExact(str, "mm\\:ss\\.fff", CultureInfo.InvariantCulture); }
        catch (FormatException) { return TimeSpan.FromSeconds(Double.Parse(str, new CultureInfo("en"))); }
      }
    }
  }
}