//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;
using Innovian.Contrib.Dapr.StateQueryBuilder.Models.Enums;
using Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Data;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Models;

[TestClass]
public class DocumentationTests
{
    [TestMethod]
    public void Example1()
    {
        var opt = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var result = new QueryBuilder<SampleData>(opt)
            .Where(data => data.Eq(prop => prop.State, "CA"))
            .Sort(prop => prop.Person.Id, Ordering.Descending)
            .Build()
            .ToString();

        const string expected = """
                                {
                                  "filter": {
                                    "EQ": {
                                      "state": "CA"
                                    }
                                  },
                                  "sort": [
                                    {
                                      "key": "person.id",
                                      "order": "DESC"
                                    }
                                  ]
                                }
                                """;

        Assert.AreEqual(expected.Trim(), result.Trim());
    }

    [TestMethod]
    public void Example2()
    {
        var opt = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var result = new QueryBuilder<SampleData>(opt)
            .Where(data => data.In(prop => prop.Person.Org, "Dev Ops", "Hardware"))
            .Build()
            .ToString();

        const string expected = """
                                {
                                  "filter": {
                                    "IN": {
                                      "person.org": [
                                        "Dev Ops",
                                        "Hardware"
                                      ]
                                    }
                                  }
                                }
                                """;

        Assert.AreEqual(expected.Trim(), result.Trim());
    }

    [TestMethod]
    public void Example3()
    {
        var opt = new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        var result = new QueryBuilder<SampleData>(opt)
            .WithPaging(3)
            .Where(b => b
                .Or(new FilterQuery<SampleData>().Eq(a => a.Person.Org, "Dev Ops"),
                    new FilterQuery<SampleData>()
                        .And(new FilterQuery<SampleData>().Eq(a => a.Person.Org, "Finance"),
                            new FilterQuery<SampleData>().In(a => a.State, "CA", "WA"))))
            .Sort(a => a.State, Ordering.Descending)
            .Sort(a => a.Person.Id)
            .Build()
            .ToString();

        const string expected = """
                                {
                                  "filter": {
                                    "OR": [
                                      {
                                        "EQ": {
                                          "person.org": "Dev Ops"
                                        }
                                      },
                                      {
                                        "AND": [
                                          {
                                            "EQ": {
                                              "person.org": "Finance"
                                            }
                                          },
                                          {
                                            "IN": {
                                              "state": [
                                                "CA",
                                                "WA"
                                              ]
                                            }
                                          }
                                        ]
                                      }
                                    ]
                                  },
                                  "sort": [
                                    {
                                      "key": "state",
                                      "order": "DESC"
                                    },
                                    {
                                      "key": "person.id",
                                      "order": "ASC"
                                    }
                                  ],
                                  "page": {
                                    "limit": 3
                                  }
                                }
                                """;

        Assert.AreEqual(expected.Trim(), result.Trim());
    }
}