using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using CommandLine;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using SharpCompress.Archives;
using SharpCompress.Archives.Rar;
using SharpCompress.Common;

[assembly: InternalsVisibleTo("Unarchive.Test")]
namespace Unarchive
{
  // ReSharper disable once ClassNeverInstantiated.Global
  internal class Program
  {
    private static CliOptions CliOptions { get; set; }
    
    private static void Main(string[] args)
    {
      ParseCommandLine(args);
      Log.Logger = new LoggerConfiguration().WriteTo.Console(theme: ConsoleTheme.None).CreateLogger();
      Log.Logger.IsEnabled(LogEventLevel.Information);
      if (GetArchiveType() == ArchiveType.Movie)
      {
        UnpackMovie();
      }
      else
      {
        UnpackTv();
      }
    }

    private static void ParseCommandLine(IEnumerable<string> args)
    {
      Parser.Default.ParseArguments<CliOptions>(args).WithParsed<CliOptions>(o => CliOptions = o);
    }

    private static ArchiveType GetArchiveType()
    {
      return CliOptions.Movie ? ArchiveType.Movie : ArchiveType.Tv;
    }

    
    private static string GetTargetBaseDirectory(ArchiveType type)
    {
      var environmentVariable = type == ArchiveType.Movie ? "MOVIES" : "TV";
      var baseDirectory = Environment.GetEnvironmentVariable(environmentVariable);
      Log.Information($"Base unpacking directory: {baseDirectory}");
      if (string.IsNullOrWhiteSpace(baseDirectory))
      {
        throw new Exception($"Environment variable {environmentVariable} is not set");
      }

      return baseDirectory;
    }

    private static string GetRarFileFromDirectory(string path)
    {
      return Directory.GetFiles(path).FirstOrDefault(f => f.EndsWith(".rar"));
    }

    private static void UnpackTv()
    {
      var targetDirectoryBase = GetTargetBaseDirectory(ArchiveType.Tv);
      var targetDirectory = !string.IsNullOrWhiteSpace(CliOptions.Directory)
        ? Path.Combine(targetDirectoryBase, CliOptions.Directory)
        : targetDirectoryBase;
      Log.Information($"Unpacking to {targetDirectory}");
      // If there is a rar in cwd, assume single episode
      var mainRar = GetRarFileFromDirectory(Directory.GetCurrentDirectory());
      if (mainRar != null)
      {
        Log.Information("Found a rar in cwd. Assuming this is not a pack.");
        UnpackRar(mainRar, targetDirectory);
      }
    }

    internal static string GetShowName(string cwd)
    {
      var nameEndPattern = new Regex(@"\.(1080p|720p|(s\d{1,2}(e\d{1,2})?))", RegexOptions.IgnoreCase);
      var match = nameEndPattern.Match(cwd);
      return match.Index == 0 ? cwd : cwd.Substring(0, match.Index);
    }

    private static void UnpackMovie()
    {
      var cwd = Directory.GetCurrentDirectory();
      var mainRarFile = GetRarFileFromDirectory(cwd);
      var targetDirectory = GetTargetBaseDirectory(ArchiveType.Movie);
      try
      {
        UnpackRar(mainRarFile, targetDirectory);
      }
      catch (Exception e)
      {
        Log.Error(e, $"Failed to unpack {mainRarFile}");
      }
    }

    private static void UnpackRar(string source, string destination)
    {
      Log.Information($"Unpacking {source}");
      using var archive = RarArchive.Open(source);
      foreach (var entry in archive.Entries)
      {
        entry.WriteToDirectory(destination, new ExtractionOptions
        {
          ExtractFullPath = true,
          Overwrite = true
        });
      }
    }
  }
}
