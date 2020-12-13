using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace TeaChair.Models
{
    public class LogForDonate
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Teacher { get; set; }

        public int Points { get; set; }

        [DataType(DataType.Date)]
        public DateTime LogDate { get; set; }

    }
}
