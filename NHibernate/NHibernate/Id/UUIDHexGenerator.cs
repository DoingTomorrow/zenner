// Decompiled with JetBrains decompiler
// Type: NHibernate.Id.UUIDHexGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Engine;
using NHibernate.Type;
using NHibernate.Util;
using System;
using System.Collections.Generic;

#nullable disable
namespace NHibernate.Id
{
  public class UUIDHexGenerator : IIdentifierGenerator, IConfigurable
  {
    protected const string FormatWithDigitsOnly = "N";
    protected string format = "N";
    protected string sep;

    public virtual object Generate(ISessionImplementor session, object obj)
    {
      string newGuid = this.GenerateNewGuid();
      return this.format != "N" && this.sep != null ? (object) StringHelper.Replace(newGuid, "-", this.sep) : (object) newGuid;
    }

    public virtual void Configure(IType type, IDictionary<string, string> parms, NHibernate.Dialect.Dialect dialect)
    {
      this.format = PropertiesHelper.GetString("format", parms, "N");
      if (!(this.format != "N"))
        return;
      this.sep = PropertiesHelper.GetString("seperator", parms, (string) null);
    }

    protected virtual string GenerateNewGuid() => Guid.NewGuid().ToString(this.format);
  }
}
