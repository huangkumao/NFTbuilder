
using System.IO;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.UI;

public class GenImage : MonoBehaviour
{
    private Image m_Image;

    void Start()
    {
        m_Image = GetComponent<Image>();
    }

    public void Gen()
    {
        Utility.GenAllFiles();
        var _T = new Texture2D[Utility.AllFiles.Count];
        for (int i = 0; i < Utility.AllFiles.Count; i++)
        {
            var _path = Utility.AllFiles[i][Random.Range(0, Utility.AllFiles[i].Count)];
            var _data = File.ReadAllBytes(_path);
            var _tex2d = new Texture2D(GlobalData.Ins.Width, GlobalData.Ins.Height);
            _tex2d.LoadImage(_data);
            _T[i] = _tex2d;
        }

        var _tex = Utility.MergeTexture(_T);

        Sprite sp = Sprite.Create(_tex, new Rect(0, 0, _tex.width, _tex.height), new Vector2(0.5f, 0.5f));
        m_Image.sprite = sp;
    }

    public void Export()
    {
        var _PngData = m_Image.sprite.texture.EncodeToPNG();
        var _Path = GenPath();
        File.WriteAllBytes(_Path, _PngData);
    }

    private string GenPath()
    {
        var FileName = "Single_" + Random.Range(0,10000) + ".png";
        return Path.Combine(GlobalData.Ins.SavePath, FileName);
    }
}
