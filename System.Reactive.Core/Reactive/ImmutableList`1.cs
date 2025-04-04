// Decompiled with JetBrains decompiler
// Type: System.Reactive.ImmutableList`1
// Assembly: System.Reactive.Core, Version=3.0.0.0, Culture=neutral, PublicKeyToken=94bc3704cddfc263
// MVID: 688278AD-98CB-433C-8F7D-49B6FA93B621
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Reactive.Core.dll

#nullable disable
namespace System.Reactive
{
  internal class ImmutableList<T>
  {
    public static readonly ImmutableList<T> Empty = new ImmutableList<T>();
    private readonly T[] _data;

    private ImmutableList() => this._data = new T[0];

    public ImmutableList(T[] data) => this._data = data;

    public T[] Data => this._data;

    public ImmutableList<T> Add(T value)
    {
      T[] objArray = new T[this._data.Length + 1];
      Array.Copy((Array) this._data, (Array) objArray, this._data.Length);
      objArray[this._data.Length] = value;
      return new ImmutableList<T>(objArray);
    }

    public ImmutableList<T> Remove(T value)
    {
      int num = this.IndexOf(value);
      if (num < 0)
        return this;
      int length = this._data.Length;
      if (length == 1)
        return ImmutableList<T>.Empty;
      T[] objArray = new T[length - 1];
      Array.Copy((Array) this._data, 0, (Array) objArray, 0, num);
      Array.Copy((Array) this._data, num + 1, (Array) objArray, num, length - num - 1);
      return new ImmutableList<T>(objArray);
    }

    private int IndexOf(T value)
    {
      for (int index = 0; index < this._data.Length; ++index)
      {
        if (object.Equals((object) this._data[index], (object) value))
          return index;
      }
      return -1;
    }
  }
}
