using Villa_API.Models.DTO;

namespace Villa_API.Data
{
    public static class VillaStore
    {
       public static List<VillaDTO> villaList = new List<VillaDTO>
            {
                new VillaDTO { Id = 1, Name = "Pool View", Occupancy = 100, Sqft = 12},
                new VillaDTO { Id = 2, Name = "Beach View", Occupancy = 200, Sqft = 8},
                new VillaDTO { Id = 3, Name = "Badroom View", Occupancy = 300, Sqft = 7 }

            };
    }
}
    