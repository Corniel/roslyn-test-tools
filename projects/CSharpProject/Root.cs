namespace CSharpProject;

public sealed class Root
{
    public Qowaiv.Uuid Id { get; init; }

    public static int Answer() => new MathNet.Numerics.Random.MersenneTwister().Next();
}
