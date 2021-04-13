using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Web;

namespace Ely_Yohanna_Vega.Models
{
    public class FileViewModel
    {
        [Required]
        [DisplayName("Archivo:")]
        public HttpPostedFileBase archivo { get; set; }
    }
}