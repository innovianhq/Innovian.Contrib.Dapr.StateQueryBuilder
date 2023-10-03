//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using System.Linq.Expressions;
using Innovian.Contrib.Dapr.StateQueryBuilder.Models.Enums;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Interfaces;

public interface ISortByQuery<T>
{
    ISortByQuery<T> Sort(Expression<Func<T, string>> propertyName, Ordering? direction = null);
    IFinishedQueryBuilder<T> Build();
}