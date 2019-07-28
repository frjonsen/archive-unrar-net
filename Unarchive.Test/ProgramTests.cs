using NUnit.Framework;

namespace Unarchive.Test
{
  public class Tests
  {
    [Test]
    public void GetShowName_valid()
    {
      Assert.AreEqual("Show1", Program.GetShowName("Show1.s02e05.1080p.WEB.x265-RACZO"));
      Assert.AreEqual("Some.Show.Name", Program.GetShowName("Some.Show.Name.S01E01.NORDIC.1080p.WEB-DL.H.264-WEEDPOT"));
      Assert.AreEqual("lower.case.and.numbers.2017", Program.GetShowName("lower.case.and.numbers.2017.S01E01.AMZN.WEB-DL.DDP5.1.H.264-NTG"));
      Assert.AreEqual("No.Episode.Number", Program.GetShowName("No.Episode.Number.S01.1080p.AMZN.WEB-DL.DDP5.1.H.264-NTG"));
      Assert.AreEqual("No.Season.Specified", Program.GetShowName("No.Season.Specified.1080p.BluRay.x264-FLAME"));
    }

    [Test]
    public void GetShowName_NothingIdentifying()
    {
      Assert.AreEqual("Nothing.Identifying.BluRay.x264-FLAME", Program.GetShowName("Nothing.Identifying.BluRay.x264-FLAME"));
    }
  }
}
