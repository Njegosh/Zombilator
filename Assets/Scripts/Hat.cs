using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
class Hat : Item
{
    [SerializeField]
    public List<Stat> stats;

    

    /*

    [SerializeField]
    public string Name { get => name; set => name = value; }

    [SerializeField]
    public string Desc { get => desc; set => desc = value; }
    
    [SerializeField]
    public Sprite Icon { get => icon; set => icon = value; }
    
    public GameObject Model { get => model; set => model = value; }
    */
}