using MTechSystems.EntityFramework.Context;
using MTechSystems.EntityFramework.Enums;
using MTechSystems.EntityFramework.Models;
using MTechSystems.Utilerias;
using MTechSystems.Vistas;
using log4net;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using LinqKit;
using Newtonsoft.Json;



namespace MTechSystems.VistasModelos
{
    public class EmployeeVM : ViewModel
    {



        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public AdminEmployeeList listEmployee;
        public EmployeeVM()
        {

            CerrarCommand = new RelayCommandParameter(obj => Cerrar((Window)obj));
            AgregarCommand = new RelayCommand(Agregar);
            GuardarCommand = new RelayCommandParameter(obj => Guardar((Window)obj));
            DesactivarCommand = new RelayCommandParameter(obj => Desactivar((Window)obj));
            ListEmployeesCommand = new RelayCommand(ListEmployees);
            ModelEmployee = new Employee();

            
        }

        public string borndate
        {
            get;
            set;
        }

        private bool windowIsEnabled;

        public bool WindowIsEnabled
        {
            get { return windowIsEnabled; }
            set
            {
                windowIsEnabled = value;
                OnPropertyChanged(nameof(WindowIsEnabled));
            }
        }


        private Employee modelEmployee;
        public Employee ModelEmployee
        {
            get { return modelEmployee; }
            set
            {
                modelEmployee = value;
                OnPropertyChanged(nameof(ModelEmployee));
            }
        }

        private ObservableCollection<Employee> employees;

        public ObservableCollection<Employee> Employees
        {
            get { return employees; }
            set
            {
                employees = value;
                OnPropertyChanged(nameof(Employees));
            }
        }


        public EmployeeStatus employeeStatus;
        public EmployeeStatus EmployeeStatus
        {
            get { return employeeStatus; }
            set
            {
                employeeStatus = value;
                OnPropertyChanged(nameof(EmployeeStatus));
            }
        }
       
        public void SerializeDB()
        {
            
            bool exists = System.IO.Directory.Exists(App.path);
            DateTime date = DateTime.Now;
            List<Employee> employees = new List<Employee>();
            if (!exists)
            {
                System.IO.Directory.CreateDirectory(App.path);
            }
            exists = System.IO.File.Exists(App.path + "backup.bin");
            if (exists)
            {
                System.IO.File.Move(App.path + "backup.bin", App.path + date.ToString("ddMMyyyy_HHmmssffff")+".bin");
            }
            using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    try
                    {

                        employees = new List<Employee>(context.Employees.OrderByDescending(c => c.EmployeeID));

                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        _ = MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

            BackUp<List<Employee>>.CrearArchivo(employees, App.path + "backup.bin");
        
        }


        public ObservableCollection<Employee> DeserializeDB()
        {
           
            ObservableCollection<Employee> employees = new ObservableCollection<Employee>();
            employees = BackUp<ObservableCollection<Employee>>.LeerArchivo(App.path + "backup.bin");
            return employees;
        }


        public void Desactivar(Window window)
        {
            _ = Task.Factory.StartNew(() =>
            {
                WindowIsEnabled = false;

                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    try
                    {
                        _ = context.Set<Employee>().Add(ModelEmployee);

                        if ((EmployeeStatus)ModelEmployee.EmployeeStatus == EmployeeStatus.Active)
                        {
                            context.Entry(ModelEmployee).State = EntityState.Modified;
                            ModelEmployee.EmployeeStatus = EmployeeStatus.Inactive;

                        }
                        else
                        {
                            context.Entry(ModelEmployee).State = EntityState.Modified;
                            ModelEmployee.EmployeeStatus = EmployeeStatus.Active;
                        }

                        _ = context.SaveChanges();

                        Cerrar(window);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        _ = MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    finally
                    {
                        context.Entry(ModelEmployee).State = EntityState.Detached;
                    }
                }

                WindowIsEnabled = true;
            });

        }
        public RelayCommandParameter DesactivarCommand { get; private set; }

       
        static private bool EsVocal(char letra)
        {
            
            if (letra == 'A' || letra == 'E' || letra == 'I' || letra == 'O' || letra == 'U' ||
                letra == 'a' || letra == 'e' || letra == 'i' || letra == 'o' || letra == 'u')
                return true;
            else
                return false;
        }
        public bool VerifyRFC(string employeename, string employeelastname, DateTime? borndate, string employeeRFC)
        {
            List<Employee> employeelist = new List<Employee>();
            string[] Apellidos = employeelastname.Split();
            string apellidop = Apellidos[0];
            string RFC = apellidop.Substring(0, 1);
            foreach (char c in apellidop.Substring(1))
            {
                if (EsVocal(c))
                {
                    RFC += c;
                    break;
                }
            }
            string apellidom = Apellidos[1].Substring(0, 1);
            string name = employeename.Substring(0, 1);
            string bornyear = borndate.Value.ToString("yy");
            string bornmonth = borndate.Value.ToString("MM");
            string bornday = borndate.Value.ToString("dd");
            RFC += apellidom + name + bornyear + bornmonth + bornday;
            RFC = RFC.ToUpper();
            
            if (RFC != employeeRFC.Substring(0, 10) || employeeRFC.Length < 13 )
            {
                string message = "The RFC is incorrect, please verify the input characters, the first part should look like this: " + RFC + "XXX where 'XXX' stands for the unique pattern assignated by STA. The RFC should contain 13 characters.";
                MessageBox.Show(message);
                return false;
            }
            else
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    try
                    {
                        employeelist = new List<Employee>(context.Employees.Where(c => c.EmployeeRFC == employeeRFC));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        _ = MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                if (employeelist.Any())
                {
                    string message = "The RFC already exists in the database, please verify your input.";
                    MessageBox.Show(message);
                    return false;
                }
                else
                {
                    employeeRFC = employeeRFC.ToUpper();
                    return true;
                }
                

            }
        }

        public void Guardar(Window window)
        {
            _ = Task.Factory.StartNew(() =>
            {
                if (VerifyRFC(ModelEmployee.EmployeeName, ModelEmployee.EmployeeLastName, ModelEmployee.EmployeeBornDate, ModelEmployee.EmployeeRFC))
                {

                    WindowIsEnabled = false;



                    using (ApplicationDbContext context = new ApplicationDbContext())
                    {
                        try
                        {
                            _ = context.Set<Employee>().Add(ModelEmployee);




                            if (context.Employees.Any())
                            {
                                ModelEmployee.EmployeeID = context.Employees.Max(c => c.EmployeeID) + 1;
                            }
                            else
                            {
                                ModelEmployee.EmployeeID = 1;
                            }
                            ModelEmployee.EmployeeStatus = EmployeeStatus.Active;


                            context.Entry(ModelEmployee).State = EntityState.Added;






                            _ = context.SaveChanges();

                            Cerrar(window);
                        }
                        catch (Exception ex)

                        {
                            Log.Error(ex);
                            _ = MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                        finally
                        {
                            context.Entry(ModelEmployee).State = EntityState.Detached;
                        }
                    }

                    WindowIsEnabled = true;

                }
                else
                {
                    WindowIsEnabled = true;
                }
            });
        }
        public RelayCommandParameter GuardarCommand { get; private set; }

       

        public void Cerrar(Window window)
        {
            window.Dispatcher.Invoke(() =>
            {
                window.Close();
            });
        }


        public RelayCommandParameter CerrarCommand { get; private set; }

        public void Agregar()
        {
            AdminEmployeeForm form = new AdminEmployeeForm();
            form.Closed += Form_Closed;
            form.Show();
        }
        private void Form_Closed(object sender, EventArgs e)
        {
            ListEmployees();
        }
        public RelayCommand AgregarCommand { get; private set; }




        public void ListEmployees()
        {
            _ = Task.Factory.StartNew(() =>
            {
                using (ApplicationDbContext context = new ApplicationDbContext())
                {
                    try
                    {
                        Employees = new ObservableCollection<Employee>(context.Employees.OrderByDescending(c => c.EmployeeBornDate));
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex);
                        _ = MessageBox.Show(ex.Message, "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            });
        }
        public void Filter_Employees(string employeename)
        {
            string[] nombres;
            try
            {
                nombres = employeename.Split();
                var predicate = PredicateBuilder.New<Employee>();
                foreach (string nombre in nombres)
                {
                    string temp = nombre;
                    predicate = predicate.And(n => n.EmployeeName.Contains(temp) || n.EmployeeLastName.Contains(temp));
                }
                using (var contexto = new ApplicationDbContext())
                {

                    Employees = new ObservableCollection<Employee>(contexto.Employees.Where(predicate).OrderByDescending(c => c.EmployeeBornDate).ToList());

                }
            }
            catch (Exception ex)
            {

                Log.Error(ex);
            }

        }
        public RelayCommand ListEmployeesCommand { get; private set; }

    }
}

