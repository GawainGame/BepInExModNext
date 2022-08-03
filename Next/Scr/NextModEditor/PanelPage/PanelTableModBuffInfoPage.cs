﻿using System.Collections.Generic;
using System.Linq;
using FairyGUI;
using SkySwordKill.Next.FGUI.Component;
using SkySwordKill.NextModEditor.Mod.Data;
using SkySwordKill.Next.Extension;
using SkySwordKill.NextEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;

namespace SkySwordKill.NextEditor.PanelPage
{
    public class PanelTableModBuffInfoPage : PanelTablePageBase<ModBuffData>
    {
        public PanelTableModBuffInfoPage(string name, ModWorkshop mod, ModProject project) : base(name, mod, project)
        {
        }
        
        public override ModDataTableDataList<ModBuffData> ModDataTableDataList { get; set; }

        protected override void OnInit()
        {
            ModDataTableDataList = new ModDataTableDataList<ModBuffData>(Project.BuffData);
            
            AddTableHeader(new TableInfo(
                "ModEditor.Main.modBuffInfo.id".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModBuffData)data;
                    return buff.Id.ToString();
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modBuffInfo.name".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH,
                data =>
                {
                    var buff = (ModBuffData)data;
                    return buff.Name;
                }));

            AddTableHeader(new TableInfo(
                "ModEditor.Main.modBuffInfo.desc".I18N(),
                TableInfo.DEFAULT_GRID_WIDTH * 3,
                data =>
                {
                    var buff = (ModBuffData)data;
                    return buff.Desc;
                }));
        }

        protected override void OnInspectItem(IModData data)
        {
            if (data == null)
            {
                return;
            }

            var buffData = (ModBuffData)data;

            AddDrawer(new CtlIDPropertyDrawer(
                    "ModEditor.Main.modBuffInfo.id".I18N(),
                    buffData,
                    () => Project.BuffData,
                    theData =>
                    {
                        var curData = (ModBuffData)theData;
                        return $"{curData.Id} {curData.Name}";
                    },
                    (theData, newId) =>
                    {
                        Project.BuffSeidDataGroup.ChangeSeidID(theData.Id, newId);
                        theData.Id = newId;
                        Project.BuffData.ModSort();
                        CurInspectIndex = Project.BuffData.FindIndex(modData => modData == theData);
                    },
                    (theData, otherData) =>
                    {
                        Project.BuffSeidDataGroup.SwiftSeidID(otherData.Id, theData.Id);
                        (otherData.Id, theData.Id) = (theData.Id, otherData.Id);
                        Project.BuffData.ModSort();
                        CurInspectIndex = Project.BuffData.FindIndex(modData => modData == theData);
                    },
                    () => { Inspector.Refresh(); })
            );

            AddDrawer(new CtlStringPropertyDrawer(
                    "ModEditor.Main.modBuffInfo.name".I18N(),
                    str => buffData.Name = str,
                    () => buffData.Name)
            );

            AddDrawer(new CtlStringPropertyDrawer(
                "ModEditor.Main.modBuffInfo.skillEffect".I18N(),
                str => buffData.SkillEffect = str,
                () => buffData.SkillEffect)
            );

            AddDrawer(new CtlIntPropertyDrawer(
                "ModEditor.Main.modBuffInfo.icon".I18N(),
                num => buffData.Icon = num,
                () => buffData.Icon)
            );

            AddDrawer(new CtlStringAreaPropertyDrawer(
                    "ModEditor.Main.modBuffInfo.desc".I18N(),
                    str => buffData.Desc = str,
                    () => buffData.Desc)
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "类型".I18NTodo(),
                () => ModEditorManager.I.BuffDataBuffTypes.Select(type => $"{type.TypeID} : {type.TypeName}"),
                index => buffData.BuffType = ModEditorManager.I.BuffDataBuffTypes[index].TypeID,
                () => ModEditorManager.I.BuffDataBuffTypes.FindIndex(type =>
                    type.TypeID == buffData.BuffType))
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "叠加类型".I18NTodo(),
                () => ModEditorManager.I.BuffDataOverlayTypes.Select(type => $"{type.ID} : {type.Desc}"),
                index => buffData.BuffOverlayType = ModEditorManager.I.BuffDataOverlayTypes[index].ID,
                () => ModEditorManager.I.BuffDataOverlayTypes.FindIndex(type =>
                    type.ID == buffData.BuffOverlayType))
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "触发方式".I18NTodo(),
                () => ModEditorManager.I.BuffDataTriggerTypes.Select(type => $"{type.ID} : {type.Desc}"),
                index => buffData.Trigger = ModEditorManager.I.BuffDataTriggerTypes[index].ID,
                () => ModEditorManager.I.BuffDataTriggerTypes.FindIndex(type =>
                    type.ID == buffData.Trigger))
            );
            
            AddDrawer(new CtlDropdownPropertyDrawer(
                "移除方式".I18NTodo(),
                () => ModEditorManager.I.BuffDataRemoveTriggerTypes.Select(type => $"{type.TypeID} : {type.TypeName}"),
                index => buffData.RemoverTrigger = ModEditorManager.I.BuffDataRemoveTriggerTypes[index].TypeID,
                () => ModEditorManager.I.BuffDataRemoveTriggerTypes.FindIndex(type =>
                    type.TypeID == buffData.RemoverTrigger))
            );

            AddDrawer(new CtlCheckboxPropertyDrawer(
                "是否隐藏".I18NTodo(),
                isOn => buffData.IsHide = isOn ? 1 : 0,
                () => buffData.IsHide == 1)
            );
            
            AddDrawer(new CtlCheckboxPropertyDrawer(
                "只显示一层".I18NTodo(),
                isOn => buffData.ShowOnlyOne = isOn ? 1 : 0,
                () => buffData.ShowOnlyOne == 1)
            );

            AddDrawer(new CtlIntArrayBindDataPropertyDrawer(
                "ModEditor.Main.modBuffInfo.affix".I18N(),
                list => buffData.AffixList = list,
                () => buffData.AffixList,
                list => Project.GetAffixDesc(list),
                new List<TableInfo>()
                {
                    new TableInfo("ModEditor.Main.modAffixData.id".I18N(),
                        TableInfo.DEFAULT_GRID_WIDTH / 2,
                        getData => ((ModAffixData)getData).Id.ToString()),
                    new TableInfo("ModEditor.Main.modAffixData.name".I18N(),
                        TableInfo.DEFAULT_GRID_WIDTH / 2,
                        getData => ((ModAffixData)getData).Name),
                    new TableInfo("ModEditor.Main.modAffixData.desc".I18N(),
                        TableInfo.DEFAULT_GRID_WIDTH * 3,
                        getData => ((ModAffixData)getData).Desc),
                },
                () => new List<IModData>(Project.GetAllAffix())
            ));
            
            AddDrawer(new CtlSeidDataPropertyDrawer(
                "特性",
                buffData.Id,
                buffData.SeidList,
                Mod,
                Project.BuffSeidDataGroup,
                ModEditorManager.I.BuffSeidMetas,
                seidId =>
                {
                    if (ModEditorManager.I.BuffSeidMetas.TryGetValue(seidId, out var seidData))
                        return $"{seidId}  {seidData.Name}";
                    return $"{seidId}  ???";
                })
            );
        }
    }
}