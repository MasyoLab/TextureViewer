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
    private CustomButton _loadButton = null;

    private void Awake() {
        _loadButton.onClick.AddListener(OnLoad);
    }

    private void OnLoad() {
        Common.Instance.FileManager.SetDirectory(_input.text);
    }
}
