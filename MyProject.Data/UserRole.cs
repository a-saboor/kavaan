//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyProject.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserRole
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UserRole()
        {
            this.UserRolePrivileges = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges1 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges2 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges3 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges4 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges5 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges6 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges7 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges8 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges9 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges10 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges11 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges12 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges13 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges14 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges15 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges16 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges17 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges18 = new HashSet<UserRolePrivilege>();
            this.UserRolePrivileges19 = new HashSet<UserRolePrivilege>();
            this.Users = new HashSet<User>();
        }
    
        public long ID { get; set; }
        public string RoleName { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges2 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges3 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges4 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges5 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges6 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges7 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges8 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges9 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges10 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges11 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges12 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges13 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges14 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges15 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges16 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges17 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges18 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UserRolePrivilege> UserRolePrivileges19 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<User> Users { get; set; }
    }
}