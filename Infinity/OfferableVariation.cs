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
    
    public partial class OfferableVariation
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PriceRef { get; set; }
        public int OfferableRef { get; set; }
        public short Quantity { get; set; }
        public string Notes { get; set; }
        public Nullable<double> Weight { get; set; }
    
        public virtual Offerable Offerable { get; set; }
        public virtual Price Price { get; set; }
    }
}
