using Microsoft.Extensions.Options;
using Moq;
using SlotMachine.Models;

namespace SlotMachine.Tests
{
    public class SlotMachineTests
    {
        private readonly Mock<IRandomWithProbabilities> mockRandomWithProbabilities;

        private readonly IOptions<SlotMachineConfiguration> slotMachineConfiguration;
        private readonly Models.SlotMachine slotMachine;

        private readonly Symbol symbolA = new Symbol {Sign = "A", Coefficient = 0.4m, Probability = 45, IsWildCard = false};
        private readonly Symbol symbolB = new Symbol {Sign = "B", Coefficient = 0.6m, Probability = 35, IsWildCard = false};
        private readonly Symbol symbolP = new Symbol {Sign = "P", Coefficient = 0.8m, Probability = 15, IsWildCard = false};
        private readonly Symbol symbolWildcard = new Symbol {Sign = "*", Coefficient = 0m, Probability = 5, IsWildCard = true};
        private const decimal stake = 10;

        public SlotMachineTests()
        {
            slotMachineConfiguration = Options.Create(new SlotMachineConfiguration()
            {
                Dimensions = new Dimension { Rows = 2, Cols = 3 },
                Symbols = new List<Symbol>() { symbolA, symbolB, symbolP, symbolWildcard }
            });

            mockRandomWithProbabilities = new Mock<IRandomWithProbabilities>();
            slotMachine = new Models.SlotMachine(slotMachineConfiguration, mockRandomWithProbabilities.Object);
            slotMachine.CurrentBalance = stake;
        }

        [Fact]
        [Trait("Win", "One row")]
        public void Play_One_Row_Win_SymbolA()
        {
            // Arrange
            mockRandomWithProbabilities.SetupSequence(c => c.Draw(It.IsAny<List<Symbol>>()))
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolB)
                .Returns(symbolA)
                .Returns(symbolA);

            // Act
            var result = slotMachine.Play(stake);

            // Assert
            Assert.Equal(1.2m, result.WinCoefficient);
            Assert.Equal(12, slotMachine.CurrentBalance);
        }

        [Fact]
        [Trait("Win", "One row")]
        public void Play_One_Row_Win_SymbolB()
        {
            // Arrange
            mockRandomWithProbabilities.SetupSequence(c => c.Draw(It.IsAny<List<Symbol>>()))
                .Returns(symbolA)
                .Returns(symbolB)
                .Returns(symbolA)
                .Returns(symbolB)
                .Returns(symbolB)
                .Returns(symbolB);

            // Act
            var result = slotMachine.Play(stake);

            // Assert
            Assert.Equal(1.8m, result.WinCoefficient);
            Assert.Equal(18, slotMachine.CurrentBalance);
        }

        [Fact]
        [Trait("Win", "One row")]
        public void Play_One_Row_Win_SymbolA_And_Wildcard_First()
        {
            // Arrange
            mockRandomWithProbabilities.SetupSequence(c => c.Draw(It.IsAny<List<Symbol>>()))
                .Returns(symbolWildcard)
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolB)
                .Returns(symbolA)
                .Returns(symbolA);

            // Act
            var result = slotMachine.Play(stake);

            // Assert
            Assert.Equal(0.8m, result.WinCoefficient);
            Assert.Equal(8, slotMachine.CurrentBalance);
        }

        [Fact]
        [Trait("Win", "One row")]
        public void Play_One_Row_Win_SymbolA_And_Wildcard_Second()
        {
            // Arrange
            mockRandomWithProbabilities.SetupSequence(c => c.Draw(It.IsAny<List<Symbol>>()))
                .Returns(symbolA)
                .Returns(symbolWildcard)
                .Returns(symbolA)
                .Returns(symbolB)
                .Returns(symbolA)
                .Returns(symbolA);

            // Act
            var result = slotMachine.Play(stake);

            // Assert
            Assert.Equal(0.8m, result.WinCoefficient);
            Assert.Equal(8, slotMachine.CurrentBalance);
        }

        [Fact]
        [Trait("Win", "One row")]
        public void Play_One_Row_Win_SymbolA_And_Wildcard_Third()
        {
            // Arrange
            mockRandomWithProbabilities.SetupSequence(c => c.Draw(It.IsAny<List<Symbol>>()))
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolWildcard)
                .Returns(symbolB)
                .Returns(symbolA)
                .Returns(symbolA);

            // Act
            var result = slotMachine.Play(stake);

            // Assert
            Assert.Equal(0.8m, result.WinCoefficient);
            Assert.Equal(8, slotMachine.CurrentBalance);
        }

        [Fact]
        [Trait("Win", "One row")]
        public void Play_One_Row_Win_Only_Wildcard()
        {
            // Arrange
            mockRandomWithProbabilities.SetupSequence(c => c.Draw(It.IsAny<List<Symbol>>()))
                .Returns(symbolWildcard)
                .Returns(symbolWildcard)
                .Returns(symbolWildcard)
                .Returns(symbolB)
                .Returns(symbolA)
                .Returns(symbolA);

            // Act
            var result = slotMachine.Play(stake);

            // Assert
            Assert.Equal(0m, result.WinCoefficient);
            Assert.Equal(0, slotMachine.CurrentBalance);
        }

        [Fact]
        [Trait("Win", "Two rows")]
        public void Play_Two_Rows_Win_SymbolA()
        {
            // Arrange
            mockRandomWithProbabilities.SetupSequence(c => c.Draw(It.IsAny<List<Symbol>>()))
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolA);

            // Act
            var result = slotMachine.Play(stake);

            // Assert
            Assert.Equal(2.4m, result.WinCoefficient);
            Assert.Equal(24, slotMachine.CurrentBalance);
        }

        [Fact]
        [Trait("Win", "Two rows")]
        public void Play_Two_Rows_Win_SymbolA_SymbolP()
        {
            // Arrange
            mockRandomWithProbabilities.SetupSequence(c => c.Draw(It.IsAny<List<Symbol>>()))
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolA)
                .Returns(symbolP)
                .Returns(symbolP)
                .Returns(symbolP);

            // Act
            var result = slotMachine.Play(stake);

            // Assert
            Assert.Equal(3.6m, result.WinCoefficient);
            Assert.Equal(36, slotMachine.CurrentBalance);
        }

        [Fact]
        [Trait("Loss", "")]
        public void Play_Two_Rows_Loss()
        {
            // Arrange
            mockRandomWithProbabilities.SetupSequence(c => c.Draw(It.IsAny<List<Symbol>>()))
                .Returns(symbolA)
                .Returns(symbolB)
                .Returns(symbolA)
                .Returns(symbolP)
                .Returns(symbolA)
                .Returns(symbolA);

            // Act
            var result = slotMachine.Play(stake);

            // Assert
            Assert.Equal(0m, result.WinCoefficient);
            Assert.Equal(0, slotMachine.CurrentBalance);
        }

        [Fact]
        [Trait("Exception", "")]
        public void Play_Negative_Stake_Throw_Exception()
        {
            // Act
            Action action = () => slotMachine.Play(-10);

            // Assert
            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        [Trait("Exception", "")]
        public void Play_Zero_Stake_Throw_Exception()
        {
            // Act
            Action action = () => slotMachine.Play(0);

            // Assert
            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);
        }

        [Fact]
        [Trait("Exception", "")]
        public void Play_Stake_Bigger_Than_CurrentBalance_Throw_Exception()
        {
            // Act
            Action action = () => slotMachine.Play(stake + 1);

            // Assert
            var caughtException = Assert.Throws<ArgumentOutOfRangeException>(action);
        }
    }
}