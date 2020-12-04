using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using TeaChair.Models;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace TeaChair.ViewModels
{
    public class TeacherViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Subject { get; set; }
        public string Tier { get; set; }
        public int points { get; set; }
        public int New_points { get; set; }
        public int Points_of_user { get; set; }

        public TeacherViewModel(Teacher tea)
        {
            Id = tea.Id;
            Name = tea.Name;
            ReleaseDate = tea.ReleaseDate;
            Subject = tea.Subject;
            Tier = tea.Tier;
            points = tea.points;
            New_points = 0;
        }
        public TeacherViewModel()
        {

        }
    }
}
