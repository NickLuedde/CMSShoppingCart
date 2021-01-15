﻿using CMSShoppingCART.Models.Data;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace CMSShoppingCART.Models.ViewModels.Pages
{
    public class PageVM
    {
        public PageVM()
        {
        }

        public PageVM(PageDTO row)
        {
            Id = row.Id;
            Title = row.Title;
            Slug = row.Slug;
            Body = row.Body;
            Sorting = row.Sorting;
            HasSidebar = row.HasSidebar;
        }

        public int Id { get; set; }
        public string Title { get; set; }
        public string Slug { get; set; }

        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]
        [AllowHtml]
        public string Body { get; set; }

        public int Sorting { get; set; }
        public bool HasSidebar { get; set; }
    }
}