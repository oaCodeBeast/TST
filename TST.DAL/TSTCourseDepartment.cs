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
    
    public partial class TSTCourseDepartment
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public TSTCourseDepartment()
        {
            this.TSTCourses = new HashSet<TSTCours>();
        }
    
        public int CourseDepartmentID { get; set; }
        public string CourseDepartmentName { get; set; }
        public Nullable<bool> CourseDepartmentIsActive { get; set; }
        public string CourseDepartmentDescription { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<TSTCours> TSTCourses { get; set; }
    }
}
