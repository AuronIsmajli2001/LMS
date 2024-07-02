using System;
using System.ComponentModel.DataAnnotations;

namespace LMS.Domain.Holidays
{
    public class Holiday
    {
        [Key]
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Name { get; set; }
    }
}
