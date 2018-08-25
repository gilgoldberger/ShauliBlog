using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace ClassExe3.Models
{
    public class Fan
    {
        [Key]
        public int ID { get; set; }
        [DisplayName("First Name")]
        public string firstName { get; set; }
        [DisplayName("Last Name")]
        public string lastName { get; set; }
        [DisplayName("Gender")]
        public Gender gender { get; set; }
        [DataType(DataType.Date)]
        [DisplayName("Birth Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime birthDate { get; set; }
        public string City { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        [DisplayName("Seniority")]
        public int seniority { get; set; }
    }
    public enum Gender { Female, Male }
}