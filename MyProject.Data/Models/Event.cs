using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyProject.Data.Models
{
    [MetadataType(typeof(EventAttributes))]
    public partial class Event
    {
        // leave it empty.
    }

    public class EventAttributes
    {
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd MM yyyy}")]
        public Nullable<System.DateTime> EventDate { get; set; }
        // Your attribs will come here.
    }
}
