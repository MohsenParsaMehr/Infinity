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
    
    public partial class OfferableCategories
    {
        public OfferableCategories()
        {
            this.OfferableCategories1 = new HashSet<OfferableCategories>();
            this.SPActivityField = new HashSet<SPActivityField>();
            this.SPActivityField1 = new HashSet<SPActivityField>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public Nullable<int> ParentRef { get; set; }
        public string Status { get; set; }
        public string Notes { get; set; }
        public string PhotoUrl { get; set; }
        public string Keywords { get; set; }
        public Nullable<short> Priority { get; set; }
    
        public virtual ICollection<OfferableCategories> OfferableCategories1 { get; set; }
        public virtual OfferableCategories OfferableCategories2 { get; set; }
        public virtual ICollection<SPActivityField> SPActivityField { get; set; }
        public virtual ICollection<SPActivityField> SPActivityField1 { get; set; }
    }
}
