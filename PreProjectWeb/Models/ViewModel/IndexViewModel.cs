using System.Collections.Generic;

namespace PreProjectWeb.Models.ViewModel
{
    public class IndexViewModel
    {
        public IEnumerable<NationalPark> NationalParkList { get; set; }
        public IEnumerable<Trail> TrailList { get; set; }  
    }
}
