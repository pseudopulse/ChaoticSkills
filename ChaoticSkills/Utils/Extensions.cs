using System;

namespace ChaoticSkills.Utils {
    public static class StringExtensions {
        public static void Add(this string token, string text) {
            LanguageAPI.Add(token, text);
        }
    }
}