internal static class Noncompliant
{
    public static int Unchanged()
    {
        var variable = 42; // Noncompliant {{'variable' can be a constant.}}
//      ^^^^^^^^^^^^^^^^^^
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
