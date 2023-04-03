using Microsoft.Extensions.Options;

namespace SlotMachine.Models
{
    public class SlotMachine : ISlotMachine
    {
        private readonly SlotMachineConfiguration _configuration;
        private readonly IRandomWithProbabilities _randomWithProbabilities;
        private decimal _currentBalance = 0;
        private Symbol[,] _field;
        private readonly List<Symbol> _symbols;
        private readonly int _rows;
        private readonly int _cols;

        public decimal CurrentBalance
        {
            get { return _currentBalance; }
            set
            {
                if (value < 0)
                {
                    _currentBalance = 0;
                }
                else
                {
                    _currentBalance = value;
                }
            }
        }

        public SlotMachine(
            IOptions<SlotMachineConfiguration> configuration, 
            IRandomWithProbabilities randomWithProbabilities)
        {
            _configuration = configuration.Value;
            _randomWithProbabilities = randomWithProbabilities;
            _rows = _configuration.Dimensions.Rows;
            _cols = _configuration.Dimensions.Cols;
            _field = new Symbol[_rows, _cols];
            _symbols = _configuration.Symbols;
        }

        public SpinResult Play(decimal stake)
        {
            if (stake > 0 && stake <= _currentBalance)
            {
                _currentBalance -= stake;
                var spinResult = this.Spin();
                _currentBalance += spinResult.WinCoefficient * stake;

                return spinResult;
            }
            else
            {
                throw new ArgumentOutOfRangeException("The stake must be positive and equal or less than the left deposit!");
            }
        }

        private SpinResult Spin()
        {
            decimal winCoefficient = 0;

            for (int row = 0; row < _rows; row++)
            {
                decimal rowWinCoefficient = 0;
                bool isFirstSymbol = true;
                bool isWinningRow = true;
                var winningSymbol = string.Empty;

                for (int col = 0; col < _cols; col++)
                {
                    var symbol = _randomWithProbabilities.Draw(_symbols);
                    _field[row, col] = symbol;

                    // get the first symbol that is not wildcard
                    if (isFirstSymbol && !symbol.IsWildCard)
                    {
                        winningSymbol = symbol.Sign;
                        isFirstSymbol = false;
                        rowWinCoefficient += symbol.Coefficient;
                    }
                    else if (isFirstSymbol && symbol.IsWildCard)
                    {
                        continue;
                    }
                    else if (isWinningRow && (winningSymbol == symbol.Sign || symbol.IsWildCard))
                    {
                        rowWinCoefficient += symbol.Coefficient;
                    }
                    else
                    {
                        isWinningRow = false;
                        rowWinCoefficient = 0;
                    }
                }

                winCoefficient += rowWinCoefficient;
            }

            return new SpinResult()
            {
                Field = _field,
                WinCoefficient = winCoefficient
            };
        }
    }
}
