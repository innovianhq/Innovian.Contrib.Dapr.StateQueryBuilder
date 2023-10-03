//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using System.Text.Json.Serialization;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Tests.Data;

public record SampleData([property: JsonPropertyName("person")] Person Person, [property: JsonPropertyName("city")] string City, [property: JsonPropertyName("state")] string State);

public record Person([property: JsonPropertyName("org")] string Org, [property: JsonPropertyName("id")] string Id);