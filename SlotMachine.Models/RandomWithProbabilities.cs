namespace SlotMachine.Models
{
    public class RandomWithProbabilities : IRandomWithProbabilities
    {
        public Symbol Draw(List<Symbol> symbols)
        {
            if (!IsProbabilitiesCorrect(symbols))
            {
                throw new ArgumentException("The probabilites of the symbols are not equal to 100%!");
            }

            var generator = new Random();
            var randNumber = generator.Next(100);
            var probability = 0;

            // on every cycle we add the probability of next symbol, in that way we add weight for every symbol - example:
            // iteration 1: from 0 to 44 - A
            // iteration 2: from 45 to 79 - B
            // iteration 3: from 80 to 94 - P
            // iteration 4: from 95 to 99 - *
            for (int i = 0; i < symbols.Count; i++)
            {
                probability += symbols[i].Probability;

                if (randNumber < probability)
                {
                    return symbols[i];
                }
            }

            return null;
        }

        private bool IsProbabilitiesCorrect(IList<Symbol> symbols)
        {
            int sumProbability = 0;

            foreach (var symbol in symbols)
            {
                sumProbability += symbol.Probability;
            }

            if (sumProbability != 100) return false;

            return true;
        }
    }
}
