namespace Lancelittle.Models
{
    public class CharacterCantrip
    {
        public int CharacterId { get; set; }
        public Character Character { get; set; }
        public int CantripId { get; set; }
        public Cantrip Cantrip { get; set; }        
        
    }
}