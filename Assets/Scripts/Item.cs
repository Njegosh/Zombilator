using System.Collections;
using System.Collections.Generic;
using Unity;
using UnityEngine;

/*
StatusType maybe in the future
*/

public enum EffectType
{
    None,
    Push,
    Knock,
    Fire,
    Ice
}
public enum EffectSource
{
    None,
    Zombi,
    Player
}

public class Effect
{
    public EffectType type;
    public EffectSource source;
    public int val;

    public Vector3 origin;

    public Effect(EffectType _type, EffectSource _source, int _val, Vector3 _origin){
        type = _type;
        source = _source;
        val = _val;
        origin = _origin;
    }
}


//------ ITEMS ------
public abstract class Item : MonoBehaviour
{
    [SerializeField]
    public string Name;
    [SerializeField]
    public string Desc;
    [SerializeField]
    public Sprite Icon;

    
    [SerializeField]
    public GameObject Model;
}