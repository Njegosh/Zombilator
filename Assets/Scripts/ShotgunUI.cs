using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunUI : UsableUI
{
    public Animator pel1;
    public Animator pel2;
    public Animator holder;

    int br = 2;

    public override void Reload()
    {
        holder.SetTrigger("Reload");
        pel1.SetTrigger("Reload");
        pel2.SetTrigger("Reload");
        br = 2;
    }

    public override void Use()
    {
        switch (br)
        {
            case 2:  
                pel2.SetTrigger("Shoot");
                break;
            case 1:  
                pel1.SetTrigger("Shoot");
                break;

            default:
                break;
        }
        br--;
    }
}
