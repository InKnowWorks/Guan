﻿//---------------------------------------------------------------------------------------------------------------------
// Copyright (c) Microsoft Corporation.  All rights reserved.
//---------------------------------------------------------------------------------------------------------------------
namespace Guan.Logic
{
    /// <summary>
    /// A variable bound to another variable.
    /// </summary>
    internal class LinkedVariable : Variable
    {
        private Variable original;

        public LinkedVariable(VariableBinding binding, Variable original, string name)
            : base(name, binding)
        {
            this.original = original;
        }

        public LinkedVariable(LinkedVariable other, VariableBinding binding)
            : base(other, binding)
        {
            this.original = other.original;
        }

        public Variable Original
        {
            get
            {
                return this.original;
            }

            set
            {
                this.original = value;
            }
        }
    }
}
