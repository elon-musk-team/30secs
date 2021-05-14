using System;

namespace WebApp.Dto
{
    public class LetterDto
    {
        public string Author { get; set; }

        public char Symbol { get; set; }

        public DateTime ShelfLife { get; set; }

        public LetterReceiverDto Receiver { get; set; }
        
        public LetterDto(string author, char symbol)
        {
            Author = author;
            Symbol = symbol;
            ShelfLife = DateTime.Now.AddSeconds(30);
        }

        public bool NeedDelete() => ShelfLife <= DateTime.Now;
    }
}