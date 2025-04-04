// Decompiled with JetBrains decompiler
// Type: NHibernate.Util.WeakEnumerator
// Assembly: NHibernate, Version=3.3.1.4000, Culture=neutral, PublicKeyToken=aa95f207798dfdb4
// MVID: F2FE07FE-F4FA-4811-8A3A-0A4855BEE49E
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NHibernate.dll

using System.Collections;

#nullable disable
namespace NHibernate.Util
{
  public class WeakEnumerator : IDictionaryEnumerator, IEnumerator
  {
    private IDictionaryEnumerator innerEnumerator;
    private object currentKey;
    private object currentValue;

    public WeakEnumerator(IDictionaryEnumerator innerEnumerator)
    {
      this.innerEnumerator = innerEnumerator;
    }

    public object Key => this.currentKey;

    public object Value => this.currentValue;

    public DictionaryEntry Entry => new DictionaryEntry(this.currentKey, this.currentValue);

    public bool MoveNext()
    {
      do
      {
        this.currentKey = (object) null;
        this.currentValue = (object) null;
        if (!this.innerEnumerator.MoveNext())
          return false;
        this.currentKey = WeakRefWrapper.Unwrap(this.innerEnumerator.Key);
        this.currentValue = WeakRefWrapper.Unwrap(this.innerEnumerator.Value);
      }
      while (this.currentKey == null || this.currentValue == null);
      return true;
    }

    public void Reset()
    {
      this.innerEnumerator.Reset();
      this.currentKey = this.currentValue = (object) null;
    }

    public object Current => (object) this.Entry;
  }
}
