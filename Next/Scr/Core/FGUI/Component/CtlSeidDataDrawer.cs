﻿using System;
using System.Collections.Generic;
using FairyGUI;
using SkySwordKill.Next.FGUI.Dialog;
using SkySwordKill.NextModEditor.Mod;
using SkySwordKill.NextFGUI.NextCore;
using SkySwordKill.NextModEditor.Mod.Data;

namespace SkySwordKill.Next.FGUI.Component;

public class CtlSeidDataPropertyDrawer : CtlPropertyDrawerBase
{
    public CtlSeidDataPropertyDrawer(string drawerName, int ownerId, List<int> seidData, ModWorkshop mod, 
        IModSeidDataGroup seidDataGroup,Dictionary<int,ModSeidMeta> seidMetas,Func<int ,string> seidDescGetter)
    {
        DrawerName = drawerName;
        OwnerId = ownerId;
        SeidData = seidData;
        Mod = mod;
        SeidDataGroup = seidDataGroup;
        SeidMetas = seidMetas;
        SeidDescGetter = seidDescGetter;
    }
    private UI_ComSeidDataDrawer Drawer => (UI_ComSeidDataDrawer)Component;
        
    private string DrawerName { get; }
    private int OwnerId { get; }
    private List<int> SeidData { get; }
    private ModWorkshop Mod { get; }
    private IModSeidDataGroup SeidDataGroup { get; }
    private Dictionary<int,ModSeidMeta> SeidMetas { get; }
    private Func<int ,string> SeidDescGetter { get; }
        
    protected override GComponent OnCreateCom()
    {
        var drawer = UI_ComSeidDataDrawer.CreateInstance();
        drawer.m_btnEdit.onClick.Add(OnClickEdit);
        drawer.title = DrawerName;
        return drawer;
    }

    protected override void OnRefresh()
    {
        Drawer.m_lstSeid.numItems = 0;
        foreach (var seidId in SeidData)
        {
            var item = Drawer.m_lstSeid.AddItemFromPool().asLabel;
            item.title = SeidDescGetter(seidId);
        }
    }

    protected override void SetDrawerEditable(bool value)
    {
        Drawer.grayed = !value;
    }

    private void OnClickEdit(EventContext context)
    {
        var window = WindowSeidEditorDialog.CreateDialog("特性编辑" ,Mod, OwnerId, SeidDataGroup,SeidMetas , SeidData, OnClose);
        window.Editable = Editable;
    }

    private void OnClose()
    {
        Refresh();
        OnChanged?.Invoke();
    }
}