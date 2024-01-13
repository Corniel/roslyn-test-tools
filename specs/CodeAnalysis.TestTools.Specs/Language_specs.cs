namespace Language_specs;

public class Parses
{
    [TestCase(".xml")]
    [TestCase("EXTENSIBLEMARKUPLANGUAGE")]
    [TestCase("XML")]
    public void as_XML(string str)
        => Language.Parse(str).Should().Be(Language.XML);

    [TestCase(".fs")]
    [TestCase("FSharp")]
    [TestCase("F#")]
    [TestCase("fs")]
    public void as_FSharp(string str)
       => Language.Parse(str).Should().Be(Language.FSharp);
}

public class Does_not_parse
{
    [Test]
    public void garbage()
        => "garbage".Invoking(Language.Parse).Should().Throw<FormatException>();
}
