using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GymSystemBLL.ViewModels.PlanViewModels
{
    public class UpdatePlanViewModel
    {
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Required !")]
        [StringLength(200,MinimumLength = 5, ErrorMessage = "You must Enter Description in Range 5 tp 200 Characters !")]
        public string Description { get; set; } = null!;

        [Required(ErrorMessage = "Required !")]
        [Range(1, 365, ErrorMessage = "You must Enter DurationDays in Range 1 to 365 !")]
        public int DurationDays { get; set; }

        [Required(ErrorMessage = "Required !")]
        [Range(1, 10000, ErrorMessage = "You must Enter Price in Range 1 to 10000 !")]
        public decimal Price { get; set; }
    }
}
