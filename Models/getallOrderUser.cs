//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace E_Commerse_Store.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class getallOrderUser
    {
        public int InvoiceId { get; set; }
        public string User { get; set; }
        public string pro_Name { get; set; }
        public Nullable<int> Bill { get; set; }
        public string Payment { get; set; }
        public Nullable<System.DateTime> InvoiceDate { get; set; }
        public Nullable<byte> Status { get; set; }
    }
}