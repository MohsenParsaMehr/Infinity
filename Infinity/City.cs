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
    
    public partial class City
    {
        public City()
        {
            this.SearchHistory = new HashSet<SearchHistory>();
            this.ServiceProvider = new HashSet<ServiceProvider>();
        }
    
        public int Id { get; set; }
        public string Title { get; set; }
        public string PrefixCode { get; set; }
        public int ProvinceRef { get; set; }
    
        public virtual ICollection<SearchHistory> SearchHistory { get; set; }
        public virtual ICollection<ServiceProvider> ServiceProvider { get; set; }
    }
}
