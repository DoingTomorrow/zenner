// Decompiled with JetBrains decompiler
// Type: NLog.Internal.ISmtpClient
// Assembly: NLog, Version=4.0.0.0, Culture=neutral, PublicKeyToken=5120e14c03d0593c
// MVID: 3664C49D-045A-43B5-BD54-D3FA0228C0EB
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\NLog.dll

using System;
using System.Net;
using System.Net.Mail;

#nullable disable
namespace NLog.Internal
{
  internal interface ISmtpClient : IDisposable
  {
    SmtpDeliveryMethod DeliveryMethod { get; set; }

    string Host { get; set; }

    int Port { get; set; }

    int Timeout { get; set; }

    ICredentialsByHost Credentials { get; set; }

    bool EnableSsl { get; set; }

    void Send(MailMessage msg);

    string PickupDirectoryLocation { get; set; }
  }
}
