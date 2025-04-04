// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ParsingScope
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using System;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public class ParsingScope : IDisposable
  {
    private readonly ParsingScopes _parsingScopes;

    public ParsingScope(ParsingScopes parsingScopes, RangeAddress address)
      : this(parsingScopes, (ParsingScope) null, address)
    {
    }

    public ParsingScope(ParsingScopes parsingScopes, ParsingScope parent, RangeAddress address)
    {
      this._parsingScopes = parsingScopes;
      this.Parent = parent;
      this.Address = address;
      this.ScopeId = Guid.NewGuid();
    }

    public Guid ScopeId { get; private set; }

    public ParsingScope Parent { get; private set; }

    public RangeAddress Address { get; private set; }

    public bool IsSubtotal { get; set; }

    public void Dispose() => this._parsingScopes.KillScope(this);
  }
}
