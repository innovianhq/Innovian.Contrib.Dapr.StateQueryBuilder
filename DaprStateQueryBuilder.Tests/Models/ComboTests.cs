//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using Innovian.Contrib.Dapr.StateQueryBuilder.Models.Enums;
using Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Data;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Models;

[TestClass]
public class ComboTests
{
    [TestMethod]
    [TestCategory("Paging")]
    [TestCategory("Sorting")]
    public void PagingAndSortingWithContinuationToken()
    {
        var result = new QueryBuilder<SampleData>()
            .WithPaging(30, "abc123")
            .Sort(c => c.Person.Org)
            .ToString();
        const string expected = """
                                {"sort":[{"key":"person.org","order":"ASC"}],"page":{"limit":30,"token":"abc123"}}
                                """;

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [TestCategory("Paging")]
    [TestCategory("Filtering")]
    public void PagingAndFiltering()
    {
        var result = new QueryBuilder<SampleData>()
            .WithPaging("abc123")
            .Where(c => c.Eq(b => b.State, "TX"))
            .ToString();
        const string expected = """
                                {"filter":{"EQ":{"state":"TX"}},"page":{"token":"abc123"}}
                                """;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [TestCategory("Sorting")]
    [TestCategory("Filtering")]
    public void SortingAndFiltering()
    {
        var result = new QueryBuilder<SampleData>()
            .Where(a => a.In(b => b.State, "TX", "IN", "CA", "NV"))
            .Sort(a => a.Person.Org)
            .ToString();

        const string expected = """
                                {"filter":{"IN":{"state":["TX","IN","CA","NV"]}},"sort":[{"key":"person.org","order":"ASC"}]}
                                """;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [TestCategory("Paging")]
    [TestCategory("Sorting")]
    public void PagingAndSorting()
    {
        var result = new QueryBuilder<SampleData>()
            .WithPaging(30)
            .Sort(c => c.Person.Org)
            .ToString();

        const string expected = """
                                {"sort":[{"key":"person.org","order":"ASC"}],"page":{"limit":30}}
                                """;

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [TestCategory("Paging")]
    [TestCategory("Sorting")]
    [TestCategory("Filtering")]
    public void FullTest()
    {
        var result = new QueryBuilder<SampleData>()
            .WithPaging(3)
            .Where(b => b
                .Or(new FilterQuery<SampleData>().Eq(a => a.Person.Org, "Dev Ops"),
                    new FilterQuery<SampleData>()
                        .And(new FilterQuery<SampleData>().Eq(a => a.Person.Org, "Finance"),
                            new FilterQuery<SampleData>().In(a => a.State, "CA", "WA"))))
            .Sort(a => a.State, Ordering.Descending)
            .Sort(a => a.Person.Id)
            .ToString();
        const string expected = """
                                {"filter":{"OR":[{"EQ":{"person.org":"Dev Ops"}},{"AND":[{"EQ":{"person.org":"Finance"}},{"IN":{"state":["CA","WA"]}}]}]},"sort":[{"key":"state","order":"DESC"},{"key":"person.id","order":"ASC"}],"page":{"limit":3}}
                                """;

        Assert.AreEqual(expected, result);
    }
}
