using System;

namespace TwitterQuiz.Domain.Account
{
    public enum Gender
    {
        Unknown = 0,
        Male = 1,
        Female = 2
    }

    public static class GenderTypeHelpers
    {
        public static Gender ToGenderType(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentNullException("value");
            }

            switch (value.ToLowerInvariant())
            {
                case "male": return Gender.Male;
                case "female": return Gender.Female;
                default: return Gender.Unknown;
            }
        }
    }
}