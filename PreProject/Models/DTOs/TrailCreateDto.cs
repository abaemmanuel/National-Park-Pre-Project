using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static PreProject.Models.Trail;

namespace PreProject.Models.DTOs
{
    public class TrailCreateDto
    {
        

            [Required]
            public string Name { get; set; }

            [Required]
            public double Distance { get; set; }


            public DifficultyType Difficulty { get; set; }

            [Required]
            public int NationalParkId { get; set; }

            [Required]
            public double Elevation { get; set; }

    }
}
