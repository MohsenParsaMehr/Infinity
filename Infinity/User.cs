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
    
    public partial class User
    {
        public User()
        {
            this.Address = new HashSet<Address>();
            this.BlackList = new HashSet<BlackList>();
            this.Comment = new HashSet<Comment>();
            this.LoginHistory = new HashSet<LoginHistory>();
            this.Offerable = new HashSet<Offerable>();
            this.Order = new HashSet<Order>();
            this.SearchHistory = new HashSet<SearchHistory>();
            this.UserRole = new HashSet<UserRole>();
        }
    
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string Status { get; set; }
        public System.DateTime InsertDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public Nullable<int> PersonRef { get; set; }
        public Nullable<int> SPRef { get; set; }
        public Nullable<int> ReferrerRef { get; set; }
        public Nullable<short> Reputation { get; set; }
        public Nullable<bool> SendPromotions { get; set; }
    
        public virtual ICollection<Address> Address { get; set; }
        public virtual ICollection<BlackList> BlackList { get; set; }
        public virtual ICollection<Comment> Comment { get; set; }
        public virtual ICollection<LoginHistory> LoginHistory { get; set; }
        public virtual ICollection<Offerable> Offerable { get; set; }
        public virtual ICollection<Order> Order { get; set; }
        public virtual Person Person { get; set; }
        public virtual ICollection<SearchHistory> SearchHistory { get; set; }
        public virtual ServiceProvider ServiceProvider { get; set; }
        public virtual ICollection<UserRole> UserRole { get; set; }
    }
}
