// Decompiled with JetBrains decompiler
// Type: Castle.Core.Smtp.DefaultSmtpSender
// Assembly: Castle.Core, Version=2.5.1.0, Culture=neutral, PublicKeyToken=407dd0808d44fbdc
// MVID: D1A7705D-6DF2-4847-8662-5721BEF57E6F
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\Castle.Core.dll

using Castle.Core.Internal;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Security.Permissions;

#nullable disable
namespace Castle.Core.Smtp
{
  public class DefaultSmtpSender : IEmailSender
  {
    private bool asyncSend;
    private readonly string hostname;
    private int port = 25;
    private int? timeout;
    private bool useSsl;
    private readonly NetworkCredential credentials = new NetworkCredential();

    public DefaultSmtpSender()
    {
    }

    public DefaultSmtpSender(string hostname) => this.hostname = hostname;

    public int Port
    {
      get => this.port;
      set => this.port = value;
    }

    public string Hostname => this.hostname;

    public bool AsyncSend
    {
      get => this.asyncSend;
      set => this.asyncSend = value;
    }

    public int Timeout
    {
      get => !this.timeout.HasValue ? 0 : this.timeout.Value;
      set => this.timeout = new int?(value);
    }

    public bool UseSsl
    {
      get => this.useSsl;
      set => this.useSsl = value;
    }

    public void Send(string from, string to, string subject, string messageText)
    {
      if (from == null)
        throw new ArgumentNullException(nameof (from));
      if (to == null)
        throw new ArgumentNullException(nameof (to));
      if (subject == null)
        throw new ArgumentNullException(nameof (subject));
      if (messageText == null)
        throw new ArgumentNullException(nameof (messageText));
      this.Send(new MailMessage(from, to, subject, messageText));
    }

    public void Send(MailMessage message) => this.InternalSend(message);

    private void InternalSend(MailMessage message)
    {
      if (message == null)
        throw new ArgumentNullException(nameof (message));
      if (this.asyncSend)
      {
        SmtpClient smtpClient;
        if (string.IsNullOrEmpty(this.hostname))
        {
          smtpClient = new SmtpClient();
        }
        else
        {
          smtpClient = new SmtpClient(this.hostname, this.port);
          this.Configure(smtpClient);
        }
        Guid msgGuid = new Guid();
        SendCompletedEventHandler sceh = (SendCompletedEventHandler) null;
        sceh = (SendCompletedEventHandler) ((sender, e) =>
        {
          if (msgGuid == (Guid) e.UserState)
            message.Dispose();
          smtpClient.SendCompleted -= sceh;
        });
        smtpClient.SendCompleted += sceh;
        smtpClient.SendAsync(message, (object) msgGuid);
      }
      else
      {
        using (message)
        {
          SmtpClient smtpClient = new SmtpClient(this.hostname, this.port);
          this.Configure(smtpClient);
          smtpClient.Send(message);
        }
      }
    }

    public void Send(IEnumerable<MailMessage> messages)
    {
      foreach (MailMessage message in messages)
        this.Send(message);
    }

    public string Domain
    {
      get => this.credentials.Domain;
      set => this.credentials.Domain = value;
    }

    public string UserName
    {
      get => this.credentials.UserName;
      set => this.credentials.UserName = value;
    }

    public string Password
    {
      get => this.credentials.Password;
      set => this.credentials.Password = value;
    }

    protected virtual void Configure(SmtpClient smtpClient)
    {
      smtpClient.Credentials = (ICredentialsByHost) null;
      if (DefaultSmtpSender.CanAccessCredentials() && this.HasCredentials)
        smtpClient.Credentials = (ICredentialsByHost) this.credentials;
      if (this.timeout.HasValue)
        smtpClient.Timeout = this.timeout.Value;
      if (!this.useSsl)
        return;
      smtpClient.EnableSsl = this.useSsl;
    }

    private bool HasCredentials => !string.IsNullOrEmpty(this.credentials.UserName);

    private static bool CanAccessCredentials()
    {
      return new SecurityPermission(SecurityPermissionFlag.UnmanagedCode).IsGranted();
    }
  }
}
