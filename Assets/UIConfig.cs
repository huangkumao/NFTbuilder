using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class UIConfig : MonoBehaviour
{
    public static float alphaThreshold = 0.9f;

    public InputField iOutputPath;
    public InputField iWidth;
    public InputField iHeight;

    public InputField iBatchCount;
    public InputField iFilePreFix;

    public GameObject SCRoot;
    public GameObject PartPrefab;

    void Awake()
    {
        GlobalData.Load();
    }

    void Start()
    {
        //初始化
        iOutputPath.text = GlobalData.Ins.SavePath;
        iWidth.text = GlobalData.Ins.Width.ToString();
        iHeight.text = GlobalData.Ins.Height.ToString();

        iBatchCount.text = GlobalData.Ins.FileCount.ToString();
        iFilePreFix.text = GlobalData.Ins.FilePreFix;

        foreach (var _Part in GlobalData.Ins.AllParts)
        {
            var _Obj = Instantiate(PartPrefab, SCRoot.transform, false);
            _Obj.GetComponent<UIPart>().Init(_Part);
        }
    }

    public void OnChange_OutPutPath(string pPath)
    {
        GlobalData.Ins.SavePath = pPath;
        if (!Directory.Exists(pPath))
            Directory.CreateDirectory(pPath);
    }

    public void OnChange_Width(string pValue)
    {
        GlobalData.Ins.Width = int.Parse(pValue);
    }

    public void OnChange_Height(string pValue)
    {
        GlobalData.Ins.Height = int.Parse(pValue);
    }

    public void OnChange_FileCount(string pValue)
    {
        GlobalData.Ins.FileCount = int.Parse(pValue);
        if (GlobalData.Ins.FileCount < 1)
            GlobalData.Ins.FileCount = 1;
    }

    public void OnChange_FilePreFix(string pValue)
    {
        GlobalData.Ins.FilePreFix = pValue;
    }

    public void OnChange_AlphaThreshold(string pValue)
    {
        alphaThreshold = float.Parse(pValue);
    }

    public void On_AddNewPart()
    {
        var _Info = new PartInfo()
        {
            PartPath = @"改成你的资源路径",
            PartLayer = 0
        };
        GlobalData.Ins.AllParts.Add(_Info);
        var _Obj = Instantiate(PartPrefab, SCRoot.transform, false);
        _Obj.GetComponent<UIPart>().Init(_Info);
    }

    public void On_SaveConfig()
    {
        GlobalData.Save();
    }

    //每个目录下的文件 预先读取保存
    public List<List<GenCell>> AllGenCells = new List<List<GenCell>>(8);
    //本次已经生成过的文件名
    public HashSet<string> AlreadyGenFileName = new HashSet<string>();

    public void BatchOutPut()
    {
        Utility.GenAllFiles();
        PreLoadAllFiles();

        StringBuilder _sName = new StringBuilder(128);
        var _T = new Texture2D[Utility.AllFiles.Count];
        for (int iIndex = 0; iIndex < GlobalData.Ins.FileCount; iIndex++)
        {
            _sName.Clear();

            for (int i = 0; i < AllGenCells.Count; i++)
            {
                var _Cell = AllGenCells[i][Random.Range(0, AllGenCells[i].Count)];
                _T[i] = _Cell.tex2d;
                _sName.Append(_Cell.fileName);
            }

            var _fname = _sName.ToString();
            if (AlreadyGenFileName.Contains(_fname))
                continue;

            AlreadyGenFileName.Add(_fname);

            var _tex = Utility.MergeTexture(_T);
            var _outpath = Path.Combine(GlobalData.Ins.SavePath,
                GlobalData.Ins.FilePreFix + _fname + ".png");
            Utility.WritePngFile(_outpath, _tex.EncodeToPNG());

            Thread.Sleep(1);
        }
    }

    private void PreLoadAllFiles()
    {
        AllGenCells.Clear();

        for (int i = 0; i < Utility.AllFiles.Count; i++)
        {
            var _t = new List<GenCell>(8);

            foreach (var pPath in Utility.AllFiles[i])
            {
                var _data = File.ReadAllBytes(pPath);
                var _tex2d = new Texture2D(GlobalData.Ins.Width, GlobalData.Ins.Height);
                _tex2d.LoadImage(_data);

                _t.Add(new GenCell
                {
                    tex2d = _tex2d,
                    fileName = Path.GetFileNameWithoutExtension(pPath)
                });
            }

            AllGenCells.Add(_t);
        }
    }
}

public class GenCell
{
    public string fileName;
    public Texture2D tex2d;
}
