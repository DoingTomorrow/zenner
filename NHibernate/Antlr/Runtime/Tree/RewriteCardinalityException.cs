// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Tree.RewriteCardinalityException
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System;

#nullable disable
namespace Antlr.Runtime.Tree
{
  [Serializable]
  internal class RewriteCardinalityException : Exception
  {
    public string elementDescription;

    public RewriteCardinalityException(string elementDescription)
    {
      this.elementDescription = elementDescription;
    }

    public override string Message
    {
      get => this.elementDescription != null ? this.elementDescription : (string) null;
    }
  }
}
