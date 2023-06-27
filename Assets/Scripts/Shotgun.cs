using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Usable
{
    
    string name;
    string desc;
    Sprite icon;

    GameObject model;
    List<Stat> stats;


    public Transform spawnPlace;
    public GameObject bullets;

/*
    public override string Name { get => name; set => name = value; }
    public override string Desc { get => desc; set => desc = value; }
    public override Sprite Icon { get => icon; set => icon = value; }
    
    [SerializeField]
    public override GameObject Model { get => model; set => model = value; }*/

    void Start() {
        spawnPlace = this.transform.GetChild(0);
    }

    public override void UseAlt()
    {
        throw new System.NotImplementedException();
    }

    public override void UsePrimary()
    {
        Instantiate(bullets, spawnPlace);
        //throw new System.NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
