namespace Guarded_collection_specs;

public class Add
{
    [Test]
    public void creates_new_instance()
    {
        var extended = DiagnosticIds.Empty.Add(DiagnosticId.AD0001);
        extended.Should().NotBeSameAs(DiagnosticIds.Empty);
    }

    [Test]
    public void contains_existing_and_new()
    {
        var init = DiagnosticIds.Empty.Add(DiagnosticId.AD0001);
        var extended = init.AddRange(DiagnosticId.BC36716);
        extended.Should().BeEquivalentTo(new[]
        {
            DiagnosticId.AD0001,
            DiagnosticId.BC36716
        });

        init.Should().BeEquivalentTo(new[] { DiagnosticId.AD0001 }, because: "Should not change.");
    }
}

public class Add_range
{
    [Test]
    public void creates_new_instance()
    {
        var extended = DiagnosticIds.Empty.AddRange(DiagnosticId.AD0001);
        extended.Should().NotBeSameAs(DiagnosticIds.Empty);
    }

    [Test]
    public void contains_existing_and_new()
    {
        var init = DiagnosticIds.Empty.AddRange(DiagnosticId.AD0001);
        var extended = init.AddRange(DiagnosticId.BC36716);
        extended.Should().BeEquivalentTo(new[]
        {
            DiagnosticId.AD0001,
            DiagnosticId.BC36716
        });

        init.Should().BeEquivalentTo(new[] { DiagnosticId.AD0001 }, because: "Should not change.");
    }
}
