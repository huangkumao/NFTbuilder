using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[Serializable]
public class GlobalData
{
    public static GlobalData Ins;
    public static void Load()
    {
        if (PlayerPrefs.HasKey("Save"))
        {
            Ins = JsonUtility.FromJson<GlobalData>(PlayerPrefs.GetString("Save"));
        }
        else
        {
            Ins = new GlobalData();
        }
    }

    public static void Save()
    {
        Sort();

        var _S = JsonUtility.ToJson(Ins);
        PlayerPrefs.SetString("Save", _S);
        PlayerPrefs.Save();
    }

    public static void Sort()
    {
        Ins.AllParts.Sort((a, b) => a.PartLayer.CompareTo(b.PartLayer));
    }

    //保存路径
    public string SavePath = Path.Combine(@"D:\", "GeneratedPng");

    //宽-高
    public int Width = 200;
    public int Height = 200;

    //生成文件数量
    public int FileCount = 100;
    //输出文件名前缀
    public string FilePreFix = "GenPng_";

    //各个部位
    public List<PartInfo> AllParts = new List<PartInfo>();
}

[Serializable]
public class PartInfo
{
    //部位路径
    public string PartPath;
    //部位层级
    public int PartLayer;
}
