﻿/*
 * SonarAnalyzer for .NET
 * Copyright (C) 2015-2016 SonarSource SA
 * mailto:contact@sonarsource.com
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 3 of the License, or (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
 * Lesser General Public License for more details.
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02
 */

using System;
using System.Collections.Generic;
using System.Linq;

namespace SonarAnalyzer.Helpers.FlowAnalysis.Common
{
    public class AndSymbolicValue : BinarySymbolicValue
    {
        public AndSymbolicValue(SymbolicValue leftOperand, SymbolicValue rightOperand)
            : base(leftOperand, rightOperand)
        {
        }

        public override IEnumerable<ProgramState> TrySetConstraint(SymbolicValueConstraint constraint, ProgramState currentProgramState)
        {
            var boolConstraint = constraint as BoolConstraint;
            if (boolConstraint == null)
            {
                return new[] { currentProgramState };
            }

            if (boolConstraint == BoolConstraint.True)
            {
                return LeftOperand.TrySetConstraint(BoolConstraint.True, currentProgramState)
                    .SelectMany(ps => RightOperand.TrySetConstraint(BoolConstraint.True, ps));
            }

            return LeftOperand.TrySetConstraint(BoolConstraint.True, currentProgramState)
                    .SelectMany(ps => RightOperand.TrySetConstraint(BoolConstraint.False, ps))
                .Union(LeftOperand.TrySetConstraint(BoolConstraint.False, currentProgramState)
                    .SelectMany(ps => RightOperand.TrySetConstraint(BoolConstraint.True, ps)))
                .Union(LeftOperand.TrySetConstraint(BoolConstraint.False, currentProgramState)
                    .SelectMany(ps => RightOperand.TrySetConstraint(BoolConstraint.False, ps)));
        }

        public override string ToString()
        {
            return LeftOperand + " & " + RightOperand;
        }
    }
}
