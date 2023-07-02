using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Zombi : Enemy
{
    bool ide = false;
    public float startDistance = 15f;
    public float giveUpDistance = 30f;

    int ti = 0;
    int tr = 5;

    public float idleSpeed = 5;
    public float idleAccel = 5;
    
    public float runSpeed = 17;
    public float runAccel = 15;

    public Animator anim;
    public GameObject particleDeath;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;

        agent = this.GetComponent<NavMeshAgent>();
        
        NavMesh.avoidancePredictionTime = 0.5f;


        agent.speed = idleSpeed;
        agent.acceleration = idleAccel;
        agent.destination = this.transform.position + new Vector3(Random.Range(-10,+10),0, Random.Range(-10,+10));
    }

    public Transform destination;

    public override void Tick(Transform _destination){
        if(this.gameObject.active){
        destination = _destination;
        anim.SetFloat("WalkSpeed",agent.velocity.magnitude);

        if(Vector3.Distance(this.transform.position, destination.position)<startDistance){
            ide = true;
        }

        else if(Vector3.Distance(this.transform.position, destination.position)>giveUpDistance){
            ide = false;

            agent.speed = idleSpeed;
            agent.acceleration = idleAccel;
        }

        if(ide) {
            agent.destination = destination.position;

            agent.speed = runSpeed;
            agent.acceleration = runAccel;
        }
        else {
            ti =  ti + 1;
            if(ti >= tr){
                agent.destination = this.transform.position + new Vector3(Random.Range(-15,+15),0, Random.Range(-15,+15));
                tr = Random.Range(3,100);
                ti=0;
            }
        }
        }
        
    }


    public override void TakeDmg(Effect dmg){
        if(dmg.source != EffectSource.Zombi)
            hp-=dmg.val;

        
        
        if(hp<=0){
            Instantiate(particleDeath,this.transform.position,particleDeath.transform.rotation);
            manager.Kill(this);
        }
    }
}
