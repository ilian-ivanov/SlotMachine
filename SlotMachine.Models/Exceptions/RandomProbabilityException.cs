namespace SlotMachine.Models.Exceptions
{
    public class RandomProbabilityException : Exception
    {
        public RandomProbabilityException() { }

        public RandomProbabilityException(string message)
            : base(message) { }
    }
}
