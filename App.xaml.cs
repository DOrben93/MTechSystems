using log4net;
using MTechSystems.EntityFramework.Context;
using MTechSystems.Utilerias;
using MTechSystems.VistasModelos;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;

namespace MTechSystems
{
    /// <summary>
    /// Lógica de interacción para App.xaml
    /// </summary>
    public partial class App : Application
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public EmployeeVM employeeVM = new EmployeeVM();
        public LoadCatalog loadCatalog = new LoadCatalog();
        public static string path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName + LecturaAppConfig.LACSystem.GetString("PATH_BACKUP_DB");
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            try
            {   

                TestConexionBD();
            }
            catch(Exception ex)
            {
                Log.Error("System initialization failure", ex);
            }
            finally
            {
                if (File.Exists(path + "backup.bin"))
                {
                    loadCatalog.BorrarBase();
                    loadCatalog.LoadCat();
                }

                    MTechSystems.Vistas.AdminEmployeeList window = new MTechSystems.Vistas.AdminEmployeeList();
                    window.Show();

               
            }


        }

        private void Application_Exit(object sender, ExitEventArgs e)
        {
            employeeVM.SerializeDB();
            Log.Info("Finalizó el programa");
        }
        public void TestConexionBD()
        {
            
            using (ApplicationDbContext context = new ApplicationDbContext())
            {
                context.Database.Initialize(false);

            }

        }
    }
}
