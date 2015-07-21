using HtmlAgilityPack;
using SharpCompress.Archive.SevenZip;
using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using VpxEncode.Properties;

namespace VpxEncode
{
  class FfmpegLoader
  {
    private const string ZERANOE_WEB = "http://ffmpeg.zeranoe.com/builds/";

    public string FolderPath { get; private set; }

    public FfmpegLoader()
    {
      FolderPath = @"C:\Program Files\FFMPEG Compact\";
    }

    public async Task Install()
    {
      Console.WriteLine("Getting link to latest ffmpeg");
      string link = await GetLinkFromWebPage();
      Console.WriteLine("Downloading ffmpeg");
      using (MemoryStream stream = await Download(link))
      using (SevenZipArchive archive = SevenZipArchive.Open(stream))
      {
        Console.WriteLine("Unpacking ffmpeg");
        string[] files = new string[] { "ffmpeg.exe", "ffprobe.exe" };
        foreach (string file in files)
        {
          SevenZipArchiveEntry f = archive.Entries.First(x => x.Key.Contains(file));
          string name = f.Key.Substring(f.Key.LastIndexOf('/') + 1);
          CheckDirAndCreate(FolderPath);
          using (Stream entryStream = f.OpenEntryStream())
          using (FileStream fileStream = File.OpenWrite(Path.Combine(FolderPath, name)))
            await entryStream.CopyToAsync(fileStream);
        }
      }
      Console.WriteLine("Update enviroment");
      SetEnvironment();
      Console.WriteLine("Set fontconfig");
      SetFontConfig();
      Console.WriteLine("Copying vp9.exe to " + FolderPath);
      string location = System.Reflection.Assembly.GetEntryAssembly().Location;
      File.Copy(location, Path.Combine(FolderPath, Path.GetFileName(location)));
    }

    private async Task<String> GetLinkFromWebPage()
    {
      WebRequest request = WebRequest.CreateHttp(ZERANOE_WEB);
      using (WebResponse response = await request.GetResponseAsync())
      using (Stream s = response.GetResponseStream())
      {
        HtmlDocument doc = new HtmlDocument();
        doc.Load(s);
        HtmlNodeCollection collection = doc.DocumentNode.SelectNodes("/html/body/div[@id='builds-page']/div[@class='grid-460']/a[@class='latest']");
        int bits = Environment.Is64BitProcess ? 64 : 32;
        Regex regex = new Regex($"Download FFmpeg git-\\S+ {bits}-bit Static");
        foreach (var item in collection)
        {
          if (regex.IsMatch(item.InnerText))
          {
            string link = item.GetAttributeValue("href", null);
            if (link != null)
            {
              // Link is not absolute (./win64/ffmpeg.7z)
              if (link.StartsWith("."))
                link = ZERANOE_WEB + link.Substring(2);
              return link;
            }
          }
        }
      }
      return null;
    }

    private async Task<MemoryStream> Download(string link)
    {
      WebRequest request = WebRequest.CreateHttp(link);
      using (WebResponse response = await request.GetResponseAsync())
      using (Stream s = response.GetResponseStream())
      {
        MemoryStream memory = new MemoryStream();
        await s.CopyToAsync(memory);
        return memory;
      }
    }

    private void SetEnvironment()
    {
      StringBuilder current = new StringBuilder(Environment.GetEnvironmentVariable("Path", EnvironmentVariableTarget.Machine));
      if (!current.Contains(FolderPath))
      {
        // Folder path always ends with \
        if (current[current.Length - 1] != ';')
          current.Append(';');
        current.Append(FolderPath);
        Environment.SetEnvironmentVariable("Path", current.ToString(), EnvironmentVariableTarget.Machine);
      }
    }

    private void SetFontConfig()
    {
      string fontDir = FolderPath + "fonts",
             fontFile = fontDir + "\\fonts.conf",
             fontCache = fontDir + "\\cache\\";
      CheckDirAndCreate(fontDir);
      CheckDirAndCreate(fontCache);

      byte[] bytes = Resources.fonts;
      File.WriteAllBytes(fontFile, bytes);

      string[] path = { "FC_CONFIG_DIR", "FONTCONFIG_PATH" };
      string[] file = { "FC_CONFIG_FILE", "FONTCONFIG_FILE" };
      foreach (string name in path)
        Environment.SetEnvironmentVariable(name, fontDir, EnvironmentVariableTarget.Machine);
      foreach (string name in file)
        Environment.SetEnvironmentVariable(name, fontFile, EnvironmentVariableTarget.Machine);
    }

    private void CheckDirAndCreate(string dir)
    {
      if (!Directory.Exists(dir))
        Directory.CreateDirectory(dir);
    }
  }
}
