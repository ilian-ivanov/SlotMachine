namespace SlotMachine.Models
{
    public interface IRandomWithProbabilities
    {
        Symbol Draw(List<Symbol> symbols);
    }
}