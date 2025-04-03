// Decompiled with JetBrains decompiler
// Type: Castle.Core.Smtp.IEmailSender
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using System.Collections.Generic;
using System.Net.Mail;

#nullable disable
namespace Castle.Core.Smtp
{
  public interface IEmailSender
  {
    void Send(string from, string to, string subject, string messageText);

    void Send(MailMessage message);

    void Send(IEnumerable<MailMessage> messages);
  }
}
