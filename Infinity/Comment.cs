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
    
    public partial class Comment
    {
        public int Id { get; set; }
        public int UserRef { get; set; }
        public Nullable<int> SPRef { get; set; }
        public Nullable<int> OfferableRef { get; set; }
        public bool IsFavorited { get; set; }
        public Nullable<bool> IsShared { get; set; }
        public Nullable<int> BadContentTypeRef { get; set; }
        public System.DateTime InsertDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Status { get; set; }
        public string Comment1 { get; set; }
        public Nullable<bool> IsLiked { get; set; }
        public Nullable<bool> IsDisliked { get; set; }
        public string ShareToWhom { get; set; }
    
        public virtual BadContentType BadContentType { get; set; }
        public virtual Offerable Offerable { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual User User { get; set; }
    }
}
