﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Guan.Logic
{
    using System.Threading.Tasks;

    /// <summary>
    /// Predicate type for "not".
    /// </summary>
    internal class NotPredicateType : PredicateType
    {
        public static readonly NotPredicateType Singleton = new NotPredicateType();

        private static readonly EvaluatedFunctor NotConstraint = new EvaluatedFunctor(NotFunc.Singleton);

        private NotPredicateType()
            : base("not", true, 1, 1)
        {
        }

        public override PredicateResolver CreateResolver(CompoundTerm input, Constraint constraint, QueryContext context)
        {
            return new Resolver(input, constraint, context);
        }

        public override void AdjustTerm(CompoundTerm term, Rule rule)
        {
            base.AdjustTerm(term, rule);

            if (!(term.Arguments[0].Value is CompoundTerm))
            {
                throw new GuanException("Invalid use of not predicate");
            }

            CompoundTerm goal = (CompoundTerm)term.Arguments[0].Value;
            if (goal.Functor is EvaluatedFunctor)
            {
                term.Functor = NotConstraint.ConstraintType;
            }
            else if (!(goal.Functor is PredicateType))
            {
                throw new GuanException("Predicate type {0} in {1} is not defined", goal.Functor.Name, term);
            }
        }

        private class Resolver : PredicateResolver
        {
            public Resolver(CompoundTerm input, Constraint constraint, QueryContext context)
                : base(input, constraint, context, 1)
            {
            }

            public override async Task<UnificationResult> OnGetNextAsync()
            {
                CompoundTerm goal = (CompoundTerm)this.Input.Arguments[0].Value;
                PredicateResolver resolver = goal.PredicateType.CreateResolver(goal, this.Constraint, this.Context);
                UnificationResult result = await resolver.GetNextAsync();
                if (result == null)
                {
                    return UnificationResult.Empty;
                }

                return null;
            }
        }
    }
}
