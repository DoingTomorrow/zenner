// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonLoadSettings
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;

#nullable disable
namespace Newtonsoft.Json.Linq
{
  public class JsonLoadSettings
  {
    private CommentHandling _commentHandling;
    private LineInfoHandling _lineInfoHandling;

    public CommentHandling CommentHandling
    {
      get => this._commentHandling;
      set
      {
        this._commentHandling = value >= CommentHandling.Ignore && value <= CommentHandling.Load ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }

    public LineInfoHandling LineInfoHandling
    {
      get => this._lineInfoHandling;
      set
      {
        this._lineInfoHandling = value >= LineInfoHandling.Ignore && value <= LineInfoHandling.Load ? value : throw new ArgumentOutOfRangeException(nameof (value));
      }
    }
  }
}
