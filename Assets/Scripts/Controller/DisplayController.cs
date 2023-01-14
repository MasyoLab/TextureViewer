using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 表示を制御する
/// </summary>
public class DisplayController : MonoBehaviour {

    [SerializeField]
    private RectTransform _display = null;
    [SerializeField]
    private Image _image = null;
    [SerializeField]
    private AspectRatioFitter _aspectRatioFitter = null;
    [SerializeField]
    private CustomButton _onOpen = null;

    private List<LinkedListNode<FileManager.FilePath>> _itemNodeList = new();
    private LinkedList<FileManager.FilePath> _itemLinked = new();
    private LinkedListNode<FileManager.FilePath> _currentNode = null;

    private float _timeCount = 0;
    private Vector2 _screenSize = default;

    // Start is called before the first frame update
    private void Start() {
        _onOpen.onClick.AddListener(OnOpen);

        SaveScreenSize();
        _image.color = Color.clear;

        Common.Instance.OnStart.AddListener(() => {
            _timeCount = 0;
            enabled = true;
            _currentNode = null;
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
        if (10 < _timeCount) {
            ShowImage();
            _timeCount = 0;
        }
    }

    private void FixedUpdate() {
        AdjustScreenSize();
    }

    /// <summary>
    /// 表示対象を登録する
    /// </summary>
    private void SetAddLast() {
        _itemLinked.Clear();
        _itemNodeList.Clear();

        foreach (var item in Common.Instance.FileManager.FilePathList) {
            var linkedListNode = _itemLinked.AddLast(item);
            _itemNodeList.Add(linkedListNode);
        }
    }

    /// <summary>
    /// LinkedListNode を更新
    /// </summary>
    private void NextNode() {
        if (_currentNode == null) {
            var random = new System.Random();
            var rnd = random.Next(_itemNodeList.Count);
            _currentNode = _itemNodeList[rnd];
        }

        if (_currentNode.Next == null) {
            _currentNode = _itemLinked.First;
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
            _image.sprite = inst.Sprite;
            _aspectRatioFitter.aspectRatio = inst.Aspect;
            DisplaySizeUpdate();
        }, ShowImage));
    }

    /// <summary>
    /// 表示サイズを更新する
    /// </summary>
    private void DisplaySizeUpdate() {
        _aspectRatioFitter.enabled = false;
        _image.SetNativeSize();
        _aspectRatioFitter.enabled = true;

        if (_display.rect.width < _image.rectTransform.sizeDelta.x) {
            _aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.WidthControlsHeight;
            var size = _image.rectTransform.sizeDelta;
            size.x = _display.rect.width;
            _image.rectTransform.sizeDelta = size;
        }

        if (_display.rect.height < _image.rectTransform.sizeDelta.y) {
            _aspectRatioFitter.aspectMode = AspectRatioFitter.AspectMode.HeightControlsWidth;
            var size = _image.rectTransform.sizeDelta;
            size.y = _display.rect.height;
            _image.rectTransform.sizeDelta = size;
        }
    }

    /// <summary>
    /// 画面サイズの変更を監視する
    /// </summary>
    private void AdjustScreenSize() {
        if (_display.rect.width != _screenSize.x || _display.rect.height != _screenSize.y) {
            DisplaySizeUpdate();
        }
        SaveScreenSize();
    }

    private void OnOpen() {
        if (_currentNode == null) {
            return;
        }
        Util.OpenPath(_currentNode.Value.Path);
    }

    private void SaveScreenSize() {
        _screenSize.x = _display.rect.width;
        _screenSize.y = _display.rect.height;
    }
}
