//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace TST.DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class TSTCourseInstance
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TSTCourseInstance()
        {
            this.TSTStudents_Courses = new HashSet<TSTStudents_Courses>();
        }
    
        public int CourseInstanceID { get; set; }
        public int CourseID { get; set; }
        public string CourseSemester { get; set; }
        public System.DateTime CourseYear { get; set; }
        public int CourseTeacherID { get; set; }
        public string CourseMeets { get; set; }
        public int CourseStatusID { get; set; }
    
        public virtual TSTCours TSTCours { get; set; }
        public virtual TSTCourseStatus TSTCourseStatus { get; set; }
        public virtual TSTEmployee TSTEmployee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TSTStudents_Courses> TSTStudents_Courses { get; set; }
    }
}