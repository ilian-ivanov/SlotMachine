namespace SlotMachine.Models
{
    public class Symbol
    {
        public string Sign { get; set; }

        public decimal Coefficient { get; set; }

        public int Probability { get; set; }

        public bool IsWildCard { get; set; }
    }
}