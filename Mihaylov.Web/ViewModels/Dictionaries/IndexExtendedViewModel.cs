using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Mihaylov.Data.Models.Dictionaries;

namespace Mihaylov.Web.ViewModels.Dictionaries
{
    public class IndexExtendedViewModel : IndexViewModel
    {
        public Course Course { get; set; }
    }
}