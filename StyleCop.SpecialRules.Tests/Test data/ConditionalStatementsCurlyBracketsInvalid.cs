namespace StyleCop.SpecialRules.Tests.Test_data
{
    internal class ConditionalStatementsCurlyBracketsInvalid
    {
        internal void InvalidIfStatement()
        {
            int number = 22;

            if (number > 11)
                number *= 2;

            number += 1;
        }

        internal void InvalidElseStatement()
        {
            int number = 22;

            if (number > 11)
            {
                number *= 2;
            }
            else
                number = 0;

            number += 1;
        }

        internal void InvalidIfElseStatementSingleLine()
        {
            int number = 22;

            if (number > 11) number *= 2;
            else number = 0;

            number += 1;
        }

        internal void InvalidIfElseStatement()
        {
            int number = 22;

            if (number > 11)
                number *= 2;
            else
                number = 0;

            number += 1;
        }
    }
}
