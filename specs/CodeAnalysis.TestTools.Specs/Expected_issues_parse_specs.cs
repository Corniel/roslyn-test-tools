namespace Expected_issues_parse_specs;

public class Regular_pattern_supports
{
    [Test]
    public void Noncompliant_type()
    {
        var parsed = ExpectedIssue.Parse("some code // Noncompliant".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: string.Empty,
            location: new IssueLocation(1)));
    }

    [Test]
    public void Noncompliant_with_message()
    {
        var parsed = ExpectedIssue.Parse("some code // Noncompliant {{Not what we want.}}".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(1)));
    }

    [Test]
    public void Noncompliant_with_location_via_precise_pattern()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code // Noncompliant {{Not what we want.}}
                //   ^^^^
".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(
                lineNumber: 2,
                start: 21,
                spanSize: 4)));
    }

    [Test]
    public void Noncompliant_with_location_via_pattern()
    {
        var parsed = ExpectedIssue.Parse(@"some code // Noncompliant ^21#4 [ID007] {{Not what we want.}}".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: "ID007",
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(
                lineNumber: 1,
                start: 21,
                spanSize: 4)));
    }

    [Test]
    public void Noncompliant_with_diagnostic_id()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code // Noncompliant [ID007] {{Not what we want.}}
                //   ^^^^".Lines());

        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: "ID007",
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(
                lineNumber: 2,
                start: 21,
                spanSize: 4)));
    }

    [Test]
    public void Noncompliant_with_positive_offset()
    {
        var parsed = ExpectedIssue.Parse(@"some code // Noncompliant@+12 {{Not what we want.}}".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(lineNumber: 1 + 12)));
    }

    [Test]
    public void Noncompliant_with_negative_offset()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code // Noncompliant@-1 {{Not what we want.}}".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(lineNumber: 2 - 1)));
    }

    [Test]
    public void secondary_type()
    {
        var parsed = ExpectedIssue.Parse("some code // Secondary".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: string.Empty,
            location: new IssueLocation(1)));
    }

    [Test]
    public void secondary_on_same_line()
    {
        var parsed = ExpectedIssue.Parse(@"
        public void Bar(ref object o1, ref object o2)
//                  ^^^ Noncompliant {{Make this method generic and replace the 'object' parameter with a type parameter.}}
//                                 ^^ Secondary@-1
//                                                ^^ Secondary@-2".Lines());

        parsed.Should().BeEquivalentTo(
            [
                new ExpectedIssue(
                    diagnosticId: string.Empty,
                    type: IssueType.Noncompliant,
                    message: "Make this method generic and replace the 'object' parameter with a type parameter.",
                    location: new IssueLocation(2, 20, 3)),
                new ExpectedIssue(
                    diagnosticId: string.Empty,
                    type: IssueType.Noncompliant,
                    message: string.Empty,
                    location: new IssueLocation(2, 35, 2)),
                new ExpectedIssue(
                    diagnosticId: string.Empty,
                    type: IssueType.Noncompliant,
                    message: string.Empty,
                    location: new IssueLocation(2, 50, 2)),
            ]);

    }

    [Test]
    public void error_type()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code // Error [ID007] {{Not what we want.}}
                //   ^^^^".Lines());

        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: "ID007",
            type: IssueType.Error,
            message: "Not what we want.",
            location: new IssueLocation(
                lineNumber: 2,
                start: 21,
                spanSize: 4)));
    }
}

public class Precise_pattern_supports
{
    [Test]
    public void Noncompliant_type()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code 
                //   ^^^^".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: string.Empty,
            location: new IssueLocation(
                lineNumber: 2,
                start: 21,
                spanSize: 4)));
    }

    [Test]
    public void Noncompliant_with_message()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code 
                //   ^^^^ {{Not what we want.}}".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(
                lineNumber: 2,
                start: 21,
                spanSize: 4)));
    }

    [Test]
    public void Noncompliant_with_diagnostic_id()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code 
                //   ^^^^ [ID007] {{Not what we want.}}".Lines());

        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: "ID007",
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(
                lineNumber: 2,
                start: 21,
                spanSize: 4)));
    }

    [Test]
    public void Noncompliant_with_positive_offset()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code 
                //   ^^^^@+12 {{Not what we want.}}".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(
                lineNumber: 2 + 12,
                start: 21,
                spanSize: 4)));
    }

    [Test]
    public void Noncompliant_with_negative_offset()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code 
                //   ^^^^@-1 {{Not what we want.}}".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: "Not what we want.",
            location: new IssueLocation(
                lineNumber: 2 - 1,
                start: 21,
                spanSize: 4)));
    }

    [Test]
    public void secondary_type()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code
                //   ^^^^ Secondary".Lines());
        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: string.Empty,
            type: IssueType.Noncompliant,
            message: string.Empty,
            location: new IssueLocation(
                lineNumber: 2,
                start: 21,
                spanSize: 4)));
    }

    [Test]
    public void error_type()
    {
        var parsed = ExpectedIssue.Parse(@"
                some code
                //   ^^^^ Error [ID007] {{Not what we want.}}".Lines());

        parsed.Single().Should().Be(new ExpectedIssue(
            diagnosticId: "ID007",
            type: IssueType.Error,
            message: "Not what we want.",
            location: new IssueLocation(
                lineNumber: 2,
                start: 21,
                spanSize: 4)));
    }
}

public class Not_allowed
{
    [Test]
    public void location_specified_twice_on_line()
    {
        @"namespace MyNameSpace
{
    class MyClass { } // Noncompliant
//  ^^^^^ ^^^^^^^"
            .Invoking(code => ExpectedIssue.Parse(code.Lines()))
            .Should().Throw<ParseError>()
            .WithMessage(
                "Unexpected ^ at line 4 pos 11. Only one precise location can be specified per line.");
    }

    [Test]
    public void remaining_open_curly_bracket()
    {
        "code // Noncompliant {"
            .Invoking(code => ExpectedIssue.Parse(code.Lines()))
            .Should().Throw<ParseError>()
            .WithMessage(
                "Unexpected { at line 1 pos 22. Either correctly use '{{message}}' or remove the curly brace.");
    }

    [Test]
    public void remaining_close_curly_bracket()
    {
        "code // Noncompliant message}}"
            .Invoking(code => ExpectedIssue.Parse(code.Lines()))
            .Should().Throw<ParseError>()
            .WithMessage(
                "Unexpected } at line 1 pos 29. Either correctly use '{{message}}' or remove the curly brace.");
    }
}
