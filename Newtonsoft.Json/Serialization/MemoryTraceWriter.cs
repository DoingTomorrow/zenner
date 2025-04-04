// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.MemoryTraceWriter
// Assembly: Newtonsoft.Json, Version=9.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 607E95F7-8559-4986-90F9-68784B884761
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;

#nullable disable
namespace Newtonsoft.Json.Serialization
{
  public class MemoryTraceWriter : ITraceWriter
  {
    private readonly Queue<string> _traceMessages;

    public TraceLevel LevelFilter { get; set; }

    public MemoryTraceWriter()
    {
      this.LevelFilter = TraceLevel.Verbose;
      this._traceMessages = new Queue<string>();
    }

    public void Trace(TraceLevel level, string message, Exception ex)
    {
      if (this._traceMessages.Count >= 1000)
        this._traceMessages.Dequeue();
      StringBuilder stringBuilder = new StringBuilder();
      stringBuilder.Append(DateTime.Now.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fff", (IFormatProvider) CultureInfo.InvariantCulture));
      stringBuilder.Append(" ");
      stringBuilder.Append(level.ToString("g"));
      stringBuilder.Append(" ");
      stringBuilder.Append(message);
      this._traceMessages.Enqueue(stringBuilder.ToString());
    }

    public IEnumerable<string> GetTraceMessages() => (IEnumerable<string>) this._traceMessages;

    public override string ToString()
    {
      StringBuilder stringBuilder = new StringBuilder();
      foreach (string traceMessage in this._traceMessages)
      {
        if (stringBuilder.Length > 0)
          stringBuilder.AppendLine();
        stringBuilder.Append(traceMessage);
      }
      return stringBuilder.ToString();
    }
  }
}
