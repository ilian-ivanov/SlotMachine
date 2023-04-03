using SlotMachine.Models;

namespace SlotMachine.Services
{
    public class GameService : IGameService
    {
        private readonly IInputOutputService _inputOutputService;
        private readonly ISlotMachine _slotMachine;


        public GameService(
            IInputOutputService inputOutputService,
            ISlotMachine slotMachine
            )
        {
            _inputOutputService = inputOutputService;
            _slotMachine = slotMachine;
        }

        public void Start()
        {
            _slotMachine.CurrentBalance = Deposit();

            while (_slotMachine.CurrentBalance != 0)
            {
                try
                {
                    _inputOutputService.ShowMessage("Enter stake amount:");
                    if (decimal.TryParse(_inputOutputService.ReadInput(), out decimal stake))
                    {
                        var result = _slotMachine.Play(stake);
                        _inputOutputService.ShowGameResult(result.Field);
                        _inputOutputService.ShowMessage($"You have won: {result.WinCoefficient * stake}");
                        _inputOutputService.ShowMessage($"Current balance is: {_slotMachine.CurrentBalance}");
                    }
                }
                catch (ArgumentException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    _inputOutputService.ShowMessage(ex.Message);
                }
            }

            _inputOutputService.ShowMessage("GAME OVER!");
        }

        private decimal Deposit()
        {
            _inputOutputService.ShowMessage("Please deposit money you would like to play with:");
            if (decimal.TryParse(_inputOutputService.ReadInput(), out var deposit))
            {
                if (deposit > 0)
                {
                    return deposit;
                }
                else
                {
                    _inputOutputService.ShowMessage("The deposit must be bigger than 0!");
                }
            }
            else
            {
                _inputOutputService.ShowMessage("Invalid decimal for deposit.");
            }

            return 0;
        }
    }
}