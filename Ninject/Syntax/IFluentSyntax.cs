// Decompiled with JetBrains decompiler
// Type: Ninject.Syntax.IFluentSyntax
// Assembly: Ninject, Version=3.0.0.0, Culture=neutral, PublicKeyToken=c7192dc5380945e7
// MVID: C76D661E-417A-4EBA-9151-4717B8101D58
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Ninject.dll

using System;
using System.ComponentModel;

#nullable disable
namespace Ninject.Syntax
{
  [EditorBrowsable(EditorBrowsableState.Never)]
  public interface IFluentSyntax
  {
    [EditorBrowsable(EditorBrowsableState.Never)]
    Type GetType();

    [EditorBrowsable(EditorBrowsableState.Never)]
    int GetHashCode();

    [EditorBrowsable(EditorBrowsableState.Never)]
    string ToString();

    [EditorBrowsable(EditorBrowsableState.Never)]
    bool Equals(object other);
  }
}
