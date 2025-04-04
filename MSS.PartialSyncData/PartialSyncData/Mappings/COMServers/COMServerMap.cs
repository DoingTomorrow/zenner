// Decompiled with JetBrains decompiler
// Type: MSS.PartialSyncData.Mappings.COMServers.COMServerMap
// Assembly: MSS.PartialSyncData, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 2C03C230-C045-4A02-9EF4-11D526D1F6D9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.PartialSyncData.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.COMServers;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.PartialSyncData.Mappings.COMServers
{
  public class COMServerMap : ClassMap<COMServer>
  {
    public COMServerMap()
    {
      this.Table("t_COMServer");
      this.Id((Expression<Func<COMServer, object>>) (n => (object) n.Id)).GeneratedBy.GuidComb();
      this.Map((Expression<Func<COMServer, object>>) (c => c.Name)).Length(200);
    }
  }
}
