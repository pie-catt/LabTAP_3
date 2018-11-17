using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TinyDependencyInjectionContainer;
using EmailSenderInterfaces;

namespace EmailExecuter
{
    public class Executer
    {
        static void Main(string[] args)
        {
            try
            {
                var resolver = new InterfaceResolver("TDIC_Configuration.txt");
                var sender = resolver.Instantiate<IEmailSender>();
                sender.SendEmail("pietro@email.it", "Questa e' un email di prova");
                Console.ReadLine();
            }
            catch (FileFormatException e)
            {
                Console.WriteLine("Errore file di configurazione: {0}", e.Message);
                Console.ReadLine();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
