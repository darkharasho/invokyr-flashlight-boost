using InvokyrFlashlightBoost;
using Xunit;

public class BeamTests
{
    [Fact]
    public void WidenDoublesTheCircleNotTheAngle() => Assert.Equal(113.8f, Beam.Widen(75f, 2f), 1);

    [Fact]
    public void WidenByOneIsVanilla() => Assert.Equal(75f, Beam.Widen(75f, 1f), 3);

    [Fact]
    public void WidenIsCapped() => Assert.True(Beam.Widen(150f, 4f) <= 170f);

    [Theory]
    [InlineData(0.2f, 1.5f, 12f, 1.5f)]
    [InlineData(0.2f, 1.5f, 1f, 0.85f)]
    [InlineData(0.2f, 0f, 12f, 0.2f)]
    public void NearPlane(float vanilla, float wanted, float range, float expected) =>
        Assert.Equal(expected, Beam.NearPlane(vanilla, wanted, range), 3);

    [Fact]
    public void GameChanged()
    {
        Assert.False(Beam.GameChanged(75f, 75f, 113.8f));
        Assert.False(Beam.GameChanged(113.8f, 75f, 113.8f));
        Assert.True(Beam.GameChanged(90f, 75f, 113.8f));
    }
}
