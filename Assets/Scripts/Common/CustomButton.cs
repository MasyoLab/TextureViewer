using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CustomButton : MonoBehaviour {

    [SerializeField]
    private CustomButtonAction buttonAction = null;
    public Button.ButtonClickedEvent onClick => buttonAction.onClick;
}
