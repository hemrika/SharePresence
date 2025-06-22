using System;
using System.Xml.Linq;
using NUnit.Framework;
using Hemrika.SharePresence.Extensions;

namespace Hemrika.SharePresence.Tests
{
    [TestFixture]
    public class XElementExtensionsTests
    {
        [Test]
        public void ToDateTime_WithValidValue_ReturnsParsedDate()
        {
            var element = new XElement("Date", DateTime.UtcNow.ToString("o"));
            var result = element.ToDateTime(DateTime.MinValue);
            Assert.AreNotEqual(DateTime.MinValue, result);
            Assert.AreEqual(DateTime.Parse(element.Value, null, System.Globalization.DateTimeStyles.RoundtripKind), result);
        }

        [Test]
        public void ToDateTime_WithEmptyValue_ReturnsEmptyValue()
        {
            var empty = new DateTime(2020, 1, 1);
            var element = new XElement("Date", string.Empty);
            var result = element.ToDateTime(empty);
            Assert.AreEqual(empty, result);
        }

        [Test]
        public void ToNullableDecimal_WithValidValue_ReturnsValue()
        {
            var element = new XElement("Dec", "12.5");
            var result = element.ToNullableDecimal();
            Assert.IsNotNull(result);
            Assert.AreEqual(12.5m, result.Value);
        }

        [Test]
        public void ToNullableDecimal_WithEmptyValue_ReturnsNull()
        {
            var element = new XElement("Dec", string.Empty);
            var result = element.ToNullableDecimal();
            Assert.IsNull(result);
        }

        [Test]
        public void ToNullableInt_WithValidValue_ReturnsValue()
        {
            var element = new XElement("Int", "42");
            var result = element.ToNullableInt();
            Assert.IsNotNull(result);
            Assert.AreEqual(42, result.Value);
        }

        [Test]
        public void ToNullableInt_WithEmptyValue_ReturnsNull()
        {
            var element = new XElement("Int", string.Empty);
            var result = element.ToNullableInt();
            Assert.IsNull(result);
        }
    }
}
