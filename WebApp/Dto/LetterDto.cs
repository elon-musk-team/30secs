using System;

namespace WebApp.Dto
{
    public class SymbolDto
    {
        public char Symbol { get; set; }

        public DateTime ShelfLife { get; set; }

        public SymbolReceiverDto Receiver { get; set; }
        
        public SymbolDto(char symbol)
        {
            Symbol = symbol;
            ShelfLife = DateTime.Now.AddSeconds(30);
        }

        public bool NeedDelete() => ShelfLife <= DateTime.Now;
    }
}