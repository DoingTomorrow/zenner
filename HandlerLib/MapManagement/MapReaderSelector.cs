// Decompiled with JetBrains decompiler
// Type: HandlerLib.MapManagement.MapReaderSelector
// Assembly: HandlerLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 32680C26-DD6F-4028-82D3-7440714FE33F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\HandlerLib.dll

using System;
using System.Collections.Generic;
using System.Linq;

#nullable disable
namespace HandlerLib.MapManagement
{
  public class MapReaderSelector
  {
    public SortedList<string, MapReader> MapReaderList;

    public MapReaderSelector()
    {
      this.MapReaderList = new SortedList<string, MapReader>();
      this.getAllDerivedMapReader();
    }

    private bool getAllDerivedMapReader()
    {
      try
      {
        foreach (MapReader mapReader in ((IEnumerable<Type>) typeof (MapReader).Assembly.GetTypes()).Where<Type>((Func<Type, bool>) (t => t.IsSubclassOf(typeof (MapReader)) && !t.IsAbstract)).Select<Type, MapReader>((Func<Type, MapReader>) (t => (MapReader) Activator.CreateInstance(t))))
          this.MapReaderList.Add(mapReader.ReaderName, mapReader);
        return true;
      }
      catch (Exception ex)
      {
        throw new Exception("An error occured: \n" + ex.Message);
      }
    }

    public void Clear() => this.MapReaderList.Clear();

    internal MapReader getReader(string p)
    {
      return this.MapReaderList.ContainsKey(p) ? this.MapReaderList[p] : (MapReader) null;
    }
  }
}
