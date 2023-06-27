using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ZombieSpawn{
    public ZombieTypes zombieType;

    public List<Vector3> pos;

    public ZombieSpawn(ZombieTypes _zombieType, List<Vector3> _pos){
        zombieType = _zombieType;
        pos = _pos;
    }
}