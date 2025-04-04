// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.ForUpdateFragment
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using NHibernate.Util;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace NHibernate.SqlCommand
{
  public class ForUpdateFragment
  {
    private readonly NHibernate.Dialect.Dialect dialect;
    private readonly StringBuilder aliases = new StringBuilder();
    private bool isNoWaitEnabled;

    public ForUpdateFragment(NHibernate.Dialect.Dialect dialect) => this.dialect = dialect;

    public ForUpdateFragment(
      NHibernate.Dialect.Dialect dialect,
      IDictionary<string, LockMode> lockModes,
      IDictionary<string, string[]> keyColumnNames)
      : this(dialect)
    {
      LockMode lockMode1 = (LockMode) null;
      foreach (KeyValuePair<string, LockMode> lockMode2 in (IEnumerable<KeyValuePair<string, LockMode>>) lockModes)
      {
        LockMode mode = lockMode2.Value;
        if (LockMode.Read.LessThan(mode))
        {
          string key = lockMode2.Key;
          if (dialect.ForUpdateOfColumns)
          {
            string[] keyColumnName = keyColumnNames[key];
            if (keyColumnName == null)
              throw new ArgumentException("alias not found: " + key);
            foreach (string alias in StringHelper.Qualify(key, keyColumnName))
              this.AddTableAlias(alias);
          }
          else
            this.AddTableAlias(key);
          lockMode1 = lockMode1 == null || mode == lockMode1 ? mode : throw new QueryException("mixed LockModes");
        }
        if (lockMode1 == LockMode.UpgradeNoWait)
          this.IsNoWaitEnabled = true;
      }
    }

    public bool IsNoWaitEnabled
    {
      get => this.isNoWaitEnabled;
      set => this.isNoWaitEnabled = value;
    }

    public ForUpdateFragment AddTableAlias(string alias)
    {
      if (this.aliases.Length > 0)
        this.aliases.Append(", ");
      this.aliases.Append(alias);
      return this;
    }

    public string ToSqlStringFragment()
    {
      if (this.aliases.Length == 0)
        return string.Empty;
      return !this.isNoWaitEnabled ? this.dialect.GetForUpdateString(this.aliases.ToString()) : this.dialect.GetForUpdateNowaitString(this.aliases.ToString());
    }
  }
}
