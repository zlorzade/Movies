using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Movies.Infrastructure
{

    public class GenreDto
    {
        [DisplayName("کد")]
        public double Code { get; set; }
        [DisplayName("اولویت")]
        public double Priority { get; set; }
        [DisplayName("نام")]
        public string Name { get; set; }
        [DisplayName("وضعیت")]
        public string Status { get; set; }
        [DisplayName("توضیحات")]
        public string description { get; set; }
        [DisplayName("اخرین تاریخ بروزرسانی")]
        public DateTime LastUpdatedDate { get; set; }

    }

}
