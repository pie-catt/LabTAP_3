using EmailSenderInterfaces;
using System;

namespace EmailSenderImplementation1 {
    public class AnEmailSender : IEmailSender {
        public bool SendEmail(string to, string body) {
            Console.WriteLine("Sending mail using implementation 1 to:" + 
                "\n\"{0}\"\n with body:\n\"{1}\"\n", to, body);
            return true;
        }
    }
}
