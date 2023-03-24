using log4net;
using MTechSystems.Utilerias;
using MTechSystems.VistasModelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MTechSystems.Vistas
{
    /// <summary>
    /// Lógica de interacción para AdminEmployeeList.xaml
    /// </summary>
    public partial class AdminEmployeeList : Window
    {

        public AdminEmployeeList()
        {
            InitializeComponent();

        }
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public string employeename = "";
        EmployeeVM ViewModel;

        private void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {

            
            employeename = TbxEmployeeName.Text;
            ViewModel = new EmployeeVM();
            
            DataContext = ViewModel;
            try
            {
                ViewModel.ListEmployees();
                ViewModel.Filter_Employees(employeename);
            }
            catch (Exception ex)
            {

                Log.Error(ex);
            }

        }
        private void TbxEmployeeName_GotFocus(object sender, RoutedEventArgs e)
        {
            TbxEmployeeName.Text = "";
        }
       
    }
}
