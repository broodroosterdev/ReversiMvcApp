namespace ReversiMvcApp.Schemas
{
    public class Game
    {
        //het unieke token van het spel
        public string Token { get; set; }
        public string Description { get; set; }
        public string? Player1Token { get; set; }
        public string? Player2Token { get; set; }
        public Color[,] Board { get; set; }   
        public Color Turn { get; set; }   
    }
}