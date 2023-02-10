using System;

namespace ChaoticSkills.Utils {
    public static class StringExtensions {
        private static string[] badChars = {
            "<style=cIsVoid>", "</style>", "\\", ".", "`", ":", ";", "_", "]", "[", "}", "{", "?", "!", "-" , "'", "\n", "\t"
        };
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

        public static T GetRandom<T>(this List<T> list, Xoroshiro128Plus rng = null) {
            if (list.Count == 0) {
                return default(T);
            }
            if (rng == null) {
                return list[UnityEngine.Random.RandomRangeInt(0, list.Count)];
            }
            else {
                return list[rng.RangeInt(0, list.Count)];
            }
        }

        public static T GetRandom<T>(this List<T> list, Func<T, bool> pred, Xoroshiro128Plus rng = null) {
            if (list.Where(pred).Count() == 0) {
                return default(T);
            }
            return list.Where(pred).ToList().GetRandom(rng);
        }

        public static void AddComponent<T>(this Component self) where T : Component {
            self.gameObject.AddComponent<T>();
        }

        public static string Filter(this string str, string[] badText = null) {
            string filtered = str;
            if (badText == null) {
                foreach (string badChar in badChars) {
                    filtered = filtered.Replace(badChar, "");
                }
            }
            else {
                foreach (string badChar in badText) {
                    filtered = filtered.Replace(badChar, "");
                }
            }
            // Debug.Log(filtered);
            return filtered;
        }

        public static bool HasSkillEquipped(this CharacterBody body, SkillDef skill) {
            foreach (GenericSkill slot in body.GetComponents<GenericSkill>()) {
               //  Debug.Log(slot.skillDef);
                if (slot.skillDef == skill) {
                    // Debug.Log("trur");
                    return true;
                }
            }
            return false;
        }

        public static void AddStateMachine<T>(this GameObject self, string name, bool enabled = true) where T : EntityState {
            EntityStateMachine machine = self.AddComponent<EntityStateMachine>();
            machine.customName = name;
            machine.mainStateType = new SerializableEntityStateType(typeof(T));
            machine.initialStateType = new SerializableEntityStateType(typeof(T));
            machine.enabled = enabled;
            NetworkStateMachine machines = self.GetComponent<NetworkStateMachine>();
            if (machines) {
                List<EntityStateMachine> list = machines.stateMachines.ToList();
                list.Add(machine);
                machines.stateMachines = list.ToArray();
            }
        }
    }
}