// Decompiled with JetBrains decompiler
// Type: ZENNER.CommonLibrary.Exceptions.InvalidJobException
// Assembly: CommonLibrary, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: 53447886-5C7B-49AE-B18C-3692A1E343CC
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\CommonLibrary.dll

using System;
using ZENNER.CommonLibrary.Entities;

#nullable disable
namespace ZENNER.CommonLibrary.Exceptions
{
  [Serializable]
  public class InvalidJobException : Exception
  {
    public Job Job { get; private set; }

    public InvalidJobException(Job job, string message)
      : base(message)
    {
      this.Job = job;
    }

    public InvalidJobException(Job job, Exception innerException)
      : base(innerException.Message, innerException)
    {
      this.Job = job;
    }
  }
}
