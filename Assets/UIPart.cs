using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

public class UIPart : MonoBehaviour
{
    public InputField PartPath;
    public InputField PartLayer;
    public Text FileCount;

    private PartInfo m_Info;

    public void Init(PartInfo pInfo)
    {
        m_Info = pInfo;
        PartPath.text = pInfo.PartPath;
        PartLayer.text = pInfo.PartLayer.ToString();

        OnDirChanged();
    }

    public void OnChange_Path(string pPath)
    {
        m_Info.PartPath = pPath;

        OnDirChanged();
    }

    public void OnChange_Layer(string pLayer)
    {
        if (string.IsNullOrEmpty(pLayer))
            m_Info.PartLayer = 0;
        m_Info.PartLayer = int.Parse(pLayer);
    }

    public void ClickDel()
    {
        GlobalData.Ins.AllParts.Remove(m_Info);
        Object.DestroyImmediate(this.gameObject);
    }

    private void OnDirChanged()
    {
        try
        {
            FileCount.text = "文件数量:" + Directory.GetFiles(m_Info.PartPath).Length.ToString();
        }
        catch
        {
            FileCount.text = "路径错误";
        }
    }
}
