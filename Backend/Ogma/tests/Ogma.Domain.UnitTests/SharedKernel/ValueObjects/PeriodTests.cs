using FluentAssertions;
using Ogma.Domain.SharedKernel.ValueObjects;

namespace Ogma.Domain.UnitTests.SharedKernel.ValueObjects;

public class PeriodTests
{
    [Fact]
    public void Constructor_ValidParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var start = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        var end = new DateTime(2030, 12, 31, 0, 0, 0, DateTimeKind.Utc);
        // Act
        var period = new Period(start, end);
        // Assert
        period.Start.Should().Be(start);
        period.End.Should().Be(end);
    }

    [Fact]
    public void Constructor_NullOptionalParameters_SetsPropertiesCorrectly()
    {
        // Arrange
        var start = new DateTime(2020, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        // Act
        var period = new Period(start);
        // Assert
        period.Start.Should().Be(start);
        period.End.Should().BeNull();
    }

    [Fact]
    public void Constructor_InvalidStart_ThrowsArgumentException()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Period(DateTime.MinValue));
    }

    [Fact]
    public void ConstructorStartAfterEnd_ThrowsArgumentException()
    {
        // Arrange
        var start = new DateTime(2030, 12, 31);
        var end = new DateTime(2020, 1, 1);
        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Period(DateTime.MinValue));
    }

    [Fact]
    public void Contains_DateInPeriod_ReturnsTrue()
    {
        // Arrange
        var start = new DateTime(2020, 1, 1);
        var end = new DateTime(2030, 12, 31);
        var dateChecked = new DateTime(2025, 12, 31);
        // Act
        var period = new Period(start, end);
        // Assert
        period.Contains(dateChecked).Should().BeTrue();
    }

    [Fact]
    public void Contains_DateOutsidePeriod_ReturnsFalse()
    {
        // Arrange
        var start = new DateTime(2020, 1, 1);
        var end = new DateTime(2030, 12, 31);
        var dateChecked = new DateTime(2035, 12, 31);
        // Act
        var period = new Period(start, end);
        // Assert
        period.Contains(dateChecked).Should().BeFalse();
    }

    [Fact]
    public void OverlapsWith_OverlappingPeriods_ReturnsTrue()
    {
        // Arrange
        var start1 = new DateTime(2020, 1, 1);
        var end1 = new DateTime(2030, 12, 31);
        var start2 = new DateTime(2025, 12, 31);
        // Act
        var period1 = new Period(start1, end1);
        var period2 = new Period(start2);
        // Assert
        period1.OverlapsWith(period2).Should().BeTrue();
    }

    [Fact]
    public void OverlapsWith_NotOverlappingPeriods_ReturnsFalse()
    {
        // Arrange
        var start1 = new DateTime(2020, 1, 1);
        var end1 = new DateTime(2030, 12, 31);
        var start2 = new DateTime(2035, 12, 31);
        // Act
        var period1 = new Period(start1, end1);
        var period2 = new Period(start2);
        // Assert
        period1.OverlapsWith(period2).Should().BeFalse();
    }

    [Fact]
    public void IsOngoing_NullEnd_ReturnsTrue()
    {
        // Arrange
        var start = new DateTime(2020, 1, 1);
        // Act
        var period = new Period(start);
        // Assert
        period.IsOngoing.Should().BeTrue();
    }

    [Fact]
    public void IsOngoing_HasEnd_ReturnsFalse()
    {
        // Arrange
        var start = new DateTime(2020, 1, 1);
        var end = new DateTime(2030, 12, 31);
        // Act
        var period = new Period(start, end);
        // Assert
        period.IsOngoing.Should().BeFalse();
    }

    [Theory]
    [InlineData(2025, 1, 1, null, null, null, null)]                     // ongoing → null
    [InlineData(2025, 1, 1, 2025, 1, 31, "30.00:00:00")]                // 30 days
    [InlineData(2025, 1, 1, 2025, 12, 31, "364.00:00:00")]              // 334 days
    [InlineData(2024, 2, 1, 2024, 3, 1, "29.00:00:00")]                  // leap year
    [InlineData(2025, 1, 1, 2025, 1, 1, "00:00:00")]                    // same day
    public void Duration_ReturnsExpectedValue(
        int startYear, int startMonth, int startDay,
        int? endYear, int? endMonth, int? endDay,
        string? expectedDurationString)
    {
        // Arrange
        var start = new DateTime(startYear, startMonth, startDay);
        var end = endYear.HasValue && endMonth.HasValue && endDay.HasValue
            ? new DateTime(endYear.Value, endMonth.Value, endDay.Value)
            : (DateTime?)null;

        // Act
        var period = new Period(start, end);

        TimeSpan? duration = period.Duration;

        // Assert
        if (expectedDurationString is null)
        {
            duration.Should().BeNull();
            period.IsOngoing.Should().BeTrue();
        }
        else
        {
            duration.Should().NotBeNull();
            var expected = TimeSpan.Parse(expectedDurationString);
            expected.Should().Be(duration.Value);
        }
    }
}
