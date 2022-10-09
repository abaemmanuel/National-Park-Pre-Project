using Microsoft.AspNetCore.Mvc.Rendering;
using PreProjectWeb.Repository;
using System.Collections.Generic;

namespace PreProjectWeb.Models.ViewModel
{
    public class TrailsViewModel
    {
        public IEnumerable<SelectListItem> NationalParkList { get; set; }

        public Trail Trail { get; set; }
    }
}
