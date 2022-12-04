using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Common : SingletonMonoBehaviour<Common> {

    [SerializeField]
    private GameObject _loadingObject = null;
    [SerializeField]
    private RectTransform _display = null;

    public FileManager FileManager => FileManager.Instance;
    public TextureManager TextureManager => TextureManager.Instance;

    public UnityEvent OnStart { set; get; } = new();
    public UnityEvent OnStop { set; get; } = new();
    public RectTransform Display => _display;

    protected override void Awake() {
#if !UNITY_EDITOR
        Debug.unityLogger.logEnabled = false;
#endif
        base.Awake();
    }

    private void Start() {
        _loadingObject.SetActive(false);

        gameObject.AddComponent<TextureManager>();
        gameObject.AddComponent<FileManager>();
    }

    public void LoadingActive(bool value) {
        _loadingObject.SetActive(value);
    }

    public void DisplayStart() {
        OnStart?.Invoke();
    }

    public void DisplayStop() {
        OnStop?.Invoke();
    }
}
