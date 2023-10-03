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
(currently in an Alpha stage) as described [here](https://docs.dapr.io/developing-applications/building-blocks/state-management/howto-state-query-api/), including the filter, sort and page functionality. There are several examples demonstrating use of these in the test project, but I'll focus on recreating the examples in the Dapr documentation here. Do note that while I'm using the `Trim()` method in the linked unit tests with the value, this isn't really necessary in normal practice - it's necessary here so extraneous whitespace doesn't mess with the expected unit test values as a result of the indentation.

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
The approach is quite simple. In the first documentation [example](https://docs.dapr.io/developing-applications/building-blocks/state-management/howto-state-query-api/#example-1), we're going to filter where the "state' property equals "CA" and then sort in descending order by the "person.id" property. For ease of readability for the output, I'll use my own JsonSerializerOptions and set the WriteIndented property to true.

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
- The `ToString` method is overridden in the implementation to actually perform the serialization of the underlying objects, so while `Build()` is a useful part of indicating to the builer that you're finished creating the structure, it's the use of `ToString()` that actually yields the JSON query string. While the use of `Build` isn't strictly necessary at this point, it's recommended to use it because the Dapr query state API is only in an alpha state. Because everything is subject to change, the use of `Build()` might become required in the future. Regardless, you'll find that some unit tests exclude it primarily for testing purposes.

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

### Example 2
Here, we'll look at the slightly more complicated [example 2](https://docs.dapr.io/developing-applications/building-blocks/state-management/howto-state-query-api/#example-2) from the Dapr documentation. This example seeks only to show off the `In` capability. Unlike an `Eq` filter which compares a given property on the object to a single value, an `In` filter compares that specified property on the object to any of a collection of values. This example in particular is going to use the `person.org` property from our `SampleData` type above and use an `In` filter to compare its value any of those in the following list: either "Dev Ops" or "Hardware".

You'll see the code is quite similar to our first example:

```cs
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
}
```

As before, we start by defining our `JsonSerializerOptions`. We pass that into the constructor of our QueryBuilder and once again use the `SampleData` type parameter, indicating that the property selection will be performed against that type.  Where in the last example we used `Eq` for the filter, here we'll instead use `In`. We'll start by specifying which property should be used in the statement and then enter the string value(s) to match to with as many values as you wish. Again, we mark the end of the query with `Build()` and then render it to a JSON string with the use of the `ToString()` override. As expected, this yields the following output:

```json
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
```

### Example 3
This one is a little bit more complex as we'll combine nested filtering, sorting and paging to match the example given in the [Dapr documentation](https://docs.dapr.io/developing-applications/building-blocks/state-management/howto-state-query-api/#example-3). Here, we'll find all the employees in the "Dev Ops" department OR all the employees from the "Finance" department that reside in the states of either Washington or California. Finally, we'll limit the result to only 3 records at a time and then sort the results first by state in descending alphabetical order, then by employee ID in ascending order.

Here's what the C# for this looks like:
```cs
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
}
```

Once more we start with the `JsonSerializerOptions` simply so the output is indented for our purposes here. We then start with another instance of the `QueryBuilder` to which we again specify the type we're building the filter against and pass in our `JsonSerializerOptions` as the constructor parameter.  We then apply the paging options we want to. In this example, we're going to limit the number of returned values to 3, so we've specified as much here. If we were given a continuation token from a previous query, we could also specify that here as either a named argument for the "continuationToken" or as the second argument if also providing a paging limit. 

On the next line, we start the filtering. Note that instead of immediately diving into an `Eq` or `In` method, we'll instead specify an `Or` as that's the outermost value of the query we're seeking to build. An `Or` method accepts any number of typed `FilterQuery` objects that themselves may introduce further nested `And`s or `Or`s or may themselves end the nesting by specifying either `Eq` or `In`. Here, we immediately specify that `Person.Org` (or `person.org` when accounting for the property names from the `JsonPropertyName` attributes on the record properties) should equal "Dev Ops" per our requirements. In the next argument, we'll specify another FilterQuery, but we'll again immediately nest an `And` within this and specify both an `Eq` between "Person.Org" and "Finance" and then an `In" between "State" and either "CA" or "WA", again per the requirements.

Finally, the filter completed, we'll wrap up with the Sort methods. Sorts are applied in the order specified, so as our requirements are to sort by the state in descending order, we'll indicate as much). On the next time, we'll sort by the "person.id" in the default order (which the docs indicate is ascending order). If preferred, you could just as well have specified `Ordering.Ascending` here as well.

Per the recommendation, we finalize with `Build()` and render it to JSON with `ToString() to get the following:
```json
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
```

## Running unit tests
All the unit tests were written using MSTestv2 and can be run using your favorite test runner from the `Innovian.Contrib.Dapr.StateQueryBuilder.Tests` project.

## Contributing
Contributions are welcome. Please read our [contributing guide](./CONTRIBUTING.md) to learn more about filing issues and submitting PRs.

## License
[Innovian.Contrib.Dapr.StateQueryBuilder](https://github.com/innovianhq/Innovian.Contrib.Dapr.StateQueryBuilder) is licensed for use under the [Apache 2.0 license](./LICENSE)
