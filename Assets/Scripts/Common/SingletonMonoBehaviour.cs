using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SingletonMonoBehaviour<_Ty> : MonoBehaviour where _Ty : MonoBehaviour {

    public static _Ty Instance { private set; get; } = null;
    public static bool HasInstance => Instance != null;

    protected virtual void Awake() {
        if (Instance == null) {
            Instance = this as _Ty;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(this);
        }
    }
}
