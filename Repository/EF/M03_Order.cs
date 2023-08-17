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
    
    public partial class M03_Order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public M03_Order()
        {
            this.M03_CouponOrder = new HashSet<M03_CouponOrder>();
            this.M03_OrderDetail = new HashSet<M03_OrderDetail>();
        }
    
        public System.Guid id { get; set; }
        public Nullable<System.Guid> customer_id { get; set; }
        public Nullable<System.Guid> employee_id { get; set; }
        public Nullable<System.Guid> addresses_id { get; set; }
        public string customer_phone_number { get; set; }
        public Nullable<decimal> total_price { get; set; }
        public Nullable<decimal> total_price_coupon { get; set; }
        public Nullable<int> payment_method { get; set; }
        public Nullable<System.DateTime> arrived_date { get; set; }
        public string shipping_address { get; set; }
        public Nullable<int> shipping_status { get; set; }
        public Nullable<int> shipping_fee { get; set; }
        public Nullable<int> order_payment_collection { get; set; }
        public string note { get; set; }
        public Nullable<System.DateTime> create_time { get; set; }
        public string create_by { get; set; }
        public Nullable<System.DateTime> modify_time { get; set; }
        public string modify_by { get; set; }
        public Nullable<System.Guid> tenant_id { get; set; }
    
        public virtual M03_Address M03_Address { get; set; }
        public virtual M03_Address M03_Address1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M03_CouponOrder> M03_CouponOrder { get; set; }
        public virtual M03_Customer M03_Customer { get; set; }
        public virtual M03_Employee M03_Employee { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<M03_OrderDetail> M03_OrderDetail { get; set; }
    }
}
