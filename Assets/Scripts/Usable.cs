using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class Usable : Item 
{
    public abstract void UsePrimary();
    public abstract void UseAlt();
}


/*
    [SerializeField]
    public abstract string Name { get; set; }
    [SerializeField]
    public abstract string Desc { get; set; }
    [SerializeField]
    public abstract Sprite Icon { get; set; }
    [SerializeField]
    public abstract GameObject Model { get; set; }
*/