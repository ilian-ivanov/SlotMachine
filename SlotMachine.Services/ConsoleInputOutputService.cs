using SlotMachine.Models;
using System.Text;

namespace SlotMachine.Services
{
    public class ConsoleInputOutputService : IInputOutputService
    {
        public void ShowMessage(string message)
        {
            Console.WriteLine(message);
        }

        public string ReadInput()
        {
            return Console.ReadLine();
        }

        public void ShowGameResult(Symbol[,] field)
        {
            var result = new StringBuilder();

            for (int row = 0; row < field.GetLength(0); row++)
            {
                for (int col = 0; col < field.GetLength(1); col++)
                {
                    result.Append(field[row, col].Sign);
                }

                result.AppendLine();
            }

            Console.WriteLine();
            Console.WriteLine(result.ToString());
            Console.WriteLine();
        }
    }
}
