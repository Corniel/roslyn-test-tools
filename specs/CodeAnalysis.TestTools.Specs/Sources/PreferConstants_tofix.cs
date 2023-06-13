internal static class Noncompliant
{
    public static int Unchanged()
    {
        int variable = 42;
        return variable;
    }
}

internal static class Compliant
{
    public static int Changed()
    {
        var variable = 42;
        variable *= 3;
        return variable;
    }
}
