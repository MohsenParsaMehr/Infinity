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
    
    public partial class Off
    {
        public int Id { get; set; }
        public Nullable<int> CustomerRoleRef { get; set; }
        public decimal Price { get; set; }
        public Nullable<System.DateTime> ValidFrom { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
        public int OfferableRef { get; set; }
    
        public virtual Offerable Offerable { get; set; }
        public virtual Role Role { get; set; }
    }
}