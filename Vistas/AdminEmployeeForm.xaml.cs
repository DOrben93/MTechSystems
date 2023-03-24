using MTechSystems.EntityFramework.Models;

using MTechSystems.VistasModelos;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;

namespace MTechSystems.Vistas
{
    /// <summary>
    /// Lógica de interacción para SucursalForm.xaml
    /// </summary>
    public partial class AdminEmployeeForm : Window
    {
        
       
        public AdminEmployeeForm(Employee employee)
        {
            InitializeComponent();
            ((EmployeeVM)DataContext).ModelEmployee = employee;
           
           
        }
        public AdminEmployeeForm()
        {
            InitializeComponent();
            
        }
        private void Fecha_Seleccionada_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            var borndate = new DateTime();
            borndate = Fecha_Seleccionada.SelectedDate.Value.Date;
            ((EmployeeVM)DataContext).ModelEmployee.EmployeeBornDate = borndate;
        }


    }
}
