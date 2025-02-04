﻿using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Fungus;
using HarmonyLib;
using UnityEngine;
using Object = UnityEngine.Object;

namespace SkySwordKill.Next.Patch;

public class PatchTag : MonoBehaviour
{
        
}

[HarmonyPatch]
public class FungusClonePatch
{
    [HarmonyTargetMethod]
    static IEnumerable<MethodBase> TargetMethods()
    {
        var methods = new List<MethodBase>();
        var normalTypes = typeof(Object)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(method => method.Name == "Instantiate" && !method.IsGenericMethod);
        methods.AddRange(normalTypes);
        var genericTypes = typeof(Object)
            .GetMethods(BindingFlags.Static | BindingFlags.Public)
            .Where(method => method.Name == "Instantiate" && method.IsGenericMethod);
        foreach (var type in genericTypes)
        {
            methods.Add(type.MakeGenericMethod(typeof(Object)));
        }
        return methods;
    }

    [HarmonyPostfix]
    public static void Postfix(ref Object __result)
    {
        if (__result is GameObject go && go.GetComponent<PatchTag>() == null)
        {
            var flowcharts = go.GetComponentsInChildren<Flowchart>();
            if (flowcharts != null)
            {
                foreach (var flowchart in flowcharts)
                {
                    Main.FPatch.PatchFlowchart(flowchart);
                }
                go.AddComponent<PatchTag>();
            }
        }
    }
}

[HarmonyPatch(typeof(talkCompont), "Start")]
public class FungusTalkPatch
{
    [HarmonyPrefix]
    public static void Prefix(talkCompont __instance)
    {
        var go = __instance.gameObject;
        if (go.GetComponent<PatchTag>() == null)
        {
            var flowcharts = go.GetComponentsInChildren<Flowchart>();
            if (flowcharts != null)
            {
                foreach (var flowchart in flowcharts)
                {
                    Main.FPatch.PatchFlowchart(flowchart);
                }
            }

            go.AddComponent<PatchTag>();
        }
    }
}