using CodeAnalysis.TestTools.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace CodeAnalysis.TestTools.Diagnostics
{
    /// <summary>Collection of <see cref="ExpectedIssue"/>'s.</summary>
    /// <remarks>
    /// * Splits issues with a diagnostic ID containing multiple ID's.
    /// * Merges issue defined by the Precise-pattern with those defined by the Regular-pattern.
    /// </remarks>
    [DebuggerTypeProxy(typeof(CollectionDebugView))]
    [DebuggerDisplay("Count = {Count}")]
    internal sealed class ExpectedIssues : IReadOnlyCollection<ExpectedIssue>
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly List<ExpectedIssue> issues = new();

        /// <inheritdoc />
        public int Count => issues.Count;

        public void Add(ExpectedIssue issue)
        {
            if (issue.DiagnosticId.Contains(','))
            {
                issues.AddRange(PerDiagnosticId(issue));
            }
            else
            {
                issues.Add(issue);
            }
        }

        public ExpectedIssues Merge(IEnumerable<ExpectedIssue> precises)
        {
            foreach (var precise in precises)
            {
                if (!Merge(precise))
                {
                    issues.Add(precise);
                }
            }
            issues.Sort();
            return this;
        }

        private bool Merge(ExpectedIssue issue)
        {
            if (issue.IsLocationOnly())
            {
                var candidates = issues.Where(existing => existing.Location.LineNumber == issue.Location.LineNumber);
                var nonprecise = candidates.Count(c => !c.Location.IsPrecise());
                if (nonprecise > 1)
                {
                    throw ParseError.New(Messages.ParseError_MultiplePreciseLocations, nonprecise, issue.Location.LineNumber);
                }
                else if (candidates.Any(c => c.Location == issue.Location))
                {
                    throw ParseError.New(Messages.ParseError_RedundentPreciseLocation, issue.Location.LineNumber);
                }
                else if (candidates.FirstOrDefault(c =>! c.Location.IsPrecise()) is { } candidate)
                {
                    issues.Remove(candidate);
                    issues.Add(candidate.Update(issue.Location));
                    return true;
                }
            }
            return false;
        }

        private IEnumerable<ExpectedIssue> PerDiagnosticId(ExpectedIssue issue)
            => issue.DiagnosticId
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .OrderBy(id => id)
            .Select(id => new ExpectedIssue(id, issue.Type, issue.Message, issue.Location));

        /// <inheritdoc />
        public IEnumerator<ExpectedIssue> GetEnumerator() => issues.GetEnumerator();

        /// <inheritdoc />
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
