// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.GMM_VersionInfo
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace ZENNER.CommonLibrary
{
  public class GMM_VersionInfo
  {
    private static List<GMM_VersionInfo> AllVersionInfos = new List<GMM_VersionInfo>();
    public readonly Version FromVersion;
    public readonly Version ToVersion;
    public readonly DateTime? StartDate;
    public readonly DateTime? EndDate;
    public readonly string ProductionLineKey;
    public readonly string Description;

    static GMM_VersionInfo()
    {
      GMM_VersionInfo.AllVersionInfos.Add(new GMM_VersionInfo(new Version(5, 51), (Version) null, new DateTime?(new DateTime(2019, 1, 28)), new DateTime?(new DateTime(2019, 5, 13)), "IUWB", "IUW production branch for bulk meters"));
      GMM_VersionInfo.AllVersionInfos.Add(new GMM_VersionInfo(new Version(5, 53), (Version) null, new DateTime?(new DateTime(2019, 5, 13)), new DateTime?(new DateTime(2020, 11, 5)), "IUWB", "IUW production branch for bulk meters"));
      GMM_VersionInfo.AllVersionInfos.Add(new GMM_VersionInfo(new Version(5, 71), (Version) null, new DateTime?(new DateTime(2020, 2, 26)), new DateTime?(), "IUWS", "IUW production branch for residential meters"));
      GMM_VersionInfo.AllVersionInfos.Add(new GMM_VersionInfo(new Version(5, 79), (Version) null, new DateTime?(new DateTime(2020, 11, 5)), new DateTime?(), "IUWB", "IUW production branch for bulk meters. MID"));
      GMM_VersionInfo.AllVersionInfos.Add(new GMM_VersionInfo(new Version(5, 91), (Version) null, new DateTime?(new DateTime(2021, 10, 1)), new DateTime?(), "IUW", "IUW production branch for bulk and residential meters. MID"));
    }

    public static string GetProductionLineKey(int major, int minor, bool markObsolete = true)
    {
      GMM_VersionInfo gmmVersionInfo = GMM_VersionInfo.AllVersionInfos.FirstOrDefault<GMM_VersionInfo>((Func<GMM_VersionInfo, bool>) (x => x.FromVersion.Major == major && x.FromVersion.Minor == minor && x.FromVersion.Revision == -1 && x.FromVersion.Build == -1 && x.ToVersion == (Version) null));
      if (gmmVersionInfo == null)
        return (string) null;
      string productionLineKey = gmmVersionInfo.ProductionLineKey;
      int num;
      if (markObsolete && gmmVersionInfo.EndDate.HasValue)
      {
        DateTime? endDate = gmmVersionInfo.EndDate;
        DateTime now = DateTime.Now;
        num = endDate.HasValue ? (endDate.GetValueOrDefault() < now ? 1 : 0) : 0;
      }
      else
        num = 0;
      return num != 0 ? productionLineKey + "-obsolet" : productionLineKey;
    }

    private GMM_VersionInfo(
      Version fromVersion,
      Version toVersion,
      DateTime? startDate,
      DateTime? endDate,
      string productionLineKey,
      string description)
    {
      this.FromVersion = fromVersion;
      this.ToVersion = toVersion;
      this.StartDate = startDate;
      this.EndDate = endDate;
      this.ProductionLineKey = productionLineKey;
      this.Description = description;
    }
  }
}
