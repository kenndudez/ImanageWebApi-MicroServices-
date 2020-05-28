using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Imanage.Shared.MessageTemplate
{
    public class MessageTemplateModel
    {
        public string MailTemplatePath { get; set; }

        public string MailBody { get; set; }

        public string Subject { get; set; }

        public string SubjectTemplate { get; set; }

        public List<Attachment> Attachements { get; set; }
        

    }
}
