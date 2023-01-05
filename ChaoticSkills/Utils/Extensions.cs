using System;

namespace ChaoticSkills.Utils {
    public static class StringExtensions {
        public static void Add(this string token, string text) {
            LanguageAPI.Add(token, text);
        }

        public static void RemoveComponent<T>(this GameObject gameObject) where T : Component {
            GameObject.Destroy(gameObject.GetComponent<T>());
        }

        public static void RemoveComponents<T>(this GameObject gameObject) where T : Component {
            T[] coms = gameObject.GetComponents<T>();
            for (int i = 0; i < coms.Length; i++) {
                GameObject.Destroy(coms[i]);
            }
        }
    }
}