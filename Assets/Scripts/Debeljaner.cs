using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Debeljaner : Enemy
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

    Rigidbody rb;

    public float speed = 10f;

    public Transform body;
    public Transform destination;

    // ---------
    Vector3 moveTo;
    Vector3 moveWant;
    Vector3 moveDown;

    Vector3 bounceForce;
    Vector3 startBounceForce;

    float brBounce;

    public float turnSpeed = 0.25f;
    public float brBounceMax = 100;

    public float bounciness = 10;
    public float maxBounce = 10;

    public float gravity = 10;

    public Transform graphics;

    List<Damagable> damaged = new List<Damagable>();


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;

        cilj = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();

        destination = this.transform;

        bounceForce  = Vector3.zero;
        startBounceForce = Vector3.zero;

        moveTo = this.transform.forward;
        moveWant = this.transform.forward;
    }

    bool bounced;

    Vector3 d;

    public override void Tick(Transform _destination){
        destination = _destination;
        d = destination.position;

        damaged = new List<Damagable>();
    }

    void FixedUpdate(){
        Vector3 p = new Vector3(
            d.x - this.transform.position.x,
            0,
            d.z - this.transform.position.z
        ).normalized;

        moveWant = Vector3.RotateTowards(moveWant, p, turnSpeed * Time.deltaTime, 0).normalized;

        moveTo = Vector3.ClampMagnitude(moveWant * speed + bounceForce, speed);

        graphics.LookAt(moveTo + this.transform.position, Vector3.up);

        anim.SetFloat("Speed", moveTo.magnitude / speed);
        /*
        if(!bounced)
            moveTo =  Vector3.RotateTowards(moveTo, p, turnSpeed * Time.deltaTime, 5).normalized * speed;  //(p * speed + bounceForce);
        else
            moveTo = bounceForce;
        */

        if(brBounce <= brBounceMax ){
            bounceForce = Vector3.Lerp(startBounceForce, Vector3.zero, brBounce / brBounceMax);
            brBounce += Time.deltaTime;
        }
        else
            bounced = false;

        rb.MovePosition(this.transform.position + moveTo * Time.deltaTime);

        Debug.DrawLine(this.transform.position, this.transform.position + moveTo, Color.blue);
        Debug.DrawLine(this.transform.position, this.transform.position + moveWant * speed, Color.red);
        Debug.DrawLine(this.transform.position, this.transform.position + bounceForce, Color.cyan);
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.layer != 6 && collision.gameObject.layer != 3){
            ContactPoint contact = collision.contacts[0];

            startBounceForce = new Vector3(
                contact.normal.x,
                0,
                contact.normal.z)
                .normalized
                * bounciness
                * moveTo.magnitude;
            
            startBounceForce = Vector3.ClampMagnitude(startBounceForce, maxBounce);
                //* collision.relativeVelocity.magnitude * 10;
                // * rb.velocity.magnitude; 
            Debug.Log(collision.relativeVelocity.magnitude);
            Debug.DrawLine(this.transform.position, this.transform.position + contact.normal , Color.magenta, 5f);
            bounceForce = startBounceForce;
            brBounce = 0;
            bounced = true;
        }
        else{
            Damagable obj = collision.gameObject.GetComponent<Damagable>();
            if(!damaged.Contains(obj)){
                Effect ef = new Effect(EffectType.Knock, EffectSource.Zombi, 5, this.transform.position);
                obj.TakeDmg(ef);
                damaged.Add(obj);
            }
        }
    }

    public override void TakeDmg(Effect dmg){
        if(dmg.source!=EffectSource.Zombi)
            hp-=dmg.val;


        if(hp<=0){
            Instantiate(particleDeath,this.transform.position,particleDeath.transform.rotation);
            manager.Kill(this);
        }
    }
}
