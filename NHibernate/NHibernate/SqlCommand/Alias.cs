// Decompiled with JetBrains decompiler
// Type: NHibernate.SqlCommand.Alias
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

#nullable disable
namespace NHibernate.SqlCommand
{
  public class Alias
  {
    private readonly int length;
    private readonly string suffix;

    public Alias(int length, string suffix)
    {
      this.length = suffix == null ? length : length - suffix.Length;
      this.suffix = suffix;
    }

    public Alias(string suffix)
    {
      this.length = int.MaxValue;
      this.suffix = suffix;
    }

    public string ToAliasString(string sqlIdentifier, NHibernate.Dialect.Dialect dialect)
    {
      bool flag = dialect.IsQuoted(sqlIdentifier);
      string aliasName = (!flag ? sqlIdentifier : dialect.UnQuote(sqlIdentifier)).TrimStart('_');
      if (aliasName.Length > this.length)
        aliasName = aliasName.Substring(0, this.length);
      if (this.suffix != null)
        aliasName += this.suffix;
      return flag ? dialect.QuoteForAliasName(aliasName) : aliasName;
    }

    public string ToUnquotedAliasString(string sqlIdentifier, NHibernate.Dialect.Dialect dialect)
    {
      string unquotedAliasString = dialect.UnQuote(sqlIdentifier).TrimStart('_');
      if (unquotedAliasString.Length > this.length)
        unquotedAliasString = unquotedAliasString.Substring(0, this.length);
      if (this.suffix != null)
        unquotedAliasString += this.suffix;
      return unquotedAliasString;
    }

    public string[] ToUnquotedAliasStrings(string[] sqlIdentifiers, NHibernate.Dialect.Dialect dialect)
    {
      string[] unquotedAliasStrings = new string[sqlIdentifiers.Length];
      for (int index = 0; index < sqlIdentifiers.Length; ++index)
        unquotedAliasStrings[index] = this.ToUnquotedAliasString(sqlIdentifiers[index], dialect);
      return unquotedAliasStrings;
    }

    public string[] ToAliasStrings(string[] sqlIdentifiers, NHibernate.Dialect.Dialect dialect)
    {
      string[] aliasStrings = new string[sqlIdentifiers.Length];
      for (int index = 0; index < sqlIdentifiers.Length; ++index)
        aliasStrings[index] = this.ToAliasString(sqlIdentifiers[index], dialect);
      return aliasStrings;
    }
  }
}
