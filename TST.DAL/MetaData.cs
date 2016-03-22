using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TST.DAL
{
    #region Employee and related meta data


    #region Employee
    public class TSTEmployeeMetaData
    {
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "First Name")]
        [StringLength(50,
            ErrorMessage = "* First name must be shorter than 50 characters.")]
        public string EmpFname { get; set; }


        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Last Name")]
        [StringLength(50,
            ErrorMessage = "* Last name must be shorter than 50 characters.")]
        public string EmpLname { get; set; }


        [Required(ErrorMessage = "* Required")]
        public int DepartmentID { get; set; }


        [Required(ErrorMessage = "* Required")]
        public int EmpStatusID { get; set; }

        [Display(Name = "Street Address 1")]
        [StringLength(100,
            ErrorMessage = "* Address must be shorter than 100 characters.")]
        public string EmpAddress1 { get; set; }
        [Display(Name = "Street Address 2")]
        [StringLength(100,
            ErrorMessage = "* Address must be shorter than 100 characters.")]
        public string EmpAddress2 { get; set; }
        [Display(Name = "City")]
        [StringLength(50,
            ErrorMessage = "* City must be less shorter 50 characters.")]
        public string EmpCity { get; set; }
        [Display(Name = "State")]
        [StringLength(2,
            ErrorMessage = "* Please enter the abbreviation for your state")]
        public string EmpState { get; set; }
        [Display(Name = "Photo")]
        public string EmpPhoto { get; set; }
        [Display(Name = "User ID")]
        public string EmpUserID { get; set; }


        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Date of Birth")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        public System.DateTime EmpDateOfBirth { get; set; }

        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Date of Hire")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        public System.DateTime EmpDateOfHire { get; set; }
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = false)]
        public string EmpEndDate { get; set; }
        [Display(Name = "Phone Number")]
        [Required(ErrorMessage = "* Required")]
        public string EmpPhone { get; set; }

        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "* Email is invalid")]
        public string EmpEmail { get; set; }
        [Display(Name = "Notes")]
        [UIHint("MultilineText")]
        [StringLength(400,
            ErrorMessage = "* Notes must be less shorter 400 characters.")]
        public string EmpNotes { get; set; }
    }
    [MetadataType(typeof(TSTEmployeeMetaData))]
    public partial class TSTEmployee
    {
        public string FullName
        {
            get
            {
                return EmpFname + " " + EmpLname;

            }
        }

    }
    #endregion

    #region EmployeeStatus

    public class TSTEmployeeStatusMetaData
    {
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Status Name")]
        [StringLength(50,
        ErrorMessage = "Status name must be shorter than 50 characters.")]
        public string EmpStatusName { get; set; }

        [Display(Name = "Status Description")]
        [StringLength(140,
        ErrorMessage = "Status description must be shorter than 140 characters.")]
        public string EmpStatusDescription { get; set; }
    }
    [MetadataType(typeof(TSTEmployeeStatusMetaData))]
    public partial class TSTEmployeeStatus { }
    #endregion

    #region Department
    public class TSTDepartmentMetaData
    {
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Department Name")]
        [StringLength(25, ErrorMessage = "* Department name must be shorter than 25 characters.")]
        public string DepartmentName { get; set; }
        [Display(Name = "Department Description")]
        [StringLength(140, ErrorMessage = "* Department description must be shorter than 140 characters.")]
        public string DepartmentDescription { get; set; }
        public bool IsActive { get; set; }
    }
    [MetadataType(typeof(TSTDepartmentMetaData))]
    public partial class TSTDepartment
    {

    }
    #endregion
    #endregion

    #region Ticket and related meta data


    #region Ticket
    public class TSTTicketMetaData
    {
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Subject")]
        [StringLength(100,
         ErrorMessage = "* Subject must be shorter than 100 characters.")]
        public string TicketSubject { get; set; }
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Description")]
        [UIHint("MultilineText")]
        public string TicketDescription { get; set; }
        //This will be programatically assigned to get the ID of the person creating the ticket
        public int SubmittedByID { get; set; }
        //This will be in edit only
        public Nullable<int> TechID { get; set; }
        //This will be programatically set when ticket is opened
        public System.DateTime TicketSubmitted { get; set; }
        //This will be programatically set when ticket is resolved
        public Nullable<System.DateTime> TicketResolved { get; set; }
        //This will be programatically set to open by default
        public int TicketStatusID { get; set; }
        
        public string Image { get; set; }
        //This will be programatically set to low by default
        public int PriorityID { get; set; }
    }
    [MetadataType(typeof(TSTTicketMetaData))]
    public partial class TSTTicket
    {

    }

    #endregion

    #region TicketStatus
    public class TSTTicketStatusMetaData
    {
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Status Name")]
        [StringLength(25,
      ErrorMessage = "* Status name must be shorter than 25 characters.")]
        public string TicketStatusName { get; set; }
        [Display(Name = "Status Description")]
        [StringLength(140,
     ErrorMessage = "* Status description must be shorter than 140 characters.")]
        public string TicketStatusDescription { get; set; }
    }
    [MetadataType(typeof(TSTTicketStatusMetaData))]
    public partial class TSTTicketStatus
    {
       
    }
    #endregion

    #region Priority
    public class TSTPrioriteMetaData
    {
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Priority Name")]
        [StringLength(25,
ErrorMessage = "* Priority name must be shorter than 25 characters.")]
        public string PriorityName { get; set; }
        [Display(Name = "Priority Description")]
        [StringLength(140,
ErrorMessage = "* Priority description must be shorter than 140 characters.")]
        public string PriorityDescription { get; set; }

    }
    [MetadataType(typeof(TSTPrioriteMetaData))]
    public partial class TSTPriorite
    {

    }

    #endregion

    #region TechNote
    public class TSTTechNoteMetaData
    {
        //Programmatically set to the tech adding the technote
        public int TechID { get; set; }
        //programmatically set to the ticket that is getting the tech note
        public int TicketID { get; set; }
        [Required(ErrorMessage = "* Required")]
        [Display(Name = "Tech Note")]
        [StringLength(500,
ErrorMessage = "* Tech Note must be shorter than 140 characters.")]
        public string Notation { get; set; }
        //programmatically set to be the current time stamp
        public System.DateTime NotationDate { get; set; }
    }
    [MetadataType(typeof(TSTTechNoteMetaData))]
    public partial class TSTTechNote
    {

    }
    #endregion

    #endregion

    #region Course Meta data
    #endregion

}
