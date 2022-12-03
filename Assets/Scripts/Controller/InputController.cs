using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 入力コントローラー
/// </summary>
public class InputController : MonoBehaviour {

    [SerializeField]
    private InputField _input = null;
    [SerializeField]
    private CustomButton _button = null;

    private void Awake() {
        _button.onClick.AddListener(OnClick);
    }

    private void OnClick() {
        Common.Instance.FileManager.SetDirectory(_input.text);
    }
}
