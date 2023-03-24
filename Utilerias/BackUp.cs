using log4net;
using MTechSystems.EntityFramework.Context;
using MTechSystems.EntityFramework.Enums;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace MTechSystems.Utilerias
{
    public static class BackUp<T>
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public static void CrearArchivo(T objeto, string archivo)
        {
            using (StreamWriter file = File.CreateText(archivo))
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(file, objeto);
            }
        }

        public static T LeerArchivo(string archivo)
        {
            string texto = File.ReadAllText(archivo);

            T objeto = JsonConvert.DeserializeObject<T>(texto);

            return objeto;
        }

        public static void Eliminar(string archivo)
        {
            File.Delete(archivo);
        }

       
    }

}