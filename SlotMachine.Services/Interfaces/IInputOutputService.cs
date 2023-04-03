using SlotMachine.Models;

namespace SlotMachine.Services
{
    public interface IInputOutputService
    {
        void ShowMessage(string message);
        
        string ReadInput();

        void ShowGameResult(Symbol[,] field);
    }
}