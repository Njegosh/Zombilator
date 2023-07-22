using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[SelectionBase]
public class Debeljaner : Enemy
{
    bool ide = false;
    public float startDistance = 15f;

    int ti = 0;
    int tr = 5;

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

    public bool rolling;

    AudioSource audioSource;

    public List<AudioClip> clipsIdle;
    public List<AudioClip> clipsAngry;
    public List<AudioClip> clipsRunning;
    public AudioClip bounce;
    public AudioClip umiranje;
    public int chanceAudio = 5;
    public int chanceAudioRunning = 5;


    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;

        cilj = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        agent = this.GetComponent<NavMeshAgent>();
        rb = this.GetComponent<Rigidbody>();
        audioSource = this.GetComponent<AudioSource>();

        destination = this.transform;

        bounceForce  = Vector3.zero;
        startBounceForce = Vector3.zero;

        moveTo = this.transform.forward;
        moveWant = this.transform.forward;
    }

    bool bounced;

    Vector3 d;

    bool canMove = true;
    bool boutToExplode = false;

    public override void Tick(Transform _destination){
        if(this.gameObject.active)
        destination = _destination;
        
        d = destination.position;

        damaged = new List<Damagable>();

        if(canMove){
            ti =  ti + 1;
            if(ti >= tr){
                agent.destination = this.transform.position + new Vector3(Random.Range(-15,+15),0, Random.Range(-15,+15));
                tr = Random.Range(3,100);
                ti=0;
                if(Random.Range(0,100)<=chanceAudio){
                    audioSource.enabled = true;
                    audioSource.volume = 0.5f;
                    audioSource.PlayOneShot(clipsIdle[Random.Range(0,clipsIdle.Count)]);
                }
            }

            if(Vector3.Distance(this.transform.position, destination.position)<startDistance){
                canMove = false;
                anim.SetTrigger("Angry");
                agent.enabled = false;
                rb.isKinematic = false;
                moveWant = this.transform.forward;
                audioSource.PlayOneShot(clipsAngry[Random.Range(0,clipsAngry.Count)]);
            }
        }
    }


    void FixedUpdate(){ 

        if(rolling){
        Vector3 p = new Vector3(
            d.x - this.transform.position.x,
            0,
            d.z - this.transform.position.z
        ).normalized;
        if(!audioSource.isPlaying){
            audioSource.clip = clipsRunning[Random.Range(0,clipsRunning.Count)];
            audioSource.Play();
        }

        if(!boutToExplode)
            moveWant = Vector3.RotateTowards(moveWant, p, turnSpeed * Time.deltaTime, 0).normalized;
        Vector3 moveWant2 = moveWant  * speed + bounceForce;

        moveTo = Vector3.ClampMagnitude(moveWant2, speed);

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
    }

    void OnCollisionEnter(Collision collision){
        if(collision.gameObject.layer != 6 && collision.gameObject.layer != 3 && collision.gameObject.layer != 8){
            ContactPoint contact = collision.contacts[0];

            startBounceForce = new Vector3(
                contact.normal.x,
                0,
                contact.normal.z)
                .normalized
                * bounciness
                * moveTo.magnitude;
            
            startBounceForce = Vector3.ClampMagnitude(startBounceForce, maxBounce);
            moveWant = Vector3.ClampMagnitude(startBounceForce * speed, speed);
            
            audioSource.PlayOneShot(bounce);
                //* collision.relativeVelocity.magnitude * 10;
                // * rb.velocity.magnitude; 

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

    public float hitValue;
    public override void TakeDmg(Effect dmg){
        if(dmg.source!=EffectSource.Zombi){
            hp-=dmg.val;
            if(dmg.type == EffectType.Push){
                Debug.Log("PUSHERD");
                startBounceForce = this.transform.position - dmg.origin;
                bounceForce = new Vector3(
                    bounceForce.x,
                    0,
                    bounceForce.z
                );
                startBounceForce = startBounceForce * dmg.val * hitValue;
                brBounce = 0;
                bounced = true;
                audioSource.PlayOneShot(bounce);
                //moveWant = Vector3.ClampMagnitude(startBounceForce, speed);
            }
        }

        if(hp<=0){
            boutToExplode = true;
            audioSource.clip = umiranje;
            audioSource.Stop();
            audioSource.volume = 1;
            audioSource.Play();
            anim.SetTrigger("Explode");
        }
    }
    public void Die(){
        if(hp<=0){
            Instantiate(particleDeath,this.transform.position,particleDeath.transform.rotation);
            manager.Kill(this);
        }
    }
}
