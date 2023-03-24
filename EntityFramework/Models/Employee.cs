using MTechSystems.EntityFramework.Enums;
using MTechSystems.VistasModelos;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MTechSystems.EntityFramework.Models
{
    [Table("Employees")]
    public class Employee : ViewModel
    {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Column("ID")]
        public int EmployeeID { get; set; }

        private string employeeName;
        [Column("Name")]
        public string EmployeeName
        {
            get { return employeeName; }
            set
            {
                employeeName = value;
                OnPropertyChanged(nameof(EmployeeName));
            }
        }

        private string employeeLastName;

        [Column("LastName")]
        public string EmployeeLastName
        {
            get { return employeeLastName; }
            set
            {
                employeeLastName = value;
                OnPropertyChanged(nameof(EmployeeLastName));
            }
        }
        private string employeeRFC;
        [StringLength(13)]
        [Index(IsUnique = true)]
        [Column("RFC")]
        public string EmployeeRFC
        {
            get { return employeeRFC; }
            set
            {
                employeeRFC = value;
                OnPropertyChanged(nameof(EmployeeRFC));
            }
        }

        private DateTime? employeeBornDate;
        [Column("BornDate")]
        public DateTime? EmployeeBornDate
        {
            get { return employeeBornDate; }
            set
            {
                employeeBornDate = value;
                OnPropertyChanged(nameof(EmployeeBornDate));
            }
        }

        public string Employee_BornDate
        {
            get
            {
                return EmployeeBornDate != null ? EmployeeBornDate.Value.ToString("dd/MM/yyyy") : "";
            }
        }

        public EmployeeStatus employeeStatus;
        [Column("EmployeeStatus")]
        public EmployeeStatus EmployeeStatus
        {
            get { return employeeStatus; }
            set
            {
                employeeStatus = value;
                OnPropertyChanged(nameof(EmployeeStatus));
            }
        }




    


    }
}
