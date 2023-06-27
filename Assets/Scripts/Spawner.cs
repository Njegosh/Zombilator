using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Spawner : MonoBehaviour
{
    [SerializeField]
    public Bounds bounds;
    [SerializeField]
    public int danger;
    public ZombieTypes zombieType;

    public bool canSpawn = true;

    Color[] cols = new Color[]{
        new Color(0f, 1f, 0f, 0.2f),
        new Color(0.85f, 0.85f, 0.1f, 0.2f),
        new Color(1f, 0.6f, 0.02f, 0.2f),
        new Color(1f, 0f, 0f, 0.2f),
    };

    #if UNITY_EDITOR
    void OnDrawGizmos(){
        Gizmos.color = cols[danger];
        Gizmos.DrawCube(transform.position, bounds.size);
    }
    #endif

    public ZombieSpawn Spawn(){
        List<Vector3> pos = new List<Vector3>();

        if(canSpawn){
            int maxZombies = (int)(bounds.size.x / 3f) *  (int)(bounds.size.z / 3f);
            int num = (int)(Mathf.Clamp((float)(danger + 1)/ 4, 0.15f, 1) * maxZombies);

            List<Vector3> could = new List<Vector3>();

            for (int i = 0; i < (int)bounds.size.x / 3f; i++)
            {
                for (int j = 0; j < (int)bounds.size.z / 3f; j++)
                {
                    float x = 
                        (this.transform.position.x - bounds.size.x/2) +
                        bounds.size.x / 3f * i;
                    float z = 
                        (this.transform.position.z - bounds.size.z/2) +
                        bounds.size.z / 3f * j;

                    could.Add(new Vector3(x, this.transform.position.y, z));
                }
            }

            for (int i = 0; i < could.Count; i+=(int)maxZombies/num)
            {
                pos.Add(could[i]);
            }

            canSpawn = false;
        }

        ZombieSpawn zs = new ZombieSpawn(zombieType, pos);

        return zs;
    }
}
