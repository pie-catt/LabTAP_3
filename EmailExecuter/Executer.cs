using EmailSenderInterfaces;
using System;
using System.IO;
using TinyDependencyInjectionContainer;

namespace EmailExecuter {
    public class Executer {
        private static void Main() {
            try {
                var resolver = new InterfaceResolver("TDIC_Configuration.txt");
                var sender = resolver.Instantiate<IEmailSender>();
                sender.SendEmail("pietro@email.it", "Hi! This is an email");
                Console.ReadLine();
            }
            catch (FileNotFoundException e) {
                Console.WriteLine("File not found: " + e.Message);
                Console.ReadLine();
            }
            catch (ConfigFileException e) {
                Console.WriteLine("Config file error: " + e.Message);
                Console.ReadLine();
            }
            catch (MissingMethodException e) {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
            catch (Exception e) {
                Console.WriteLine(e.Message);
                Console.ReadLine();
            }
        }
    }
}
