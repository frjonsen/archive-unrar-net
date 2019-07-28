using CommandLine;

namespace Unarchive
{
  public class CliOptions
  {
    [Option('m', "movie", Required = false, HelpText = "Unpack archive as a movie")]
    public bool Movie { get; set; }
    
    [Option('d', "dir", Required = false, HelpText = "Subdirectory to unpack to")]
    public string Directory { get; set; }
    
    [Option('c', "count", Required = false, HelpText = "Max amount of episodes to unpack")]
    public int EpisodeCount { get; set; }
  }
}
