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
    
    public partial class Price
    {
        public Price()
        {
            this.OfferableVariation = new HashSet<OfferableVariation>();
        }
    
        public int Id { get; set; }
        public Nullable<decimal> PriceValue { get; set; }
        public Nullable<System.DateTime> ValidFrom { get; set; }
        public Nullable<System.DateTime> ValidTo { get; set; }
        public System.DateTime InsertDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<decimal> CoWorkerPrice { get; set; }
        public string Status { get; set; }
    
        public virtual ICollection<OfferableVariation> OfferableVariation { get; set; }
    }
}