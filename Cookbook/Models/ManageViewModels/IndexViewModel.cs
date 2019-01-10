using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Cookbook.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public string Username { get; set; }

        [EmailAddress]
        public string Email { get; set; }

        public string StatusMessage { get; set; }

        public string Language { get; set; }
        public string Theme { get; set; }
    }
}
