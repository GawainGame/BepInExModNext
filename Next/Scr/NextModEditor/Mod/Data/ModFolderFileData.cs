﻿using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using SkySwordKill.Next.Mod;
using SkySwordKill.Next.Scr.NextModEditor.Mod.CommonClass;

namespace SkySwordKill.NextModEditor.Mod.Data;

public abstract class ModFolderFileData<T> : IModData where T : ModFolderFileData<T>
{
    public abstract int Id { get; set; }
    public static string FolderName { get; set; }

    public static List<T> Load(string dir)
    {
        List<T> dataList = new List<T>();
        var buffDir = $"{dir}/{FolderName}";
        if (!Directory.Exists(buffDir))
            return dataList;
        
        foreach (var filePath in Directory.GetFiles(buffDir))
        {
            T data;
            try
            {
                data = JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
            }
            catch (Exception e)
            {
                throw new Exception($"加载 {typeof(T).Name} 数据失败！文件路径：{filePath}", e);
            }

            if (Path.GetFileNameWithoutExtension(filePath) != data?.Id.ToString())
                throw new ModException("文件ID与定义ID不一致");
            
            dataList.Add(data);
        }

        return dataList;
    }

    public static void Save(string dir, List<T> dataDic)
    {
        var buffDir = $"{dir}/{FolderName}";
        if (Directory.Exists(buffDir))
            Directory.Delete(buffDir, true);
        Directory.CreateDirectory(buffDir);
        foreach (var data in dataDic)
        {
            var filePath = $"{buffDir}/{data.Id}.json";
            var json = JsonConvertEx.SerializeObject(data);
            File.WriteAllText(filePath, json);
        }
    }
}