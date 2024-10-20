namespace Battleship.Models
{
    public class Bateau
    {
        public char Lettre { get; }
        public int Taille { get; }
        public List<string> Positions { get; set; }

        public Bateau(char lettre, int taille)
        {
            Lettre = lettre;
            Taille = taille;
            Positions = new List<string>();
        }
    }
}
