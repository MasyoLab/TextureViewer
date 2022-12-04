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

    public interface ISpriteInstance {
        Sprite Sprite { get; }
        float Aspect { get; }
    }
    public class SpriteInstance : ISpriteInstance {
        public Sprite Sprite { private set; get; }
        public Texture2D Texture { private set; get; }
        public float Aspect { private set; get; }
        public int LifeSpan { set; get; }

        public SpriteInstance(Texture2D texture) {
            var sprite = Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));
            Sprite = sprite;
            Texture = texture;
            Aspect = (float)texture.width / (float)texture.height;
            LifeSpan = 10;
        }
        ~SpriteInstance() {
            Release();
        }

        public void Release() {
            if (Sprite != null) {
                Destroy(Sprite);
            }
            if (Texture != null) {
                Destroy(Texture);
            }
            Sprite = null;
        }
    }

    private Dictionary<string, SpriteInstance> _spriteDict = new();
    private List<KeyValuePair<string, SpriteInstance>> _deletingTarget = new();

    private void OnDestroy() {
        foreach (var item in _spriteDict) {
            item.Value.Release();
        }
        _spriteDict.Clear();
    }

    public IEnumerator LoadTextureCoroutine(string filePath, UnityAction<ISpriteInstance> unityAction, UnityAction reject) {

        if (_spriteDict.ContainsKey(filePath)) {
            unityAction?.Invoke(_spriteDict[filePath]);
            yield break;
        }

        if (!Util.FileExists(filePath)) {
            Common.Instance.LoadingActive(false);
            reject?.Invoke();
            yield break;
        }

        var byteData = File.ReadAllBytes(filePath);
        var texture2D = new Texture2D(1, 1);
        if (!texture2D.LoadImage(byteData)) {
            Common.Instance.LoadingActive(false);
            reject?.Invoke();
            yield break;
        }

        Debug.Log($"<color=#00FF00>Load... {filePath}</color>");
        _spriteDict.Add(filePath, new SpriteInstance(texture2D));

        unityAction?.Invoke(_spriteDict[filePath]);
        StartCoroutine(LifecycleCoroutine());
    }

    private IEnumerator LifecycleCoroutine() {
        yield return null;
        _deletingTarget.Clear();
        foreach (var item in _spriteDict) {
            item.Value.LifeSpan--;
            if (item.Value.LifeSpan <= 0) {
                _deletingTarget.Add(item);
            }
        }
        foreach (var item in _deletingTarget) {
            _spriteDict.Remove(item.Key);
        }

        if (_deletingTarget.Any()) {
            _deletingTarget.Clear();
            yield return Resources.UnloadUnusedAssets();
        }
    }
}
