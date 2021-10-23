using FluentAssertions;
using NUnit.Framework;
using CodeAnalysis.TestTools;
using Specs.Analyzers;
using System;

namespace Verify_specs
{
    public class Crashes_when
    {
        [Test]
        public void analyzer_crashes()
        {
            Action verify = () => new CrashingAnalyzer()
                .ForCS()
                .AddSnippet("public class Dummy { }")
                .Verify();

            verify.Should()
                .Throw<AnalyzerCrashed>()
                .WithMessage("Analyzer 'Specs.Analyzers.CrashingAnalyzer' threw an exception of type *");
        }

        [Test]
        public void no_sources_provided()
        {
            Action verify = () => new CSharpOnly()
                .ForCS()
                .Verify();

            verify.Should().Throw<IncompleteSetup>()
                .WithMessage("The setup is incomplete. No sources have been configured.");
        }
    }

    public class Succeeds_when
    {
        [Test]
        public void Non_precise_issue_is_found_on_same_line()
        {
            Action verify = () => new CSharpOnly()
                .ForCS()
                .AddSnippet("public class MyClass { // Error")
                .Verify();

            verify.Should().NotThrow();
        }

        [Test]
        public void expected_location_is_matches_actual()
        {
            Action verify = () => new CSharpOnly()
                .ForCS()
                .AddSnippet(@"public class MyClass { // Error ^37#0")
                .Verify();

            verify.Should().NotThrow();
        }
    }

    public class Supports
    {
        [Test]
        public void Unsafe_CSharp_when_enabled()
        {
            Action verify = () => new CSharpOnly()
                .ForCS()
                .WithUnsafeCode(enable: true)
                .AddSnippet(@"
        public class MyClass 
        {
            unsafe int Risky() => 666;
        }")
                .Verify();

            verify.Should().NotThrow();
        }
    }

    public class Fails_when
    {
        [Test]
        public void Noncompliant_line_raises_error_instead()
        {
            Action verify = () => new CSharpOnly()
                .ForCS()
                .AddSnippet("public class MyClass { // Noncompliant")
                .Verify();

            verify.Should().Throw<VerificationFailed>();
        }

        [Test]
        public void expected_location_is_other_than_actual()
        {
            Action verify = () => new CSharpOnly()
                .ForCS()
                .AddSnippet(@"
                    public class MyClass { // Error
                    //     ^^^^")
                .Verify();

            verify.Should().Throw<VerificationFailed>();
        }
    }
}
