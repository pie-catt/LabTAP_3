using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace TinyDependencyInjectionContainer {

    public class InterfaceResolver {
        //Associazioni Interfaccia -> Classe che la implementa
        private readonly Dictionary<Type, Type> _intClassAssociation = new Dictionary<Type, Type>();

        public InterfaceResolver(string filename) {
            //Controllo se file di configurazione null o non esiste
            if (filename == null) throw new ArgumentNullException(nameof(filename));
            if (!File.Exists(filename)) throw new FileNotFoundException(filename);

            //Leggo linee del file
            var fileLines = File.ReadAllLines(filename);

            foreach (var line in fileLines) {
                //Ignoro linee commenti
                if (line.StartsWith("#")) continue;
                //Divido linea nei token
                var tokens = line.Split('*');

                if (tokens.Length != 4)
                    throw new ConfigFileException("Invalid configuration file format");

                if (!File.Exists(tokens[0]) ||
                    !File.Exists(tokens[2]))
                    throw new FileNotFoundException("Assembly File not found");

                //Carico Assembly per definizione interfaccia
                var assemblyInt = Assembly.LoadFrom(tokens[0]);
                //Controllo se interfaccia presente in assembly
                if (assemblyInt.GetType(tokens[1]) == null)
                    throw new ConfigFileException("Interface" + tokens[1] + " not in Assembly file");

                //Carico Assembly per implementazione interfaccia
                var assemblyClass = Assembly.LoadFrom(tokens[2]);
                //Controllo se classe presente in assembly
                if (assemblyClass.GetType(tokens[3]) == null)
                    throw new ConfigFileException("Class" + tokens[3] + " not in Assembly file");

                //Associo interfaccia alla classe che la implementa
                _intClassAssociation.Add(assemblyInt.GetType(tokens[1]), 
                    assemblyClass.GetType(tokens[3]));
            }
        }

        public T Instantiate<T>() where T : class {
            //Se tipo T non presente nell'associazione restituisco null
            if (!_intClassAssociation.TryGetValue(typeof(T), out var type))
                return null;

            var typeInstance = Activator.CreateInstance(type);
                return (T)typeInstance;
        }

    }

    public class ConfigFileException : Exception {
        public ConfigFileException(string message)
            : base(message) {
        }
    }

}
