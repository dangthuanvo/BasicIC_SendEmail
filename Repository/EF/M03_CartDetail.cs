//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Repository.EF
{
    using System;
    using System.Collections.Generic;
    
    public partial class M03_CartDetail
    {
        public System.Guid id { get; set; }
        public Nullable<System.Guid> cart_id { get; set; }
        public Nullable<System.Guid> product_id { get; set; }
        public Nullable<int> quantity { get; set; }
        public Nullable<decimal> cart_total_price { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public string create_by { get; set; }
        public Nullable<System.DateTime> modify_time { get; set; }
        public string modify_by { get; set; }
        public Nullable<System.Guid> tenant_id { get; set; }
    
        public virtual M03_Cart M03_Cart { get; set; }
        public virtual M03_Product M03_Product { get; set; }
    }
}