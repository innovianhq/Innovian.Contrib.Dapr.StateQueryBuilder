//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using System.Text.Json.Serialization;
using Innovian.Contrib.Dapr.StateQueryBuilder.JsonConverters;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Models.Enums;

[JsonConverter(typeof(OrderingJsonConverter))]
public enum Ordering
{
    Ascending,
    Descending
}