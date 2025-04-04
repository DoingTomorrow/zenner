// Decompiled with JetBrains decompiler
// Type: FluentNHibernate.Cfg.FluentConfigurationException
// Assembly: FluentNHibernate, Version=1.3.0.733, Culture=neutral, PublicKeyToken=8aa435e3cb308880
// MVID: 69C84109-3D3C-4837-B1CB-9C46FBBAE966
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\FluentNHibernate.dll

using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Security;

#nullable disable
namespace FluentNHibernate.Cfg
{
  [Serializable]
  public class FluentConfigurationException : Exception
  {
    public FluentConfigurationException(string message, Exception innerException)
      : base(message, innerException)
    {
      this.PotentialReasons = (IList<string>) new List<string>();
    }

    protected FluentConfigurationException(SerializationInfo info, StreamingContext context)
      : base(info, context)
    {
      this.PotentialReasons = (IList<string>) (info.GetValue(nameof (PotentialReasons), typeof (List<string>)) as List<string>);
    }

    public IList<string> PotentialReasons { get; private set; }

    public override string Message
    {
      get
      {
        string message = base.Message + Environment.NewLine + Environment.NewLine;
        foreach (string potentialReason in (IEnumerable<string>) this.PotentialReasons)
        {
          message = message + "  * " + potentialReason;
          message += Environment.NewLine;
        }
        return message;
      }
    }

    public override string ToString()
    {
      string str = base.ToString() + Environment.NewLine + Environment.NewLine;
      foreach (string potentialReason in (IEnumerable<string>) this.PotentialReasons)
      {
        str = str + "  * " + potentialReason;
        str += Environment.NewLine;
      }
      return str;
    }

    [SecurityCritical]
    public override void GetObjectData(SerializationInfo info, StreamingContext context)
    {
      base.GetObjectData(info, context);
      info.AddValue("PotentialReasons", (object) this.PotentialReasons, typeof (List<string>));
    }
  }
}
