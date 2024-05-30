using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

[SelectionBase]
public class PlayerControls : Damagable
{
    public float jumpSpeed;
    public float speed = 0;
    public bool canJump = true;
    public bool falling = false;

    public float gravity = 1.9f;

    [SerializeField]
    float coyoteTime;

    int layerMask = 0b_100000001;

    public Transform cameraAim;
    
    CharacterController r;

    public Transform playerModel;
    public Animator anim;

    [SerializeField]
    public Usable usable;

    public GameObject usableObject;

    public List<Usable> items = new List<Usable>();
    public int itemSelected;

    Hat hat;
    public Transform usableHolder;

    public Transform hatHolder;

    public Transform usableUIholder;
    public UsableUI usableUI;
    public GameObject usableUIgameobject;
    public CinemachineCameraOffset camOffset;
    public CinemachineBrain cb;

    public GameObject pauseMenu;

    void Start()
    {
        r = this.GetComponent<CharacterController>();

        items.Add(null);
        items.Add(null);
        items.Add(null);
    }
    Vector3 velocity = new Vector3();
    Vector3 lookAt = Vector3.forward;
    public float fallLimit;
    float camY = 0;
    public float camSpeed = 1;

    float y;

    public Transform aim;

    public Vector3 mousePos;
    public Vector3 transpose;

    public float camSeeRange = 2;


    public int maxHp = 20;
    public int hp = 20;


    public RectTransform playerUIrect;
    public PlayerUI playerUI;
    public RectTransform dedUI;

    public bool paused;

    public GameObject particleDeath;

    void Update()
    {


        if(usable != null){
            if(Input.GetKeyDown(KeyCode.Mouse0)) usable.UsePrimary();
            if(Input.GetKeyDown(KeyCode.Mouse1)) usable.UseAlt();
            if(Input.GetKeyDown(KeyCode.R)) usable.Reload();
        }
        if(Input.GetKeyDown(KeyCode.Escape)){
            cb.enabled = false;
            pauseMenu.SetActive(true);
        }

        if(!paused){
            camY = cameraAim.position.y;

            mousePos = new Vector3((Input.mousePosition.x / Screen.width) * 2 - 1, (Input.mousePosition.y / Screen.height) * 2 - 1);
            transpose = new Vector3(mousePos.x , Mathf.Cos(50) * mousePos.y, Mathf.Sin(50) * -mousePos.y);

            camOffset.m_Offset = transpose * camSeeRange;


            if(r.isGrounded) camY = this.transform.position.y;
            
            cameraAim.position = new Vector3(this.transform.position.x, Mathf.Lerp(cameraAim.position.y, camY, Time.deltaTime * camSpeed) ,this.transform.position.z);
            UI();

            Kretanje();
            Ciljaj();
            DodatnaAnim();
        }
    }
    void UI(){
        // CENRIRANJE
        Vector3 myScreenPos = Camera.main.WorldToScreenPoint(this.transform.position);
        playerUIrect.position = myScreenPos;

        // POKAZI INVENTORY
        if(Input.GetKeyDown(KeyCode.Tab)) playerUI.ShowHideInventory();

        // MENJANJE ITEMA
        int i = -1;
        if(Input.GetKeyDown(KeyCode.Alpha1)) i=0;
        if(Input.GetKeyDown(KeyCode.Alpha2)) i=1;
        if(Input.GetKeyDown(KeyCode.Alpha3)) i=2;
        if(Input.GetKeyDown(KeyCode.Q)) i=3;

        if(i!=-1){
            if(i<3){
                Destroy(usableObject);
                Destroy(usableUIgameobject);
                if(items[i]!=null){
                    usableObject = Instantiate(items[i].Model, usableHolder);
                    usable = usableObject.GetComponent<Usable>();

                    usableUIgameobject = Instantiate(usable.usableUIobject, usableUIholder);
                    usableUI = usableUIgameobject.GetComponent<UsableUI>();
                    usable.usableUI = usableUI;
                }
                else{
                    usableObject = null;
                    usable = null;
                    usableUI = null;
                }
            }
            playerUI.Select(i);
        }
    }

    void Ciljaj(){
        Vector3 vect3 = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        Physics.Raycast(ray, out hit, 1000, layerMask);

        aim.position = hit.point;
        
        playerModel.LookAt(new Vector3(aim.position.x, this.transform.position.y, aim.position.z), Vector3.up);
    }

    void Kretanje(){
        Vector3 smer = new Vector3();
        smer.x = Input.GetAxis("Horizontal");
        smer.z = Input.GetAxis("Vertical");

        smer = Vector3.ClampMagnitude(smer, 1);
        
        anim.SetBool("Run", smer.magnitude > 0);

        if(smer.magnitude>0.1)
            lookAt = smer * 10;

        velocity = smer *speed;

        if(Input.GetKeyDown(KeyCode.Space) && r.isGrounded){
            y =  jumpSpeed;
            anim.SetTrigger("Jump");
        }
        anim.SetFloat("FallSpeed", Mathf.Abs(y/fallLimit));

        if(r.isGrounded){
            anim.SetTrigger("Landed");
        }
        else             anim.ResetTrigger("Landed");

        y -= gravity * Time.deltaTime;
        if(r.isGrounded && y < -0.1f) y = -0.1f;
        velocity.y = y;

        r.Move(velocity * Time.deltaTime);

    }

    void DodatnaAnim(){
        //anim.SetLayerWeight()
        if(usable!=null){
            int index = anim.GetLayerIndex(usable.Name);
            anim.SetLayerWeight(index,1);
        }
        else{
            for(int i = 1; i< anim.layerCount; i++){
                anim.SetLayerWeight(i,0);
            }
        }
    }

    public void DoAnim(string name){
        anim.SetTrigger(name);
    }

    public bool Equip(Item item){
        Debug.Log(item.ToString());
        Debug.Log(item.GetType());
        Debug.Log(item is Usable);

        if (item is Usable) {
            //if(items.Count  <3){
            foreach(Usable us in items){
                if(us == null){
                    Usable u = (Usable)item;
                
                    int i =  items.IndexOf(us);
                    items[i] = u;
                    itemSelected = i;

                    Destroy(usableObject);
                    Destroy(usableUIgameobject);

                    usableObject = Instantiate(u.gameObject, usableHolder);
                    usable = usableObject.GetComponent<Usable>();

                    playerUI.UpdateItem(itemSelected, usable);
                    playerUI.Select(itemSelected);

                    usableUIgameobject = Instantiate(usable.usableUIobject, usableUIholder);
                    usableUI = usableUIgameobject.GetComponent<UsableUI>();
                    usable.usableUI = usableUI;
                    
                    return true;
                }
            }
        }
        else if(item is Hat){
            if(hat == null){
                Hat h = (Hat)item;
                GameObject g = Instantiate(h.gameObject, hatHolder);
                hat = g.GetComponent<Hat>();

                playerUI.UpdateItem(3, hat);
                //playerUI.Select(itemSelected);
                
                return true;
            }
        }
        // TODO: Dodati izbacivanje itema
        //Instantiate(usable.Model, this.transform.position, Quaternion.identity);

        //usable = null;

        return false;
    }

    public override void TakeDmg(Effect dmg){
        hp-=dmg.val;
        playerUI.UpdateHp(hp);
        if (hp <= maxHp/3)
        {
            playerUI.DangerHp(true);
        }
        if(hp<=0){
            Instantiate(particleDeath,this.transform.position,particleDeath.transform.rotation);
            dedUI.gameObject.SetActive(true);
            Destroy(this.gameObject);
        }
    }
}
