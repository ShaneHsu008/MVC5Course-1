using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVC5Course.ViewModels
{
    public class ClientBatchViewModel
    {
        [Required]
        public int ClientId { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "欄位長度不得大於 40 個字元")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "欄位長度不得大於 40 個字元")]
        public string MiddleName { get; set; }

        [Required]
        [StringLength(40, ErrorMessage = "欄位長度不得大於 40 個字元")]
        public string LastName { get; set; }
    }
}