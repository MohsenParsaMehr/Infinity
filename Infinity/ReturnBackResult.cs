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
    
    public partial class ReturnBackResult
    {
        public ReturnBackResult()
        {
            this.TransactionDetail = new HashSet<TransactionDetail>();
        }
    
        public int Id { get; set; }
        public int ReturnBackRef { get; set; }
        public string Answer { get; set; }
        public bool IsAccepted { get; set; }
        public System.DateTime InsertDate { get; set; }
        public Nullable<System.DateTime> UpdateDate { get; set; }
        public string Status { get; set; }
        public int OperatorUserRef { get; set; }
    
        public virtual ReturnBack ReturnBack { get; set; }
        public virtual ICollection<TransactionDetail> TransactionDetail { get; set; }
    }
}
