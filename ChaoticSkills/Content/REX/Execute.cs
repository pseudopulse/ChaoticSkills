using System;
using RoR2.UI;
using RoR2;
using TMPro;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Reflection;
using UnityEngine.UIElements;
using Button = RoR2.UI.HGButton;
using Image = UnityEngine.UI.Image;
using IL.RoR2.Orbs;
using System.Globalization;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis;
using System.IO;
using Microsoft.CodeAnalysis.Emit;
using Newtonsoft.Json.Utilities;
using System.Collections.Generic;
using System.Linq;

namespace ChaoticSkills.Content.REX {
    public class Execute : SkillBase<Execute>
    {
        public override SerializableEntityStateType ActivationState => ContentAddition.AddEntityState<EntityStates.REX.Execute>(out bool _);

        public override float Cooldown => 7f;

        public override string Machine => "Weapon";

        public override int MaxStock => 1;

        public override string LangToken => "Execute";

        public override string Name => "DIRECTIVE: Execute";

        public override string Description => "Use your integrated processing unit to <style=cIsUtility>execute arbitrary code</style>.";

        public override Sprite SkillIcon => null;

        public override SkillSlot Slot => SkillSlot.Special;

        public override string Survivor => Utils.Paths.GameObject.TreebotBody;

        public override bool AutoApply => true;

        public override bool Agile => false;

        public override bool IsCombat => false;

        public override bool SprintCancelable => false;
        public override bool ForceOff => true;

        public static GameObject UIPrefab;
        public static GameObject InputTextPrefab;
        public static GameObject ErrorPrefab;
        public static Dictionary<CharacterBody, Assembly> Assemblies = new();
        public static string BaseCode = 
        @"
        using System;
        using RoR2;
        using UnityEngine;

        namespace ExecuteSkill {
            public class Directive {
                public static void Main(CharacterBody body) {
                    
                }
            }
        }
        ";


        public static Color32 SyntaxRed = new(255, 0, 0, 255);
        public static Color32 SyntaxBlue = new(0, 0, 255, 255);
        public static Color32 SyntaxGreen = new(0, 255, 0, 255);
        public static Color32 SyntaxYellow = new(255, 255, 0, 255);
        public static Color32 SyntaxPurple = new(255, 0, 255, 255);

        public static Dictionary<List<string>, Color32> SyntaxHex = new() {
            {new() { "using", "namespace", "public", "static", "override", "new", "void", "int", "float", "string" }, SyntaxPurple},
            {new() { "(", ")", "{", "}" }, SyntaxBlue},
            {new() { "if", "else", "for", "while", "return" }, SyntaxPurple},
            {new() { "true", "false", "null" }, SyntaxGreen},
            {new() { "this", "base" }, SyntaxPurple},
            {new() { "\"", "@", "\'"}, SyntaxGreen}
        };

        public override void PostCreation()
        {
            UIPrefab = PrefabAPI.InstantiateClone(Utils.Paths.GameObject.ScrapperPickerPanel.Load<GameObject>(), "ExecuteUI");
            UIPrefab.AddComponent<ExecuteUIController>();
            UIPrefab.RemoveComponent<MPButton>();

            On.RoR2.UI.MainMenu.MainMenuController.Start += (orig, self) => {
                orig(self);
                
                GameObject baseObj = GameObject.Find("MainMenu").transform.Find("MENU:  Signup").Find("Signup Menu").Find("PopupPanel").Find("SubmitPanel").Find("ContentArea").Find("Panel").Find("EmailInputField").gameObject;

                InputTextPrefab = PrefabAPI.InstantiateClone(baseObj, "ExecuteInput");
                ErrorPrefab = PrefabAPI.InstantiateClone(baseObj, "ExecuteError");

                ErrorPrefab.RemoveComponent<TMP_InputField>();

                ErrorPrefab.GetComponent<Image>().color = Color.black;
                InputTextPrefab.GetComponent<Image>().color = Color.black;

                ErrorPrefab.transform.Find("Text Area").Find("Placeholder").gameObject.SetActive(false);
                InputTextPrefab.transform.Find("Text Area").Find("Placeholder").gameObject.SetActive(false);

                ErrorPrefab.transform.Find("Text Area").Find("Text").GetComponent<HGTextMeshProUGUI>().text = "";
                ErrorPrefab.transform.Find("Text Area").Find("Text").GetComponent<HGTextMeshProUGUI>().color = Color.red;
                InputTextPrefab.transform.Find("Text Area").Find("Text").GetComponent<HGTextMeshProUGUI>().color = Color.white;
            };
        }

        // TODO: add mscorlib reference to compilation

        public static string[] Compile(string code, CharacterBody body) {
            Debug.Log("beginning compilation");
            CSharpParseOptions options = CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.CSharp9);
            SyntaxTree tree = CSharpSyntaxTree.ParseText(code, options);

            CSharpCompilationOptions compOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
                .WithOptimizationLevel(OptimizationLevel.Release)
                .WithPlatform(Platform.AnyCpu)
                .WithAssemblyIdentityComparer(DesktopAssemblyIdentityComparer.Default)
                .WithAllowUnsafe(true)
                .WithWarningLevel(0);

            CSharpCompilation comp = CSharpCompilation.Create(System.IO.Path.GetRandomFileName())
                .WithOptions(compOptions)
                .AddReferences(AppDomain.CurrentDomain.GetAssemblies().Where(a => !a.IsDynamic && !string.IsNullOrWhiteSpace(a.Location)).Select(a => MetadataReference.CreateFromFile(a.Location)))
                .AddSyntaxTrees(tree);

            using (MemoryStream stream = new MemoryStream()) {
                EmitResult res = comp.Emit(stream);

                if (!res.Success) {
                    List<string> errors = new();

                    foreach (Diagnostic diag in res.Diagnostics) {
                        errors.Add(diag.ToString());
                    }

                    return errors.ToArray();
                }

                stream.Seek(0, SeekOrigin.Begin);
                Assembly assembly = Assembly.Load(stream.ToArray());
                Debug.Log(assembly.FullName);
                Assemblies[body] = assembly;

                Module module = assembly.GetModules()[0];
                if (module != null) {
                    Type type = module.GetType("ExecuteSkill.Directive");

                    if (type != null) {
                        MethodInfo method = type.GetMethod("Main");

                        if (method == null || !method.IsStatic) {
                            return new string[] { "No static Main method under ExecuteSkill.Directive class." };
                        }
                    }
                    else {
                        return new string[] { "No Directive class under ExecuteSkill namespace." };
                    }
                }
            }

            return null;
        }

        public static void Run(CharacterBody body) {
            if (!Assemblies.ContainsKey(body)) {
                return;
            }

            Assembly assembly = Assemblies[body];
            
            Module module = assembly.GetModules()[0];

            if (module != null) {
                Type type = module.GetType("ExecuteSkill.Directive");

                if (type != null) {
                    MethodInfo method = type.GetMethod("Main");

                    if (method != null) {
                        method.Invoke(null, new object[] { body });
                    }
                }
            }
        }

        public static void SaveCode(string code) {
            string path = Assembly.GetExecutingAssembly().Location.Replace("ChaoticSkills.dll", "Directive_EXECUTE.cs");

            if (System.IO.File.Exists(path)) {
                System.IO.File.Delete(path);
            }

            System.IO.File.WriteAllText(path, code);
        }

        public static string LoadCode() {
            string path = Assembly.GetExecutingAssembly().Location.Replace("ChaoticSkills.dll", "Directive_EXECUTE.cs");

            if (System.IO.File.Exists(path)) {
                return System.IO.File.ReadAllText(path);
            }

            return BaseCode;
        }

        public class ExecuteUIController : MonoBehaviour {
            public TMP_InputField CodeField;
            public HGTextMeshProUGUI ErrorText;
            public GameObject InputBox;
            public GameObject ErrorBox;
            public Button.ButtonClickedEvent CompileEvent;
            public Button.ButtonClickedEvent RunEvent;
            public Button.ButtonClickedEvent SaveEvent;
            public Button.ButtonClickedEvent LoadEvent;
            public CharacterBody whoSpawnedUs;
            public void Start() {
                Transform MainPanel = this.transform.Find("MainPanel");
                Transform Juice = MainPanel.Find("Juice");
                Transform Icons = Juice.Find("IconContainer");
                Transform Template = Icons.Find("PickupButtonTemplate");
                Juice.Find("Label").GetComponent<LanguageTextMeshController>().token = "RE-Xde";

                Icons.gameObject.SetActive(false);

                InputBox = GameObject.Instantiate(InputTextPrefab, Juice);
                InputBox.SetActive(true);
                InputBox.transform.localPosition = new Vector3(0, 40, 0);
                InputBox.GetComponent<RectTransform>().sizeDelta = new Vector2(900, 400);
                CodeField = InputBox.GetComponent<TMP_InputField>();
                CodeField.text = Execute.LoadCode();
                CodeField.textComponent.richText = false;
                CodeField.onValueChanged.AddListener(HandleSyntaxHighlighting);

                HandleSyntaxHighlighting(CodeField.text);

                CodeField.lineType = TMP_InputField.LineType.MultiLineNewline;
                CodeField.textComponent.verticalAlignment = VerticalAlignmentOptions.Top;

                ErrorBox = GameObject.Instantiate(ErrorPrefab, Juice);
                ErrorBox.transform.localPosition = new Vector3(0, -270, 0);
                ErrorBox.SetActive(true);
                ErrorText = ErrorBox.transform.Find("Text Area").Find("Text").GetComponent<HGTextMeshProUGUI>();
                ErrorText.text = ">";

                CompileEvent = new();
                RunEvent = new();
                SaveEvent = new();
                LoadEvent = new();

                CompileEvent.AddListener(Compile);
                RunEvent.AddListener(Run);
                SaveEvent.AddListener(SaveCode);
                LoadEvent.AddListener(LoadCode);


                CreateButton("Compile", CompileEvent, -250);
                CreateButton("Run", RunEvent, -500);
                CreateButton("Save", SaveEvent, 250);
                CreateButton("Load", LoadEvent, 500);
            }

            public void SaveCode() {
                Execute.SaveCode(CodeField.text);
            }

            public void LoadCode() {
                CodeField.text = Execute.LoadCode();
                
            }

            public void Compile() {
                Debug.Log("compiling");
                string[] errors = Execute.Compile(CodeField.text, whoSpawnedUs);

                if (errors != null) {
                    ErrorText.text = "> " + errors[0];
                } else {
                    ErrorText.text = "> No errors.";
                }
            }

            public void Run() {
                if (ErrorText.text != "> No errors.") {
                    return;
                }

                Execute.Run(whoSpawnedUs);
            }

            public void CreateButton(string name, Button.ButtonClickedEvent onClick, int x) {
                Transform MainPanel = this.transform.Find("MainPanel");
                Transform Juice = MainPanel.Find("Juice");
                Transform Icons = Juice.Find("IconContainer");
                Transform Template = Icons.Find("PickupButtonTemplate");
                GameObject button = Juice.Find("CancelButton").gameObject;

                GameObject newButton = GameObject.Instantiate(button, Juice);
                newButton.SetActive(true);
                newButton.transform.localPosition = new Vector3(x, -376, 0);
                HGButton hgButton = newButton.GetComponent<HGButton>();
                hgButton.onClick = onClick;
                newButton.GetComponent<LanguageTextMeshController>().token = name;
            }

            public void HandleSyntaxHighlighting(string text) {
                return;

                ResetAllCharsToWhite();

                TMP_TextInfo textInfo = CodeField.textComponent.textInfo;

                for (int i = 0; i < textInfo.wordInfo.Length; i++) {
                    TMP_WordInfo word = textInfo.wordInfo[i];

                    foreach (KeyValuePair<List<string>, Color32> pair in SyntaxHex) {
                        foreach (string syntax in pair.Key) {
                            string str = CodeField.text.Substring(word.firstCharacterIndex, word.characterCount);
                            if (String.IsNullOrEmpty(str)) {
                                goto cont;
                            }

                            if (str == syntax) {
                                SetWordColor(word, pair.Value);
                                goto cont;
                            }

                            if (syntax.Length > 1) {
                                continue;
                            }

                            for (int j = 0; j < word.characterCount; j++) {
                                int charIndex = word.firstCharacterIndex + j;
                                
                                if (CodeField.text[charIndex] == syntax[0]) {
                                    SetCharColor(charIndex, pair.Value);
                                    goto cont;
                                }
                            }
                        }
                    }

                    cont:
                    continue;
                }
            }

            public void ResetAllCharsToWhite() {
                TMP_TextInfo textInfo = CodeField.textComponent.textInfo;

                for (int i = 0; i < textInfo.wordInfo.Length; i++) {
                    TMP_WordInfo wordInfo = textInfo.wordInfo[i];

                    SetWordColor(wordInfo, Color.white);
                }
            }

            public void SetWordColor(TMP_WordInfo wordInfo, Color32 color) {
                TMP_TextInfo textInfo = CodeField.textComponent.textInfo;

                for (int j = 0; j < wordInfo.characterCount; j++) {
                    Debug.Log("setting char color at index " + (wordInfo.firstCharacterIndex + j) + " to " + color);
                    SetCharColor(wordInfo.firstCharacterIndex + j, color);
                }
            }

            public void SetCharColor(int index, Color32 color) {
                CodeField.textComponent.ForceMeshUpdate();
                
                /*int meshIndex = CodeField.textComponent.textInfo.characterInfo[index].materialReferenceIndex;
                int vertexIndex = CodeField.textComponent.textInfo.characterInfo[index].vertexIndex;

                Color32[] vertexColors = CodeField.textComponent.textInfo.meshInfo[meshIndex].colors32;
                vertexColors[vertexIndex + 0] = color;
                vertexColors[vertexIndex + 1] = color;
                vertexColors[vertexIndex + 2] = color;
                vertexColors[vertexIndex + 3] = color;*/

                CodeField.textComponent.textInfo.characterInfo[index].color = color;

                CodeField.textComponent.UpdateGeometry(CodeField.textComponent.mesh, 0);
            }
        }
    }
}