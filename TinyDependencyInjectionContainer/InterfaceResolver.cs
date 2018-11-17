using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TinyDependencyInjectionContainer
{
    public class InterfaceResolver
    {
        //Associazioni Interfaccia -> Classe che la implementa
        private readonly Dictionary<Type, Type> _intClassAssociation = new Dictionary<Type, Type>();

        public InterfaceResolver(string filename)
        {
            //Controllo se file di configurazione null o non esiste
            if(filename == null) throw new ArgumentNullException(nameof(filename));
            if(!File.Exists(filename)) throw new FileNotFoundException(filename);

            //Leggo linee del file
            var fileLines = File.ReadAllLines(filename);

            foreach (var line in fileLines)
            {
                //Ignoro linee commenti
                if (!line.StartsWith("#"))
                {
                    //Divido linea 
                    var tokens = line.Split('*');

                    if (tokens.Length != 4) throw new FileFormatException("Formato file configurazione non valido");

                    if (!File.Exists(tokens[0]) ||
                        !File.Exists(tokens[2])) throw new FileNotFoundException("File assembly non trovato");

                    //Carico Assembly per definizione interfaccia
                    var _assemblyInt = Assembly.LoadFrom(tokens[0]);
                    //Controllo se interfaccia presente in assembly
                    if ((_assemblyInt.GetType(tokens[1])) == null)
                        throw new FileFormatException("Interfaccia" + tokens[1] + " non presente in assembly");

                    //Carico Assembly per implementazione interfaccia
                    var _assemblyClass = Assembly.LoadFrom(tokens[2]);
                    //Controllo se classe presente in assembly
                    if ((_assemblyClass.GetType(tokens[3])) == null)
                        throw new FileFormatException("Classe" + tokens[3] + " non presente in Assembly");

                    //Associo interfaccia alla classe che la implementa
                    _intClassAssociation.Add(_assemblyInt.GetType(tokens[1]), _assemblyClass.GetType(tokens[3]));
                }
            }
        }

        public T Instantiate<T>() where T : class
        {
            Type valueS;
            _intClassAssociation.TryGetValue(typeof(T), out valueS);
            var ex = Activator.CreateInstance(valueS);
            return (T)ex;
        }

    }

    public class FileFormatException : Exception
    {
        public FileFormatException(string message)
            : base(message)
        { 
        }
    }

}
