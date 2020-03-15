namespace Fanex.Athena.ExtService.BetList.UnitTest.Helper
{
    using System.Collections.Generic;
    using Fanex.Athena.ExtService.BetList.Helper;
    using Fanex.Athena.ExtService.BetList.Models.DataDomain;
    using Fanex.Athena.ExtService.BetList.Models.DTO;
    using Fanex.Athena.ExtService.Common;
    using Moq;
    using Xunit;

    public class StatementHelperTest
    {
        private const decimal PreviousBalance = 10;
        private const decimal TotalAmt = 100;
        private const decimal TurnOvert = 200;
        private const decimal Comm = 0.5m;

        private const string SabaPromotion = "SPR";

        private readonly Mock<IStatementValue> _statementValue;

        public StatementHelperTest()
        {
            _statementValue = new Mock<IStatementValue>();
            _statementValue.Setup(x => x.Commission).Returns(Comm);
            _statementValue.Setup(x => x.TotalAmt).Returns(TotalAmt);
            _statementValue.Setup(x => x.TurnOver).Returns(TurnOvert);
        }

        public static IEnumerable<object[]> GetStatementRemarkTestCase
        {
            get
            {
                return new[]
                {
                    new object[] { string.Empty, TransTypeValue.BET, TransTypeValue.BET },
                    new object[] { TransDescValue.BetStatement, TransTypeValue.BET, $"- {TransDescValue.RemarkValues[TransDescValue.BetStatement].Invoke()}" }
                };
            }
        }

        public static IEnumerable<object[]> GetTransDescTextTestCase
        {
            get
            {
                return new[]
                {
                    new object[] { string.Empty,  string.Empty },
                    new object[] { TransDescValue.VoidCancell, TransDescValue.RemarkValues[TransDescValue.VoidCancell].Invoke() }
                };
            }
        }

        public static IEnumerable<object[]> GetBetingStatementRemarkTestCase
        {
            get
            {
                return new[]
                {
                    new object[] { string.Empty, SabaPromotion, SystemHelper.GetProductName(SabaPromotion) },
                    new object[]
                    {
                        "transDescText",
                        "Test",
                        $"{SystemHelper.GetProductName("Test")} transDescText"
                    }
                };
            }
        }

        [Theory]
        [InlineData(TransDescValue.BetStatement)]
        [InlineData(TransDescValue.VoidCancell)]
        [InlineData(TransDescValue.Outstanding)]
        [InlineData("Any")]
        public void GetStatementBalance_WhenCallWithBETTransType_ReturnCorrectValue(string transDesc)
        {
            // Arrange
            _statementValue.Setup(x => x.TransType).Returns(TransTypeValue.BET);
            _statementValue.Setup(x => x.TransDesc).Returns(transDesc);
            var expectedValue = GetExpectedValueWithBetTransType();
            
            // Action
            var result = StatementHelper.GetStatementBalance(PreviousBalance, _statementValue.Object);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData(TransTypeValue.BAL)]
        [InlineData(TransTypeValue.GP)]
        [InlineData("ANY")]
        public void GetStatementBalance_WhenCallWithTransType_ReturnCorrectValue(string transtype)
        {
            // Arrange
            var expectedValue = transtype == TransTypeValue.BAL ? TotalAmt : PreviousBalance + TotalAmt;
            _statementValue.Setup(x => x.TransType).Returns(transtype);

            // Action
            var result = StatementHelper.GetStatementBalance(PreviousBalance, _statementValue.Object);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData(TransDescValue.PromotionPrize)]
        [InlineData(TransDescValue.BetStatement)]
        [InlineData(TransDescValue.VoidCancell)]
        [InlineData(TransDescValue.Outstanding)]
        [InlineData("Any")]
        public void GetBettingBalance_WhenCall_ReturnCorrectValue(string transDesc)
        {
            // Arrange
            _statementValue.Setup(x => x.TransDesc).Returns(transDesc);
            var expectedValue = GetExpectedValueWithBetTransType();

            // Action
            var result = StatementHelper.GetBettingBalance(PreviousBalance, _statementValue.Object);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData(TransTypeValue.BAL, TransDescValue.Outstanding, 0)]
        [InlineData("ANY", TransDescValue.Outstanding, 0)]
        [InlineData(TransTypeValue.BAL, "ANY", 0)]
        [InlineData("ANY", "ANY", 100)]
        public void GetCreditDebit_WhenCall_ReturnCorrectValue(string transType, string transDesc, decimal expectedValue)
        {
            // Arrange
            _statementValue.Setup(x => x.TransType).Returns(transType);
            _statementValue.Setup(x => x.TransDesc).Returns(transDesc);
            
            // Action
            var result = StatementHelper.GetCreditDebit(_statementValue.Object);

            // Assert 
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData("", false)]
        [InlineData("AGC", true)]
        [InlineData("GD", true)]
        [InlineData("BX", true)]
        public void HideBetListLink_WhenCall_ReturnCorrectResult(string productCode, bool expectedValue)
        {
            // Action
            var result = StatementHelper.HideBetListLink(productCode);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [InlineData("", TransTypeValue.BET, false)]
        [InlineData("", TransTypeValue.PROMO, false)]
        [InlineData(TransDescValue.BetStatement, TransTypeValue.BET, true)]
        [InlineData(TransDescValue.BetStatement, TransTypeValue.PROMO, true)]
        [InlineData(TransDescValue.BetStatement, TransTypeValue.GP, false)]
        public void ShowBetingStatementLink_WhenCall_ReturnCorrectResult(string transDesc, string transType, bool expectedValue)
        {
            // Action
            var result = StatementHelper.ShowBetingStatementLink(transDesc, transType);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [MemberData(nameof(GetStatementRemarkTestCase))]
        public void GetStatementRemark_WhenCall_ReturnCorrectResult(string transDesc, string transType, string expectedValue)
        {
            // Action
            var result = StatementHelper.GetStatementRemark(transDesc, transType);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [MemberData(nameof(GetTransDescTextTestCase))]
        public void GetTransDescText_WhenCall_ReturnCorrectResult(string transDesc, string expectedValue)
        {
            // Action
            var result = StatementHelper.GetTransDescText(transDesc);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        [Theory]
        [MemberData(nameof(GetBetingStatementRemarkTestCase))]
        public void GetBetingStatementRemark_WhenCall_ReturnCorrectResult(string transDescText, string productCode, string expectedValue)
        {
            // Action
            var result = StatementHelper.GetBetingStatementRemark(transDescText, productCode);

            // Assert
            Assert.Equal(expectedValue, result);
        }

        private object GetExpectedValueWithBetTransType()
        {
            if (_statementValue.Object.TransDesc == TransDescValue.BetStatement)
            {
                return PreviousBalance + _statementValue.Object.TotalAmt + _statementValue.Object.Commission;
            }

            if (_statementValue.Object.TransDesc == TransDescValue.VoidCancell)
            {
                return PreviousBalance;
            }

            if (_statementValue.Object.TransDesc == TransDescValue.Outstanding)
            {
                return PreviousBalance - _statementValue.Object.TurnOver;
            }

            if (_statementValue.Object.TransDesc == TransDescValue.PromotionPrize)
            {
                return PreviousBalance + _statementValue.Object.TotalAmt;
            }

            return 0m;
        }
    }
}
