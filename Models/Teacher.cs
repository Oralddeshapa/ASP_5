using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using TeaChair.ViewModels;

namespace TeaChair.Models
{
    public class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Subject { get; set; }
        public string Tier { get; set; }

        public int points { get; set; }

        public Teacher() { }
        public Teacher(TeacherViewModel tea)
        {
            Id = tea.Id;
            Name = tea.Name;
            ReleaseDate = tea.ReleaseDate;
            Subject = tea.Subject;
            Tier = tea.Tier;
            points = tea.points + tea.New_points;
        }
    }
}
