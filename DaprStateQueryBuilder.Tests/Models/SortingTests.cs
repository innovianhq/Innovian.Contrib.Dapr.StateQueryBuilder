//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using Innovian.Contrib.Dapr.StateQueryBuilder.Models.Enums;
using Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Data;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Models;

[TestClass]
public class SortingTests
{
    [TestMethod]
    [TestCategory("Sorting")]
    public void SortByNestedPropertyAscTest()
    {
        var result = new QueryBuilder<SampleData>()
            .Sort(c => c.Person.Id, Ordering.Ascending)
            .ToString();
        const string expected = """
                                {"sort":[{"key":"person.id","order":"ASC"}]}
                                """;

        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void OrderingShouldDefaultToAscending()
    {
        var result = new QueryBuilder<SampleData>()
            .Sort(a => a.City)
            .ToString();
        const string expected = """
                                {"sort":[{"key":"city","order":"ASC"}]}
                                """;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [TestCategory("Sorting")]
    public void SortByDirectPropertyDescTest()
    {
        var result = new QueryBuilder<SampleData>()
            .Sort(c => c.State, Ordering.Descending)
            .ToString();
        const string expected = """
                                {"sort":[{"key":"state","order":"DESC"}]}
                                """;
        Assert.AreEqual(expected, result);
    }
}