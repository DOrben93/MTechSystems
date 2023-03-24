
using log4net;
using MTechSystems.EntityFramework.Context;
using MTechSystems.EntityFramework.Enums;
using MTechSystems.EntityFramework.Models;
using MTechSystems.VistasModelos;
using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace MTechSystems.Utilerias
{
    public class LoadCatalog
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
        private Employee Employee = new Employee();
        private EmployeeVM EVM = new EmployeeVM();
        public void BorrarBase()
        {
            ServiceController service = new ServiceController("SQLEXPRESS");

            try
            {
                using (var context = new ApplicationDbContext())
                {



                    context.Employees.Where(c => c.EmployeeStatus == EmployeeStatus.Active || c.EmployeeStatus == EmployeeStatus.Inactive || c.EmployeeStatus == EmployeeStatus.NotSet).DeleteFromQuery();


                }
                
            }
            catch (Exception ex)
            {

                Log.Error(ex);
            }

        }
        public void LoadCat()
        {
            _ = Task.Factory.StartNew(() =>
            {
                
                employees = EVM.DeserializeDB();

                 foreach(Employee employee in employees)
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {

                        try
                        {

                            context.Entry(Employee).State = EntityState.Added;


                            Employee.EmployeeName = employee.EmployeeName;
                            Employee.EmployeeLastName = employee.EmployeeLastName;
                            Employee.EmployeeBornDate = employee.EmployeeBornDate;
                            Employee.EmployeeRFC = employee.EmployeeRFC;
                            Employee.EmployeeID = employee.EmployeeID;
                            Employee.EmployeeStatus = employee.EmployeeStatus;

                            _ = context.SaveChanges();

                        }
                        catch (Exception ex)
                        {
                            Log.Error(ex);
                        }
                        finally
                        {

                            context.Entry(Employee).State = EntityState.Detached;
                        }

                }
               
            }

            });
        }
    }
}
