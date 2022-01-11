using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AllupStore.Areas.Admin.ViewModels
{
    public class CategoryCreateViewModel
    {
        [Required]
        public string Name { get; set; }

       
        public IFormFile File { get; set; }
        public bool IsMain { get; set; }
       
        public int ParentId { get; set; }

      


    }
}
