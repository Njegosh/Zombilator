using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUI : MonoBehaviour
{
    public ItemUI[] itemUIs;
    public HpUI hpUI;
    public int selected;

    public Animator animInventory;

    public Animator animHp;
    public Animator animItemWorks;

    bool show;

    void Start(){
        selected = 0;
        itemUIs[selected].Select();
    }

    public void Select(int id){
        if(id != selected){
            itemUIs[selected].Deselect();
            selected = id;
            itemUIs[selected].Select();
        }
    }

    public void ShowHideInventory(){
        show = !show;
        animInventory.SetBool("Show", show);
        animHp.SetBool("Show", show);
        animItemWorks.SetBool("Down",show);
    }

    public void UpdateItem(int id, Item item){
        itemUIs[id].UpdateItem(item);
    }

    public void UpdateHp(int hp){
        hpUI.UpdateHp(hp);
        animHp.SetTrigger("Hit");
        //animItemWorks.SetBool("Down",true);
        animItemWorks.ResetTrigger("GoDown");
        animItemWorks.SetTrigger("GoDown");
    }

    public void DangerHp(bool danger){
        animHp.SetBool("Danger", danger);
        animItemWorks.SetBool("Down",danger);
    }
}
