using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EmailSenderInterfaces;

namespace EmailSenderImplementation1
{
    public class AnEmailSender : EmailSenderInterfaces.IEmailSender
    {
        public bool SendEmail(string to, string body)
        {
            Console.WriteLine("Sending mail using implementation 1 to:\n\"{0}\"\n with body:\n\"{1}\"\n", to, body);
            return true;
        }
    }
}
