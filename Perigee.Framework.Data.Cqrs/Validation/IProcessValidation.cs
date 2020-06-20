﻿namespace Perigee.Framework.Data.Cqrs.Validation
{
    using FluentValidation.Results;
    using Transactions;

    public interface IProcessValidation
    {
        ValidationResult Validate(IDefineCommand command);
        ValidationResult Validate<TResult>(IDefineQuery<TResult> query);
    }
}