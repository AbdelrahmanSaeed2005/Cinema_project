using CinemaHub.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CinemaHub.ViewModels
{
    public class MovieFormVM
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public double? Price { get; set; }
        public string ImageUrl { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public MovieStatus MovieStatus { get; set; }

        public int CategoryId { get; set; }
        public int CinemaId { get; set; }
        //vre imbortent
        public IEnumerable<SelectListItem>? Categories { get; set; }
        public IEnumerable<SelectListItem>? Cinemas { get; set; }
     
        public List<int> SelectedActorIds { get; set; } = new();
        public IEnumerable<SelectListItem> Actors { get; set; } = Enumerable.Empty<SelectListItem>();

    }
}
