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
    public float attackDistance = 1.5f;

    int ti = 0;
    int tr = 5;

    public float idleSpeed = 5;
    public float idleAccel = 5;
    
    public float runSpeed = 17;
    public float runAccel = 15;

    public Animator anim;
    public GameObject particleDeath;

    AudioSource audioSource;

    [SerializeField]
    public List<AudioClip> clipsIdle;
    public List<AudioClip> clipsAngry;
    public List<AudioClip> clipsRunning;
    public int chanceAudio = 5;
    public int chanceAudioRunning = 5;
    // Start is called before the first frame update
    void Start()
    {
        hp = maxHp;

        agent = this.GetComponent<NavMeshAgent>();
        audioSource = this.GetComponent<AudioSource>();
        audioSource.enabled = false;
        
        NavMesh.avoidancePredictionTime = 0.5f;


        agent.speed = idleSpeed;
        agent.acceleration = idleAccel;
        agent.destination = this.transform.position + new Vector3(Random.Range(-10,+10),0, Random.Range(-10,+10));
    }

    public Transform destination;

    bool angyZvuk = false;
    bool runZvuk = false;
    int runT = 0;
    int runTmax = 10;

    public override void Tick(Transform _destination){
        if(this.gameObject.active){
        destination = _destination;
        anim.SetFloat("WalkSpeed",agent.velocity.magnitude);


        if(audioSource.enabled){
            if(!audioSource.isPlaying){
                audioSource.enabled = false;
            }
        }

        if(Vector3.Distance(this.transform.position, destination.position)<startDistance){
            ide = true;
            if(!angyZvuk){
                audioSource.enabled = true;
                audioSource.volume = .8f;
                audioSource.PlayOneShot(clipsAngry[Random.Range(0,clipsAngry.Count)]);
                angyZvuk = true;
            }
        }

        else if(Vector3.Distance(this.transform.position, destination.position)>giveUpDistance){
            ide = false;
            angyZvuk = false;

            agent.speed = idleSpeed;
            agent.acceleration = idleAccel;
        }

        if(ide) {
            agent.destination = destination.position;

            agent.speed = runSpeed;
            agent.acceleration = runAccel;

            if(Vector3.Distance(this.transform.position, destination.position)<attackDistance){
                anim.SetTrigger("Attack");
            }
            else
                anim.ResetTrigger("Attack");

            if(runT <= runTmax){
                runT++;
            }
            else {
                if(Random.Range(0,100)<=chanceAudioRunning && !audioSource.isPlaying){
                    audioSource.enabled = true;
                    audioSource.volume = 0.5f;
                    audioSource.clip = clipsRunning[Random.Range(0,clipsRunning.Count)];
                    audioSource.Play();
                }
                runT=0;
            }
        }
        else {
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
        }
        }
        
    }

    float tAttack = 0;
    Damagable damagable = null;

    public void ResumeWalking(){
        agent.isStopped = false;
    }
    public void Attack(){
        agent.isStopped = true;
        Debug.Log("tAttack: " + tAttack +
                "\nTime: " + Time.deltaTime +
                "\nDelta: " + (tAttack - Time.time));
        if(tAttack <= Time.time - 2 && damagable!= null){
            tAttack = Time.time;
            damagable.TakeDmg(
                new Effect(
                    EffectType.None,
                    EffectSource.Zombi,
                    2,
                    this.transform.position)
                );
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            damagable = other.GetComponent<PlayerControls>();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player"){
            damagable = null;
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
