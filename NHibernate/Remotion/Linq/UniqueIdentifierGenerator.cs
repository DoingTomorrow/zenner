// Decompiled with JetBrains decompiler
// Type: Remotion.Linq.UniqueIdentifierGenerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using Remotion.Linq.Utilities;
using System.Collections.Generic;

#nullable disable
namespace Remotion.Linq
{
  public class UniqueIdentifierGenerator
  {
    private readonly HashSet<string> _knownIdentifiers = new HashSet<string>();
    private int _identifierCounter;

    public void AddKnownIdentifier(string identifier)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (identifier), identifier);
      this._knownIdentifiers.Add(identifier);
    }

    private bool IsKnownIdentifier(string identifier)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (identifier), identifier);
      return this._knownIdentifiers.Contains(identifier);
    }

    public void Reset()
    {
      this._knownIdentifiers.Clear();
      this._identifierCounter = 0;
    }

    public string GetUniqueIdentifier(string prefix)
    {
      ArgumentUtility.CheckNotNullOrEmpty(nameof (prefix), prefix);
      string identifier;
      do
      {
        identifier = prefix + (object) this._identifierCounter;
        ++this._identifierCounter;
      }
      while (this.IsKnownIdentifier(identifier));
      return identifier;
    }
  }
}
