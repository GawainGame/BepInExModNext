﻿using System;
using System.Text.RegularExpressions;
using BepInEx.Configuration;
using Newtonsoft.Json.Linq;
using SkySwordKill.Next.I18N;

namespace SkySwordKill.Next.Extension;

public static class ExtensionUtils
{
    public static void TryAddOrReplace(this JSONObject jsonObject, string key, JSONObject value)
    {
        var index = jsonObject.keys.IndexOf(key);
        if (index <= -1)
        {
            jsonObject.AddField(key, value.Copy());
        }
        else
        {
            jsonObject.list[index] = value.Copy();
        }
    }

    public static void TryAddOrReplace(this JObject jsonObject, string key, JToken value)
    {
        if (jsonObject.ContainsKey(key))
            jsonObject.Remove(key);
        jsonObject.Add(key, value);
    }

    public static string DecodeJsonUnicode(this string json)
    {
        Regex reg = new Regex(@"(?i)\\[uU]([0-9a-f]{4})");
        string convertSrt = reg.Replace(json,
            delegate (Match m) { return ((char)Convert.ToInt32(m.Groups[1].Value, 16)).ToString(); });
        return convertSrt.Replace("\t", "    ");
    }

    /// <summary>
    /// 国际化文本
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string I18N(this string key)
    {
        return NextLanguage.Get(Main.I.CurrentLanguage, key);
    }

    /// <summary>
    /// 待国际化的文本
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string I18NTodo(this string key)
    {
        return key;
    }

    public static ConfigTarget<T> CreateConfig<T>(this ConfigFile config, string section, string key,
        T defaultValue, string desc = "")
    {
        return new ConfigTarget<T>(config, section, key, defaultValue, desc);
    }
}