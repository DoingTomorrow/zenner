// Decompiled with JetBrains decompiler
// Type: OfficeOpenXml.FormulaParsing.ParsingScopes
// Assembly: EPPlus, Version=4.0.0.1, Culture=neutral, PublicKeyToken=ea159fdaa78159a1
// MVID: 3F10EAEA-823F-4076-B5B1-DE322159D5F9
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\EPPlus.dll

using OfficeOpenXml.FormulaParsing.ExcelUtilities;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace OfficeOpenXml.FormulaParsing
{
  public class ParsingScopes
  {
    private readonly IParsingLifetimeEventHandler _lifetimeEventHandler;
    private Stack<ParsingScope> _scopes = new Stack<ParsingScope>();

    public ParsingScopes(IParsingLifetimeEventHandler lifetimeEventHandler)
    {
      this._lifetimeEventHandler = lifetimeEventHandler;
    }

    public virtual ParsingScope NewScope(RangeAddress address)
    {
      ParsingScope parsingScope = this._scopes.Count<ParsingScope>() <= 0 ? new ParsingScope(this, address) : new ParsingScope(this, this._scopes.Peek(), address);
      this._scopes.Push(parsingScope);
      return parsingScope;
    }

    public virtual ParsingScope Current
    {
      get => this._scopes.Count<ParsingScope>() <= 0 ? (ParsingScope) null : this._scopes.Peek();
    }

    public virtual void KillScope(ParsingScope parsingScope)
    {
      this._scopes.Pop();
      if (this._scopes.Count<ParsingScope>() != 0)
        return;
      this._lifetimeEventHandler.ParsingCompleted();
    }
  }
}
