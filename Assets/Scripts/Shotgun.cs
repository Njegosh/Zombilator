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

    PlayerControls player;

/*
    public override string Name { get => name; set => name = value; }
    public override string Desc { get => desc; set => desc = value; }
    public override Sprite Icon { get => icon; set => icon = value; }
    
    [SerializeField]
    public override GameObject Model { get => model; set => model = value; }*/

    void Start() {
        spawnPlace = this.transform.GetChild(0);
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerControls>();
    }

    public override void UseAlt()
    {
        throw new System.NotImplementedException();
    }

    public int br = 2;

    public override void UsePrimary()
    {
        if(br>0){
            Instantiate(bullets, spawnPlace);
            usableUI.Use();
            br--;
            if(br==0){
                usableUI.Reload();
                player.DoAnim("Reload");
            }
        }
    }
    public override void Reload()
    {
        usableUI.Reload();
        player.DoAnim("Reload");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
