using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HpUI : MonoBehaviour
{
    public Slider slider;
    public TextMeshProUGUI text;

    public void UpdateHp(int hp){
        slider.value = hp;
        text.text = hp.ToString();
    }
}
