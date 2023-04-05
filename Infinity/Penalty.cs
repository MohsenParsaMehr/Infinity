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
    
    public partial class Penalty
    {
        public Penalty()
        {
            this.BadContentType = new HashSet<BadContentType>();
            this.BlackList = new HashSet<BlackList>();
            this.ForbiddenPhrase = new HashSet<ForbiddenPhrase>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public byte Severity { get; set; }
    
        public virtual ICollection<BadContentType> BadContentType { get; set; }
        public virtual ICollection<BlackList> BlackList { get; set; }
        public virtual ICollection<ForbiddenPhrase> ForbiddenPhrase { get; set; }
    }
}