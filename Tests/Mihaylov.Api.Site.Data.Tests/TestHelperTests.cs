using Mihaylov.Api.Site.Contracts.Models;
using Mihaylov.Common.Generic.Extensions;

namespace Mihaylov.Api.Site.Data.Tests
{
    [TestClass]
    public class TestHelperTests
    {
        public const int AGE = 10;
        public const string DOB = "2014.04.18";  // Datetime.Now - 10y - 6m
        public const string DOB2 = "2004.10.23";

        [TestMethod]
        [DataRow(null, null, null, null, null)]
        [DataRow(DateOfBirthType.YearCalculated, null, null, null, null)]
        [DataRow(DateOfBirthType.Full, null, null, null, null)]
        
        [DataRow(null, AGE, null, null, null)]
        [DataRow(DateOfBirthType.YearCalculated, AGE, null, DateOfBirthType.YearCalculated, DOB)]
        [DataRow(DateOfBirthType.Full, AGE, null, null, null)]

        [DataRow(null, null, DOB2, null, null)]
        [DataRow(DateOfBirthType.YearCalculated, null, DOB2, DateOfBirthType.YearCalculated, DOB2)]
        [DataRow(DateOfBirthType.Full, null, DOB2, DateOfBirthType.Full, DOB2)]
        
        [DataRow(null, AGE, DOB2, null, null)]
        [DataRow(DateOfBirthType.YearCalculated, AGE, DOB2, DateOfBirthType.YearCalculated, DOB2)]
        [DataRow(DateOfBirthType.Full, AGE, DOB2, DateOfBirthType.Full, DOB2)]
        public void Test(DateOfBirthType? type, int? age, string dateString,
            DateOfBirthType? typeResult, string dateStrResult)
        {
            DateTime? date = null;
            if (!string.IsNullOrWhiteSpace(dateString))
            {
                date = DateTime.Parse(dateString).Date;
            }

            DateTime? dateResult = null;
            if (!string.IsNullOrWhiteSpace(dateStrResult))
            {
                dateResult = DateTime.Parse(dateStrResult).Date;
            }

            var isCalculating = type == DateOfBirthType.YearCalculated;
            var type1 = date.IsBirthDateTypeValid(age, isCalculating) ? type : null;
            var date1 = date.GetBirthDate(age, type.HasValue, isCalculating);

            Assert.AreEqual(typeResult, type1);
            Assert.AreEqual(dateResult, date1);
        }
    }
}