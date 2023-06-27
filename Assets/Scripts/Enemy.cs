using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Enemy : Damagable
{
    public ZombieTypes type;
    protected Transform cilj;
    protected NavMeshAgent agent;
    public ZombiManager manager;

    public int maxHp = 10;
    protected int hp;

    public abstract void Tick(Transform _destination);
}

public abstract class Damagable : MonoBehaviour 
{
    public abstract void TakeDmg(Effect dmg);
}