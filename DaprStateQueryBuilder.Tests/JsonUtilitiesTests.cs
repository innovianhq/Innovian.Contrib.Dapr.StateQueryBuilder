//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Data;
using Innovian.Contrib.Dapr.StateQueryBuilder.Utilities;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Tests;

[TestClass]
public class JsonUtilitiesTests
{
    [TestMethod]
    public void GetJsonPropertyNameHandlesNestedValues()
    {
        var result = JsonNameHelpers<SampleData>.GetJsonPropertyName(b => b.State);
        Assert.AreEqual("state", result);
    }

    [TestMethod]
    public void GetJsonPropertyNameHandlesSimpleValues()
    {
        var result = JsonNameHelpers<SampleData>.GetJsonPropertyName(b => b.Person.Org);
        Assert.AreEqual("person.org", result);
    }
}