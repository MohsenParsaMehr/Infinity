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
    
    public partial class OfferableTypeProperty
    {
        public int Id { get; set; }
        public int OfferableTypeRef { get; set; }
        public Nullable<short> Priority { get; set; }
        public int FieldRef { get; set; }
    
        public virtual Field Field { get; set; }
        public virtual OfferableType OfferableType { get; set; }
    }
}