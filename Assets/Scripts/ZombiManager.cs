using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombiManager : MonoBehaviour
{

    public int max = 100;

    public float tickRate = 0.1f;

    public GameObject zombiPrefab;

    public Transform cilj;

    public List<Spawner> spawns = new List<Spawner>();
    List<Enemy> zombies = new List<Enemy>();
    List<Enemy> batchZombies = new List<Enemy>();
    List<Enemy> activeZombies = new List<Enemy>();

    public float minSpawnDistance = 100;
    public float DespawnDistance = 3000;
    public float EnableDistance = 300;
    public int spawnTime = 100; //In ticks

    [SerializeField]
    public List<GameObject> zombiPrefabs;


    void Start()
    {
        cilj = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        spawns = new List<Spawner>();
        
        StartCoroutine(StartPhase());
    }
    IEnumerator StartPhase(){
        
        for (int i = 0; i < max; i++)
        {
            GameObject g = Instantiate(zombiPrefabs[0],
                        new Vector3(Random.Range(-100,100), 0, Random.Range(-100,100)),
                        Quaternion.identity);
            Enemy z = g.GetComponent<Enemy>();
            z.manager = this;
            zombies.Add(z);
            batchZombies.Add(z);
            g.SetActive(false);
        }
        for (int i = 0; i < 20; i++)
        {
            GameObject g = Instantiate(zombiPrefabs[1],
                        new Vector3(Random.Range(-100,100), 0, Random.Range(-100,100)),
                        Quaternion.identity);
            Enemy z = g.GetComponent<Enemy>();
            z.manager = this;
            zombies.Add(z);
            batchZombies.Add(z);
            g.SetActive(false);
        }
        
        // NADJI SPAWNOWE
        GameObject[] rsGameObject = GameObject.FindGameObjectsWithTag("Respawn");
        
        foreach(GameObject o in rsGameObject){
            spawns.Add(o.GetComponent<Spawner>());
        }

        // SPAWNUJ
        SpawnAll();


        yield return null;
        StartCoroutine(tick());
    }

    public int ticks = 0;

    IEnumerator tick(){
        while (true)
        {
            yield return new WaitForSeconds(tickRate);
            foreach (Enemy z in activeZombies)
            {
                z.Tick(cilj);
            }

            ticks = (ticks + 1) % spawnTime;
            if(ticks==0){
                SpawnAll();
            }

            // REBATCHING
            ReBach();
        }
    }

    void ReBach(){
        foreach (Enemy z in activeZombies)
        {
            if(Vector3.Distance(z.transform.position, cilj.position) > DespawnDistance){
                z.gameObject.SetActive(false);
            }
            if(!z.gameObject.active){
                batchZombies.Add(z);
            }
        }
        foreach (Enemy z in batchZombies)
        {
            if(activeZombies.Contains(z)){
                activeZombies.Remove(z);
            }
        }
    }

    void SpawnAll(){
        foreach(Spawner s in spawns){
            float diag =  Mathf.Sqrt(Mathf.Pow(s.bounds.size.x/2, 2) + Mathf.Pow(s.bounds.size.z/2, 2));
            if(Vector3.Distance(s.transform.position, cilj.position) > EnableDistance + diag){
                s.canSpawn = true;
            }
            if(Vector3.Distance(s.transform.position, cilj.position) > minSpawnDistance &&
               Vector3.Distance(s.transform.position, cilj.position) < DespawnDistance  - diag){
                
                ZombieSpawn zs = s.Spawn();
                List<Vector3> pos = zs.pos;

                foreach(Vector3 p in pos){
                    if(batchZombies.Count>0){
                        Enemy z = null;

                        foreach(Enemy e in batchZombies){
                            if(e.type == zs.zombieType){
                                z = e;
                                break;
                            }
                        }
                        if(z){
                            activeZombies.Add(z);
                            batchZombies.Remove(z);

                            z.transform.position = p;
                            z.gameObject.SetActive(true);
                        }
                    }
                }
            }
        }
    }

    public void Kill(Enemy z){
        z.gameObject.SetActive(false);
    }
}