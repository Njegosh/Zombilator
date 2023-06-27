using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{
    Animator anim;

    public TextMeshProUGUI text;
    public Image image;

    void Start(){
        anim = GetComponent<Animator>();
    }

    public void Select(){
        if(anim==null)  // Na buildu pravi problem bez ovog
            anim = GetComponent<Animator>();
        anim.SetTrigger("Selected");
    }

    public void Deselect(){
        anim.SetTrigger("Deselect");
        anim.ResetTrigger("Selected");
    }

    public void UpdateItem(Item item){
        text.text = item.Name;
        image.sprite = item.Icon;

        if(image.sprite != null)
            image.color = Color.white;
        else
            image.color = new Color(0,0,0,0);
    }
}
