using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using System.IO;

/// <summary>
/// テクスチャマネージャー
/// </summary>
public class TextureManager : SingletonMonoBehaviour<TextureManager> {

    public class SpriteInstance {
        public Sprite sprite { private set; get; }
        public int width { private set; get; }
        public int height { private set; get; }
        public float aspect { private set; get; }

        public SpriteInstance(Sprite s, int w, int h) {
            sprite = s;
            width = w;
            height = h;
            aspect = (float)width / (float)height;
        }
    }

    private Dictionary<string, SpriteInstance> _spriteDict = new();

    public IEnumerator LoadTextureCoroutine(string filePath, UnityAction<SpriteInstance> unityAction) {

        if (_spriteDict.ContainsKey(filePath)) {
            unityAction?.Invoke(_spriteDict[filePath]);
            yield break;
        }

        if (!Util.FileExists(filePath)) {
            Common.Instance.LoadingActive(false);
            yield break;
        }

        var byteData = File.ReadAllBytes(filePath);
        var texture2D = new Texture2D(1, 1);
        if (!texture2D.LoadImage(byteData)) {
            Common.Instance.LoadingActive(false);
            yield break;
        }

        Debug.Log($"<color=#00FF00>Load... {filePath}</color>");
        var sprite = Sprite.Create(texture2D, new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
        _spriteDict.Add(filePath, new(sprite, texture2D.width, texture2D.height));

        unityAction?.Invoke(_spriteDict[filePath]);
    }
}
