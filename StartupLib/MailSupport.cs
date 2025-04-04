// Decompiled with JetBrains decompiler
// Type: StartupLib.MailSupport
// Assembly: StartupLib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=f5405c50fba4c3ca
// MVID: F485B12B-6718-4E49-AD83-1AB4C51945B5
// Assembly location: F:\tekst\DoingTomorrow\Zenner_Software\program_filer\StartupLib.dll

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

#nullable disable
namespace StartupLib
{
  internal class MailSupport
  {
    private readonly string Host = "192.168.0.199";
    private readonly string UserName = "web_notice@msh.org";
    private readonly string Password = "111111";
    private SmtpClient Client;
    public List<string> MailTo;
    public List<string> MailCopy;
    public string MailSubject;
    public string MailBody;

    public MailSupport()
    {
      this.MailTo = new List<string>();
      this.MailCopy = new List<string>();
      this.MailSubject = string.Empty;
      this.MailBody = string.Empty;
      this.Client = new SmtpClient();
      this.Client.DeliveryMethod = SmtpDeliveryMethod.Network;
      this.Client.Host = this.Host;
      this.Client.UseDefaultCredentials = true;
      this.Client.Credentials = (ICredentialsByHost) new NetworkCredential(this.UserName, this.Password);
    }

    private Task<MailMessage> RealizeMailMessage()
    {
      return Task.Run<MailMessage>((Func<MailMessage>) (() =>
      {
        try
        {
          MailMessage mailMessage = new MailMessage();
          mailMessage.From = new MailAddress(this.UserName, "GMM");
          foreach (string addresses in this.MailTo)
            mailMessage.To.Add(addresses);
          foreach (string addresses in this.MailCopy)
            mailMessage.CC.Add(addresses);
          mailMessage.Subject = this.MailSubject;
          mailMessage.Body = this.MailBody;
          mailMessage.BodyEncoding = Encoding.UTF8;
          mailMessage.IsBodyHtml = true;
          mailMessage.Priority = MailPriority.High;
          return mailMessage;
        }
        catch (Exception ex)
        {
          throw ex;
        }
      }));
    }

    public async Task<bool> SendMail()
    {
      try
      {
        SmtpClient smtpClient = this.Client;
        MailMessage message = await this.RealizeMailMessage();
        smtpClient.Send(message);
        smtpClient = (SmtpClient) null;
        message = (MailMessage) null;
        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}
