//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infinity
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserRole
    {
        public int Id { get; set; }
        public int UserRef { get; set; }
        public int RoleRef { get; set; }
        public string Status { get; set; }
        public int OperatorUserRef { get; set; }
    
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}