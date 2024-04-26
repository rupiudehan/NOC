﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Noc_App.Models.ViewModel
{
    public class ForwardApplicationViewModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string ApplicationID { get; set; }
        public string ApplicantName { get; set; }
        [Display(Name = "Applicant Email")]
        public string ApplicantEmailID { get; set; }
        public string ForwardToRole { get; set; }
        [Required]
        [Display(Name = "Officer")]
        public string SelectedOfficerId { get; set; }
        public IEnumerable<SelectListItem> Officers { get; set; }
        public string LocationDetails { get; set; }
        public double TotalArea { get; set; }
        public string LoggedInRole { get; set; }
        //[Required]
        [Display(Name = "Site Condition Report")]
        public IFormFile SiteConditionReportFile { get; set; }
        //[Required]
        [Display(Name = "Catchment Area & Flow")]
        public IFormFile CatchmentAreaFile { get; set; }
        //[Required]
        [Display(Name = "Distance From the Creek")]
        public IFormFile DistanceFromCreekFile { get; set; }
        //[Required]
        [Display(Name = "GIS Report/DWS Report")]
        public IFormFile GisOrDwsFile { get; set; }
        //[Required]
        [Display(Name = "KML File Report")]
        public IFormFile KmlFile { get; set; }
        //[Required]
        [Display(Name = "Cross-Section/Calculation Sheets")]
        public IFormFile CrossSectionOrCalculationFile { get; set; }
        //[Required]
        [Display(Name = "L-Section of the Drain if Sanctioned")]
        public IFormFile LSectionOfDrainFile { get; set; }
    }
}