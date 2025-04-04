// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonMergeSettings
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public class JsonMergeSettings
  {
    private MergeArrayHandling _mergeArrayHandling;
    private MergeNullValueHandling _mergeNullValueHandling;

    public MergeArrayHandling MergeArrayHandling
    {
      get => this._mergeArrayHandling;
      set
      {
        this._mergeArrayHandling = value >= MergeArrayHandling.Concat && value <= MergeArrayHandling.Merge ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    public MergeNullValueHandling MergeNullValueHandling
    {
      get => this._mergeNullValueHandling;
      set
      {
        this._mergeNullValueHandling = value >= MergeNullValueHandling.Ignore && value <= MergeNullValueHandling.Merge ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }
  }
}
