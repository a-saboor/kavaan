//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MyProject.Data
{
    using System;
    using System.Collections.Generic;
    
    public partial class NotificationReceiver
    {
        public long ID { get; set; }
        public Nullable<long> ReceiverID { get; set; }
        public string ReceiverType { get; set; }
        public Nullable<long> NotificationID { get; set; }
        public Nullable<bool> IsSeen { get; set; }
        public Nullable<bool> IsDelivered { get; set; }
        public Nullable<bool> IsRead { get; set; }
    
        public virtual Notification Notification { get; set; }
    }
}