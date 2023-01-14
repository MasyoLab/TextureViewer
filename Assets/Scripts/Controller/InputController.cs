using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// 入力コントローラー
/// </summary>
public class InputController : MonoBehaviour {

    private const string TAG_MENU = "MenuUI";

    [SerializeField]
    private RectTransform _display = null;
    [SerializeField]
    private InputField _input = null;
    [SerializeField]
    private CustomButton _onLoadMenu = null;
    [SerializeField]
    private CustomButton _onLoad = null;
    [SerializeField]
    private RectTransform _menuDisplay = null;
    [SerializeField]
    private RectTransform[] _menuTransforms = null;

    private List<RaycastResult> _raycastResults = new();

    private void Awake() {
        _onLoadMenu.onClick.AddListener(OnLoadMenu);
        _onLoad.onClick.AddListener(OnLoad);
    }

    private void Start() {
        SetMenuActive(false);
        foreach (var rectTransform in _menuTransforms) {
            InitTag(rectTransform);
        }
    }

    private void Update() {
        OnMenu();
        CloseMenu();
    }

    private void OnLoadMenu() {
        SetMenuActive(true);
    }

    private void OnLoad() {
        Common.Instance.FileManager.SetDirectory(_input.text);
        SetMenuActive(false);
    }

    /// <summary>
    /// メニューを表示する
    /// </summary>
    private void OnMenu() {
        if (!Input.GetMouseButtonUp((int)Util.MouseButtonEnum.RIGHT)) {
            return;
        }

        var mousePos = Input.mousePosition;

        _menuDisplay.gameObject.SetActive(true);
        _menuDisplay.position = mousePos;

        var posx = Mathf.Abs(_menuDisplay.GetAnchoredPositionRight());
        var posy = Mathf.Abs(_menuDisplay.GetAnchoredPositionBottom());

        if (_display.rect.width < posx) {
            mousePos.x -= posx - _display.rect.width;
        }

        if (_display.rect.height < posy) {
            mousePos.y += posy - _display.rect.height;
        }

        _menuDisplay.position = mousePos;
    }

    /// <summary>
    /// メニューを閉じる
    /// </summary>
    private void CloseMenu() {
        if (!Input.GetMouseButtonUp((int)Util.MouseButtonEnum.LEFT)) {
            return;
        }

        if (ClickMenu(Input.mousePosition)) {
            return;
        }

        SetMenuActive(false);
    }

    /// <summary>
    /// メニューをクリックした
    /// </summary>
    /// <param name="screenPosition"></param>
    /// <returns></returns>
    private bool ClickMenu(Vector2 screenPosition) {
        var pointerEventData = new PointerEventData(EventSystem.current);
        pointerEventData.position = screenPosition;

        EventSystem.current.RaycastAll(pointerEventData, _raycastResults);
        var over = _raycastResults.Any(v => v.gameObject.CompareTag(TAG_MENU));
        _raycastResults.Clear();
        return over;
    }

    /// <summary>
    /// メニューの表示切り替え
    /// </summary>
    /// <param name="value"></param>
    private void SetMenuActive(bool value) {
        foreach (var rectTransform in _menuTransforms) {
            rectTransform.gameObject.SetActive(value);
        }
    }

    /// <summary>
    /// 子要素のタグを全て変更
    /// </summary>
    /// <param name="transform"></param>
    /// <param name="tag"></param>
    private static void InitTag(Transform transform) {
        transform.tag = TAG_MENU;

        var children = transform.GetComponentInChildren<Transform>();
        if (children.childCount == 0) {
            return;
        }

        foreach (Transform child in children) {
            InitTag(child);
        }
    }
}
