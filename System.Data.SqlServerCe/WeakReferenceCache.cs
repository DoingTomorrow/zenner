// Decompiled with JetBrains decompiler
// Type: System.Data.SqlServerCe.WeakReferenceCache
// Assembly: System.Data.SqlServerCe, Version=3.5.1.50, Culture=neutral, PublicKeyToken=89845dcd8080cc91
// MVID: 5CF67607-9835-4428-8475-9E80A4482327
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\System.Data.SqlServerCe.dll

#nullable disable
namespace System.Data.SqlServerCe
{
  internal class WeakReferenceCache
  {
    private bool _trackResurrection;
    protected WeakReference[] items;

    internal int Count
    {
      get
      {
        lock (this)
          return this.items.Length;
      }
    }

    internal WeakReferenceCache(bool trackResurrection)
    {
      this.items = new WeakReference[20];
      this._trackResurrection = trackResurrection;
    }

    internal int Add(object value)
    {
      lock (this)
      {
        int length = this.items.Length;
        for (int index = 0; index < length; ++index)
        {
          WeakReference weakReference = this.items[index];
          if (weakReference == null)
          {
            this.items[index] = new WeakReference(value, this._trackResurrection);
            return index;
          }
          if (!ADP.IsAlive(weakReference))
          {
            weakReference.Target = value;
            return index;
          }
        }
        WeakReference[] weakReferenceArray = new WeakReference[5 == length ? 15 : length + 15];
        for (int index = 0; index < length; ++index)
          weakReferenceArray[index] = this.items[index];
        weakReferenceArray[length] = new WeakReference(value, this._trackResurrection);
        this.items = weakReferenceArray;
        return length;
      }
    }

    internal object GetObject(int indx)
    {
      lock (this)
      {
        try
        {
          WeakReference weakReference = this.items[indx];
          return ADP.IsAlive(weakReference) ? weakReference.Target : (object) null;
        }
        catch (InvalidOperationException ex)
        {
          return (object) null;
        }
      }
    }

    internal void Remove(object value)
    {
      lock (this)
      {
        int length = this.items.Length;
        for (int index = 0; index < length; ++index)
        {
          WeakReference weakReference = this.items[index];
          try
          {
            if (ADP.IsAlive(weakReference))
            {
              if (value == weakReference.Target)
              {
                this.items[index] = (WeakReference) null;
                break;
              }
            }
          }
          catch (InvalidOperationException ex)
          {
          }
        }
      }
    }

    internal void RemoveAt(int index)
    {
      lock (this)
        this.items[index] = (WeakReference) null;
    }
  }
}
