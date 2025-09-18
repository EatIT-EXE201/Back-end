using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EatIT.Core.Entities
{
    public class Combo : BasicEntity<int>
    {
        public string? ComboDescription { get; set; }
        public string? ComboImg { get; set; }
        public int DishId { get; set; }

        public Dishes Dish { get; set; }
        public ICollection<Favorites> Favorites { get; set; }
    }
}
