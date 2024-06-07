﻿using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class TehsilBlockViewModelEdit
    {
        public int Id { get; set; }
        [Required]
        public int LGD_ID { get; set; }
        [Required]
        [MaxLength(150, ErrorMessage = "Name cannot exceed 150 characters")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Division")]
        public int SelectedDivisionId { get; set; }
        public IEnumerable<SelectListItem> Divisions { get; set; }
        [Required]
        [Display(Name = "Sub-Division")]
        public int SelectedSubDivisionId { get; set; }
        public IEnumerable<SelectListItem> SubDivisions { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime UpdatedOn { get; set; }
    }
}
