using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 表示を制御する
/// </summary>
public class DisplayController : MonoBehaviour {

    [SerializeField]
    private Image _image = null;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter = null;

    private LinkedList<FileManager.FilePath> _itemList = new();
    private LinkedListNode<FileManager.FilePath> _currentNode = null;

    private float _timeCount = 0;
    private Vector2 _screenSize = default;

    // Start is called before the first frame update
    private void Start() {
        _screenSize.x = Screen.width;
        _screenSize.y = Screen.height;
        _image.color = Color.clear;

        Common.Instance.OnStart.AddListener(() => {
            enabled = true;
            SetAddLast();
            ShowImage();
        });
        Common.Instance.OnStop.AddListener(() => {
            enabled = false;
        });

        enabled = false;
    }

    // Update is called once per frame
    void Update() {
        _timeCount += Time.deltaTime;
        AdjustScreenSize();
    }

    /// <summary>
    /// 表示対象を登録する
    /// </summary>
    private void SetAddLast() {
        _itemList.Clear();
        foreach (var item in Common.Instance.FileManager.FilePathList) {
            _itemList.AddLast(item);
        }
    }

    /// <summary>
    /// LinkedListNode を更新
    /// </summary>
    private void NextNode() {
        if (_currentNode == null || _currentNode.Next == null) {
            _currentNode = _itemList.First;
        }
        else {
            _currentNode = _currentNode.Next;
        }
    }

    /// <summary>
    /// 画像を表示する
    /// </summary>
    private void ShowImage() {
        NextNode();

        StartCoroutine(Common.Instance.TextureManager.LoadTextureCoroutine(_currentNode.Value.Path, inst => {
            _aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.None;
            _image.color = Color.white;
            _image.sprite = inst.sprite;
            _aspectRatioFitter.aspectRatio = inst.aspect;
            DisplaySizeUpdate();
        }));
    }

    /// <summary>
    /// 表示サイズを更新する
    /// </summary>
    private void DisplaySizeUpdate() {
        _aspectRatioFitter.enabled = false;
        _image.SetNativeSize();
        _aspectRatioFitter.enabled = true;

        if (Screen.height < _image.rectTransform.sizeDelta.y) {
            _aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            var size = _image.rectTransform.sizeDelta;
            size.y = Screen.height;
            _image.rectTransform.sizeDelta = size;
        }
        if (Screen.width < _image.rectTransform.sizeDelta.x) {
            _aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
            var size = _image.rectTransform.sizeDelta;
            size.x = Screen.width;
            _image.rectTransform.sizeDelta = size;
        }
    }

    /// <summary>
    /// 画面サイズの変更を監視する
    /// </summary>
    private void AdjustScreenSize() {
        if (Screen.width != _screenSize.x || Screen.height != _screenSize.y) {
            DisplaySizeUpdate();
        }
        _screenSize.x = Screen.width;
        _screenSize.y = Screen.height;
    }
}
