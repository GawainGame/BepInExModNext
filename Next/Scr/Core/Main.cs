using UnityEngine;
using SkySwordKill.Next.Lua;
using SkySwordKill.Next.Extension;
using SkySwordKill.Next.FGUI;
using HarmonyLib;
using BepInEx;
using System.Reflection;
using System.Text;
using System.Linq;
using System.Collections;
﻿using System;

namespace SkySwordKill.Next
{
    [BepInPlugin("skyswordkill.plugin.Next", "Next", MOD_VERSION)]
    public partial class Main : BaseUnityPlugin
        public const string MOD_VERSION = "0.5.0";
    {
        
        public static Lazy<string> pathLocalModsDir;
        
        public static Lazy<string> pathLibraryDir;
        
        public static Lazy<string> pathConfigDir;

        public static Lazy<string> pathBaseDataDir;
        
        public static Lazy<string> pathLuaLibDir;
        
        public static Lazy<string> pathLanguageDir;
        
        public static Lazy<string> pathModSettingFile;
        public static Lazy<string> pathInnerAssetDir =
            new Lazy<string>(() => Utility.CombinePaths(
                BepInEx.Paths.PluginPath, "NextAssets"));
        public static Lazy<string> pathBaseFungusDataDir =
            new Lazy<string>(() => Utility.CombinePaths(
                pathBaseDataDir.Value, "Fungus"));
        public static Lazy<string> pathABDir = 
            new Lazy<string>(() => Utility.CombinePaths(
                pathLibraryDir.Value, "AB"));
        public static Main Instance { get; private set; }
        public static Main I { get; private set; }
        public static ResourcesManager Res => I.resourcesManager;
        public static LuaManager Lua => I.luaManager;
        public static FGUIManager FGUI => I.fguiManager;
        
        
        public static int logIndent = 0;
        
        public ConfigTarget<string> languageID;
        public ConfigTarget<bool> debugMode;
        public ConfigTarget<bool> openInStart;
        public ConfigTarget<KeyCode> winKeyCode;
        
        public NextLanguage nextLanguage;
        public NextModSetting nextModSetting;

        private ResourcesManager resourcesManager;
        private LuaManager luaManager;
        private FGUIManager fguiManager;

        private void Awake()
        {
            Init();
        }

        private void Init()
        {
            I = this;
            
            InitDir();

            languageID = Config.CreateConfig("Main.Language", "Plugin Language", "",
                "");
            winKeyCode = Config.CreateConfig("Main.OpenKeyCode", "Window HotKey", KeyCode.F4,
                "");
            openInStart = Config.CreateConfig("Main.OpenInStart", "Open Window In Game Start", true,
                "");
            debugMode = Config.CreateConfig("Debug.Mode", "Debug Mode", false,
                "");
            
            resourcesManager = gameObject.AddComponent<ResourcesManager>();
            resourcesManager.Init();
            
            luaManager = new LuaManager();
            luaManager.Init();

            fguiManager = new FGUIManager();
            fguiManager.Init();

            new Harmony("skyswordkill.plugin.Next").PatchAll();

            // 加载运行时脚本所需DLL
            // load runtime dll
            Assembly.LoadFrom(Utility.CombinePaths(pathLibraryDir.Value, "Microsoft.CSharp.dll"));
            Assembly.LoadFrom(Utility.CombinePaths(pathLibraryDir.Value, "System.Windows.Forms.dll"));
            Assembly.LoadFrom(Utility.CombinePaths(pathLibraryDir.Value, "Ookii.Dialogs.dll"));

            DialogAnalysis.Init();

            // 初始化语言与配置
            // Init language and config
            NextLanguage.InitLanguage();
            LoadDefaultLanguage();
            nextModSetting = NextModSetting.LoadSetting();
            
            // 检查更新
            // Check Update
            Updater.CheckVersion();
            
            // 根据设置显示窗口
            // show window by config
            _isWinOpen = openInStart.Value;
            
            // ModManager 启动由 JsonDataPatch 进行引导
            // ModManager startup is booted by JsonDataPatch
        }

        private void InitDir()
        {
            pathLocalModsDir =
            new Lazy<string>(() => BepInEx.Paths.GameRootPath + @"\本地Mod测试");
            pathLibraryDir =
                new Lazy<string>(() => Utility.CombinePaths(
                    Directory.GetParent(Instance.GetType().Assembly.Location).FullName,
                    "NextLib"));
            pathConfigDir =
                new Lazy<string>(() => Utility.CombinePaths(
                    Directory.GetParent(Instance.GetType().Assembly.Location).FullName,
                    "NextConfig"));
            pathBaseDataDir =
                new Lazy<string>(() => Utility.CombinePaths(
                    Directory.GetParent(Instance.GetType().Assembly.Location).FullName,
                    "../BaseOutPut"));
            pathLuaLibDir =
            new Lazy<string>(() => Utility.CombinePaths(
                pathLibraryDir.Value, "Lua"));
            pathLanguageDir =
            new Lazy<string>(() => Utility.CombinePaths(
                pathConfigDir.Value, "language"));
            pathModSettingFile =
            new Lazy<string>(() => Utility.CombinePaths(
                BepInEx.Paths.GameRootPath, "nextModSetting.json"));
        }

        private void LoadDefaultLanguage()
        {
            if (string.IsNullOrEmpty(languageID.Value) || !NextLanguage.languages.TryGetValue(languageID.Value, out var language))
            {
                // 选择一个语言作为默认语言
                // Choose Default Language
                SelectLanguage(NextLanguage.languages.Values.FirstOrDefault());
                isSelectedLanguage = true;
            }
            else
            {
                SelectLanguage(language);
            }
            
            languageID.SetName("Config.Main.LanguageID.Name".I18N());
            languageID.SetDesc("Config.Main.LanguageID.Desc".I18N());
            
            winKeyCode.SetName("Config.Main.OpenKeyCode.Name".I18N());
            winKeyCode.SetDesc("Config.Main.OpenKeyCode.Desc".I18N());
            
            openInStart.SetName("Config.Main.OpenInStart.Name".I18N());
            openInStart.SetDesc("Config.Main.OpenInStart.Desc".I18N());
            
            debugMode.SetName("Config.Debug.Mode.Name".I18N());
            debugMode.SetDesc("Config.Debug.Mode.Desc".I18N());
        }

        public void SelectLanguage(NextLanguage language)
        {
            nextLanguage = language;

            if (language == null)
                return;
            
            languageID.Value = language.FileName;
            LogInfo($"{"Misc.CurrentLanguage".I18N()} : {language.LanguageName}");
        }

        public void SaveModSetting()
        {
            NextModSetting.SaveSetting(nextModSetting);
        }

        public static Coroutine InvokeCoroutine(IEnumerator enumerator)
        {
            return I.StartCoroutine(enumerator);
        }

        public static void LogLua(object obj)
        {
            I.Logger.LogInfo($"Lua:\t{obj}");
        }
        
        public static void LogInfo(object obj)
        {
            I.Logger.LogInfo($"{GetIndent()}{obj}");
        }

        public static void LogWarning(object obj)
        {
            I.Logger.LogWarning($"{GetIndent()}{obj}");
        }

        public static void LogError(object obj)
        {
            I.Logger.LogError($"{GetIndent()}{obj}");
        }
        
        public static void LogDebug(object obj)
        {
            if (I.debugMode.Value)
            {
                I.Logger.LogInfo($"[Debug]{obj}");
            }
        }

        private static string GetIndent()
        {
            if (logIndent <= 0)
                return string.Empty;
            var sb = new StringBuilder(logIndent * 4);
            for (int i = 0; i < logIndent * 4; i++)
            {
                sb.Append(' ');
            }

            return sb.ToString();
        }
    }
}