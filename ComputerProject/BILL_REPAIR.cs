//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ComputerProject
{
    using System;
    using System.Collections.Generic;
    
    public partial class BILL_REPAIR
    {
        public int id { get; set; }
        public Nullable<System.DateTime> timeDelivery { get; set; }
        public System.DateTime timeReceive { get; set; }
        public string desReceiveItems { get; set; }
        public string desProblem { get; set; }
        public Nullable<int> price { get; set; }
        public Nullable<int> customerMoney { get; set; }
        public bool isWarranty { get; set; }
        public int status { get; set; }
        public int customerId { get; set; }
        public Nullable<int> seriId { get; set; }
    
        public virtual CUSTOMER CUSTOMER { get; set; }
        public virtual ITEM_BILL_SERI ITEM_BILL_SERI { get; set; }
    }
}
