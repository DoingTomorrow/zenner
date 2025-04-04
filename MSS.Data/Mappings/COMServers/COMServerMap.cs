// Decompiled with JetBrains decompiler
// Type: MSS.Data.Mappings.COMServers.COMServerMap
// Assembly: MSS.Data, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 25FA456E-0753-42F8-8F73-BE22E5B867CF
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\MSS.Data.dll

using FluentNHibernate.Mapping;
using MSS.Core.Model.COMServers;
using System;
using System.Linq.Expressions;

#nullable disable
namespace MSS.Data.Mappings.COMServers
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
