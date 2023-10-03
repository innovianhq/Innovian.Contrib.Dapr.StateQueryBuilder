//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Data;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Models;

[TestClass]
public class PagingTests
{
    [TestMethod]
    [TestCategory("Paging")]
    public void PageWithSpecifiedLimit()
    {
        var result = new QueryBuilder<SampleData>()
            .WithPaging(30)
            .ToString();
        const string expected = """
                                {"page":{"limit":30}}
                                """;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [TestCategory("Paging")]
    public void PageWithContinuationToken()
    {
        var result = new QueryBuilder<SampleData>()
            .WithPaging(continuationToken: "abc123")
            .ToString();
        const string expected = """
                                {"page":{"token":"abc123"}}
                                """;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    [TestCategory("Paging")]
    public void PageWithAllOptions()
    {
        var result = new QueryBuilder<SampleData>()
            .WithPaging(30, "abc123")
            .ToString();
        const string expected = """
                                {"page":{"limit":30,"token":"abc123"}}
                                """;
        Assert.AreEqual(expected, result);
    }
}