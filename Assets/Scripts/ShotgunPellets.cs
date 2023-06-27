using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPellets : MonoBehaviour
{
    ParticleSystem particles;
    List<ParticleCollisionEvent> collisionEvents;
    // Start is called before the first frame update
    void Start()
    {
        particles = this.GetComponent<ParticleSystem>();
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // TODO: Svetla i slicno

    void OnParticleCollision(GameObject other){
        if(other.TryGetComponent(out Damagable zombi)){
            int numCollisionEvents = particles.GetCollisionEvents(zombi.gameObject, collisionEvents);
            
            Debug.Log("pellets hit: " + numCollisionEvents);

            Effect effect =  new Effect(EffectType.Push, EffectSource.Player, 2 * numCollisionEvents, this.transform.position);

            zombi.TakeDmg(effect);
        }
    }

}
