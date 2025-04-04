// Decompiled with JetBrains decompiler
// Type: Antlr.Runtime.Collections.StackList
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;

#nullable disable
namespace Antlr.Runtime.Collections
{
  internal class StackList : ArrayList
  {
    public void Push(object item) => this.Add(item);

    public object Pop()
    {
      object obj = this[this.Count - 1];
      this.RemoveAt(this.Count - 1);
      return obj;
    }

    public object Peek() => this[this.Count - 1];
  }
}
