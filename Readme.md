# [English ReadMe Click Here](ReadmeEn.md)

# 觅长生 Next Mod框架

![Next](/preview.png)

觅长生 Next Mod是基于BepinEx框架的Mod，为觅长生游戏提供了数据增量修改、添加剧情与触发器的功能

github地址：https://github.com/magicskysword/Next

## 功能与作用
可以通过编写json文件，以在游戏里插入额外的功能与数据。

## 使用方法
从Steam创意工坊安装：

下载压缩包，将文件解压至 `觅长生\BepInEx\plugins` 文件夹

参考贴：https://bbs.3dmgame.com/thread-6207429-1-1.html

Mod WiKi：

* [文档附件](doc/Next文档.md) （保持最新）

* [Bilibili Wiki](https://wiki.biligame.com/mcs/Next%E9%A6%96%E9%A1%B5)

* [Fandom Wiki](https://michangshengnext.fandom.com/zh/wiki/%E8%A7%85%E9%95%BF%E7%94%9FNext_Wiki) （停止更新）

在游戏内可以通过 F4 打开mod菜单（按键可修改）

## 鸣谢
3dm  宵夜97  BepinEx开发教程<br>
3dm  ゞ残月﹎_|  Villain Mod的框架思路<br>
@玄武 赞助者

## 下载地址
[Github Release](https://github.com/magicskysword/Next/releases/latest)
[3DM 论坛](https://bbs.3dmgame.com/thread-6207429-1-1.html)
[3DM Mod站](https://mod.3dmgame.com/mod/178805)

## Build
clone该库后，自行重新添加觅长生游戏文件夹里的Dll引用，包括

`觅长生\BepInEx\core` 里的
```
0Harmony.dll
BepInEx.dll
```
`觅长生\觅长生_Data\Managed` 里的
```
Assembly-CSharp.dll
Assembly-CSharp-firstpass.dll
Newtonsoft.CSharp.dll
UnityEngine.dll
UnityEngine.AssetBundleModule.dll
UnityEngine.AudioModule.dll
UnityEngine.CoreModule.dll
UnityEngine.IMGUIModule.dll
UnityEngine.InputModule.dll
UnityEngine.TextRenderingModule.dll
UnityEngine.UI.dll
UnityEngine.UIModule.dll
UnityEngine.UnityWebRequestAssetBundleModule.dll
UnityEngine.UnityWebRequestAudioModule.dll
UnityEngine.UnityWebRequestModule.dll
UnityEngine.UnityWebRequestTextureModule.dll
UnityEngine.UnityWebRequestWWWModule.dll
```

添加完后直接Build即可，将Build出来的 `Next.Dll` 文件 、 `NextLib` 文件夹 与 `NextConfig` 文件夹 置入 `觅长生\BepInEx\plugins` 目录 即可

## 基于Next开发Mod
Next插件范例：https://github.com/magicskysword/NextExamplePlugin

## 使用库
[codingseb/ExpressionEvaluator](https://github.com/codingseb/ExpressionEvaluator) 用于事件与触发器的条件判断

[xiaoye97/VRoidXYTool](https://github.com/xiaoye97/VRoidXYTool) 使用了其中的GUI库

[Tencent/xLua](https://github.com/Tencent/xLua) 嵌入lua脚本

[rxi/json.lua](https://github.com/rxi/json.lua) lua json库

## 相关库
[BepinEx](https://github.com/BepInEx/BepInEx) 本mod基于BepinEx

## 许可证
[MIT许可证](https://github.com/magicskysword/Next/blob/main/Licenses/NextLICENSE)