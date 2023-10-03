# Innovian.Contrib.Dapr.StateQueryBuilder

This is a contribution to the .NET community of Dapr, providing a fluent API for building out the appropriate JSON for querying state stores as described [here](https://docs.dapr.io/developing-applications/building-blocks/state-management/howto-state-query-api/). As some of these nested filters could get a little unwieldy to write by hand, 
I wanted to take a stab at creating a fluent API capable of doing so for me and my experiment was successful. Thus, I'm sharing the fruits of my efforts with the larger community.

Innovian is not associated with Dapr.

## Installation
Using the .NET Core CLI tools:
```sh
dotnet add package Innovian.Contrib.Dapr.StateQueryBuilder
```

Using the NuGet CLI:
```sh
nuget install Innovian.Contrib.Dapr.StateQueryBuilder
```

Using the Package Manager Console:
```powershell
Install-Package Innovian.Contrib.Dapr.StateQueryBuilder
```

From within Visual Studio:

1. Open the Solution Explorer.
2. Right-click on a project within your solution.
3. Click on "Manage NuGet Packages...".
4. Click on the "Browse" tab and search for "Innovian.Contrib.Dapr.StateQueryBuilder".
5. Click on the Innovian.Contrib.Dapr.StateQueryBuilder package, select the appropriate version in the right-tab and click *Install*.

## Usage

This is a fluent API, meaning that the idea is that create a new instance of the query builder and then iteratively use method chaining to append further criteria to the end of it, ultimately returning an object that creates whatever it was you were intending to build. This project fully implements the Dapr query state functionality 
(currently in an Alpha stage) as described [here](https://docs.dapr.io/developing-applications/building-blocks/state-management/howto-state-query-api/), including the filter, sort and page functionality. There are several examples demonstrating use of these in the test project, but I'll focus on recreating the examples in the Dapr documentation here.

First, we're going to need a type to query. This can be either a record or class and ours will take the following shape:

```cs
public record SampleData([property: JsonPropertyName("person")] Person Person, [property: JsonPropertyName("city")] string City, [property: JsonPropertyName("state")] string State);

public record Person([property: JsonPropertyName("org")] string Org, [property: JsonPropertyName("id")] string Id);
```

A couple of things to note:
- This project uses System.Text.Json internally.
- I'm using the `JsonPropertyName` attribute on the record properties to override the default property names that `System.Text.Json` would normally use as the example here in order to force them to be strictly lower-cased to match the Dapr documentation examples. Feel free to pass your
- own `JsonSerializationOptions` into the QueryBuilder constructor to provide your own serialization preferences (e.g. use indenting, allow trailing commas, use camel-casing without attributes, etc.).
- I highly recommend that if you use your own `JsonSerializerOptions` instance, you set `DefaultIgnoreCondition` to `JsonIgnoreCondition.WhenWritingNull` as the fluent builder intends this to be set so as to prune off unintended branches.

### Example 1
The approach is quite simple. In the first documentation example, we're going to filter where the "state' property equals "CA" and then sort in descending order by the "person.id" property. For ease of readability for the output, I'll use my own JsonSerializerOptions and set the WriteIndented property to true.

We'll start with the following:
```cs
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
    .Build();
}
```

And with that we're effectively done, but let me provide a brief explanation to each line.
- We start by instantiating a new `JsonSerializerOptions` instance and setting the two properties we desire.
- Create a new QueryBuilder instance and specify the type you intend to query the Dapr storage state for as the type param. Optionally pass in your `JsonSerializerOptions` as the only argument on the constructor.
- If you were going to page the values at all, you'd do so here, but in this example, we'll skip that and revisit in a later example.
- You may then specify zero or more uses of `.Where` to add more filters to your query. This also supports nested filters, but we'll get there in a later example. You can specify either "Equals" (single comparison value) or "In" (collection of comparison value) and within that method, indicate the property the condition should be applied to and the value(s) to compare to it.
- Following any `Where` filters, you can then specify zero or more `Sort` queries that allow to you indicate a property (root or nested) on the type you indicated and indicate whether the sorting should be applied in an ascending or descending manner.

Matching the example in the documentation, `result` now has a value as follows:

```json
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
```
