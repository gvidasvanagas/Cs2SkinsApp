namespace Cs2SkinsApp.Models
{
    public class Skin
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;


        public RarityInfo? Rarity { get; set; }

        public bool Stattrak { get; set; }
        public bool Souvenir { get; set; }

        public string Image { get; set; } = string.Empty;
    }


    public class RarityInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Color { get; set; } = string.Empty;
    }
}
