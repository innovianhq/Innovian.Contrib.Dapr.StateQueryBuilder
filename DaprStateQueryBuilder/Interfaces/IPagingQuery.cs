//  -------------------------------------------------------------
//  Copyright (c) 2023 Innovian Corporation. All rights reserved.
//  -------------------------------------------------------------

using System.Linq.Expressions;
using Innovian.Contrib.Dapr.StateQueryBuilder.Models.Enums;

namespace Innovian.Contrib.Dapr.StateQueryBuilder.Interfaces;

public interface IPagingQuery<T>
{
    ISortByQuery<T> Where(Func<FilterQuery<T>, IFinishedFilterQuery> filterAction);
    ISortByQuery<T> Sort(Expression<Func<T, string>> propertyName, Ordering? direction = null);
}