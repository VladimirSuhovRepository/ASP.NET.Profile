using System;

namespace StyleCop.SpecialRules.Tests.Test_data
{
    internal class ConditionalStatementsCurlyBracketsValid
    {
        internal void ValidIfStatement()
        {
            int number = 22;

            if (number > 11)
            {
                number *= 2;
            }

            number += 1;
        }

        internal void ValidIfStatementWithExpressionSingleLine()
        {
            int number = 22;

            if (number > 11) number *= 2;

            number += 1;
        }

        internal void ValidIfStatementWithReturnSingleLine()
        {
            int number = 22;

            if (number > 11) return;

            number += 1;
        }

        internal void ValidIfStatementWithBreakSingleLine()
        {
            for (int i = 0; i < 10; i++)
            {
                if (i == 5) break;
            }
        }

        internal void ValidIfStatementWithThrowSingleLine()
        {
            int number = 22;

            if (number > 11) throw new Exception();

            number += 1;
        }

        internal void ValidIfElseStatement()
        {
            int number = 22;

            if (number > 11)
            {
                number *= 2;
            }
            else
            {
                number = 0;
            }

            number += 1;
        }
    }
}
