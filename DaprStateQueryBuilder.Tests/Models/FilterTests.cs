//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Data;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Models;
  
[TestClass]
public class QueryBuilderTests
{
    [TestMethod]
    public void SingleAndTest()
    {
        var result = new QueryBuilder<SampleData>()
            .Where(b => b
                .And(
                    new FilterQuery<SampleData>().Eq(c => c.State, "TX"),
                    new FilterQuery<SampleData>().Eq(c => c.City, "Austin")))
            .ToString();
        const string expected = """
                                {"filter":{"AND":[{"EQ":{"state":"TX"}},{"EQ":{"city":"Austin"}}]}}
                                """;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SingleOrTest()
    {
        var result = new QueryBuilder<SampleData>()
            .Where(b => b
                .Or(new FilterQuery<SampleData>().Eq(c => c.Person.Org, "Dev Ops"),
                    new FilterQuery<SampleData>().In(c => c.State, "CA", "TX", "WA", "CO")))
            .ToString();
        const string expected = """
                                {"filter":{"OR":[{"EQ":{"person.org":"Dev Ops"}},{"IN":{"state":["CA","TX","WA","CO"]}}]}}
                                """;
        Assert.AreEqual(expected, result);
    }

    [TestMethod]
    public void SimpleEqTest()
    {
        var result = new QueryBuilder<SampleData>();
        result.Where(b => b.Eq(a => a.State, "CA"));
        var strResult = result.ToString();
        const string expected = """
                                {"filter":{"EQ":{"state":"CA"}}}
                                """;
        Assert.AreEqual(expected, strResult);
    }

    [TestMethod]
    public void SimpleInTest()
    {
        var result = new QueryBuilder<SampleData>();
        result.Where(b => b.In(a => a.State, "CA", "TX", "OR"));
        var strResult = result.ToString();
        const string expected = """
                       {"filter":{"IN":{"state":["CA","TX","OR"]}}}
                       """;
        Assert.AreEqual(expected, strResult);
    }
}