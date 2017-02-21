using System;

namespace Profile.UI.Utility
{
    public class WordTransformer
    {
        private const string NounEndingFirstCase = "ов";
        private const string NounEndingSecondCase = "а";

        public string GetNounCase(int count, string noun)
        {
            if (count < 0 || count > 999)
            {
                var errorMsg = string.Format("{0} is not a valid nouns count, because the range is limited to 0..999",
                                             count);
                throw new ArgumentOutOfRangeException(errorMsg);
            }

            if (count == 0) return noun + NounEndingFirstCase;
            var rest = count % 100;

            if ((rest > 10) && (rest < 20))
            {
                return noun + NounEndingFirstCase;
            }
            else
            {
                rest = rest % 10;
                if (rest == 0) return noun + NounEndingFirstCase;
                if (rest == 1) return noun;

                if ((rest > 1) && (rest < 5))
                {
                    return noun + NounEndingSecondCase;
                }
                
                 return noun + NounEndingFirstCase;
            }
        }
    }
}