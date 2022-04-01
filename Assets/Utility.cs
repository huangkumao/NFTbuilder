
using System.Collections.Generic;
using System.IO;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public static class Utility
{
#if UNITY_EDITOR
    [MenuItem("Tools/ClearSave")]
    public static void ClearSave()
    {
        PlayerPrefs.DeleteAll();
    }
#endif

    public static List<string> GetFiles(string pPath)
    {
        var _Paths = new List<string>();
        var _Fs = Directory.GetFiles(pPath);
        foreach (var _S in _Fs)
        {
            if (_S.EndsWith(".png") || _S.EndsWith(".PNG"))
            {
                _Paths.Add(_S);
            }
        }

        return _Paths;
    }

    public static void WritePngFile(string pPath, byte[] pData)
    {
        File.WriteAllBytes(pPath, pData);
    }

    public static List<List<string>> AllFiles = new List<List<string>>(10);

    public static void GenAllFiles()
    {
        AllFiles.Clear();
        GlobalData.Sort();

        foreach (var _PartInfo in GlobalData.Ins.AllParts)
        {
            AllFiles.Add(GetFiles(_PartInfo.PartPath));
        }
    }

    public static Texture2D MergeTexture(Texture2D[] pTexs)
    {
        Texture2D tex2d = new Texture2D(GlobalData.Ins.Width, GlobalData.Ins.Height, TextureFormat.ARGB32, false);
        Color[] colors = new Color[GlobalData.Ins.Width * GlobalData.Ins.Height];
        for (int i = 0; i < pTexs.Length; i++)
        {
            var color = pTexs[i].GetPixels(0);

            for (int j = 0; j < colors.Length; j++)
            {
//                if (color[j].a > UIConfig.alphaThreshold)
//                    colors[j] = color[j];
//                else
                    colors[j] = GenColor(colors[j], color[j]);
            }
        }
        tex2d.SetPixels(0, 0, GlobalData.Ins.Width, GlobalData.Ins.Height, colors);
        tex2d.Apply();
        return tex2d;
    }

    public static Color GenColor(Color drc, Color src)
    {
        //源颜色 * 源透明值 + 目标颜色*（1 - 源透明值）
        var s = RGBMultiplied(src, src.a) + RGBMultiplied(drc, 1 - src.a);
        return s;
    }

    public static Color RGBMultiplied(Color c, float multiplier)
    {
        return new Color(c.r * multiplier, c.g * multiplier, c.b * multiplier, c.a);
    }
}
