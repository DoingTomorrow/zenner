// Decompiled with JetBrains decompiler
// Type: Common.Library.Core.Business.Database.IConnectionStringProvider
// Assembly: Common.Library.Core.Business, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 848DF87A-D999-47E1-B1BF-1A19BA680E53
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Common.Library.Core.Business.dll

#nullable disable
namespace Common.Library.Core.Business.Database
{
  public interface IConnectionStringProvider
  {
    string GetConnectionString();

    string GetConnectionString(params object[] parameters);
  }
}
