using BepInEx;
using BepInEx.Logging;
using Expedition;
using Fisobs.Core;
using HarmonyLib;
using IL;
using IL.MoreSlugcats;
using JetBrains.Annotations;
using On;
using RWCustom;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using TemplateMod.Creatures;
using UnityEngine;
using Debug = UnityEngine.Debug;
#pragma warning disable CS0618

[module: UnverifiableCode]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]

namespace TemplateMod;

[BepInPlugin(GUID, Name, Version)]
public partial class Plugin : BaseUnityPlugin
{
    public const string GUID = "none.mynicerainworldmod";
    public const string Name = "My Nice Rain World Mod";
    public const string Version = "1.0.0";

    static Plugin()
    {
        Console.WriteLine("Plugin static constructor executed.");
    }

    public void OnEnable()
    {
        Content.Register(new CrateFisobs());
        Content.Register(new LizardCritob());
        On.RainWorld.OnModsInit += RainWorldOnOnModsInit;
    }

    private bool IsInit;
    private void RainWorldOnOnModsInit(On.RainWorld.orig_OnModsInit orig, RainWorld self)
    {
        orig(self);

        // 초기화 로직이 한 번만 실행되도록 방지. 처음에는 false이므로 진입.
        if (IsInit) return;
        
        try
        {
            // 초기화 실패 시: Content.Register나 LogAtlases()에서
            // 오류가 발생하여 catch 블록으로 이동하면,
            // IsInit = true; 코드는 실행되지 못하고 건너뛰어져.
            IsInit = true;
            
            
         
            LogAtlases();

            //Your hooks go here
            Hooks.Apply();

            Console.WriteLine("My Nice Rain World Mod loaded!");
        }
        catch (Exception ex)
        {
            Console.WriteLine("--- OnEnable/Content Registration FAILED! ---");
            Console.WriteLine($"Error Message: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            Console.WriteLine("------------------------------------------");
        }
    }
    
    public void LogAtlases()
    {
        var atlasManager = Futile.atlasManager;

        foreach (KeyValuePair<string, FAtlasElement> item in atlasManager._allElementsByName)
        {
            Console.WriteLine($"{item.Value.name}");
        }
    }
}
