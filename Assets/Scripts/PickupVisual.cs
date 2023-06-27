using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class PickupVisual : MonoBehaviour
{
    public GameObject white;

    [SerializeField]
    public GameObject item;

    void OnTriggerEnter(Collider other) {
        //white.SetActive(other.gameObject.tag == "Player");
        if(other.gameObject.tag == "Player"){
            Item i = item.GetComponent<Item>();
            bool picked = other.GetComponent<PlayerControls>().Equip(i);
            if(picked)
                Destroy(this.gameObject);
        }

    }
    void OnTriggerExit(Collider other) {
        //white.SetActive(!(other.gameObject.tag == "Player"));
    }
}
