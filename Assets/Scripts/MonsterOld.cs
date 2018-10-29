using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterOld : MonoBehaviour {
    public int health = 10;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "projectile")
        {
            collision.gameObject.tag = "Untagged";
            health--;
            //Destroy(collision.gameObject);
        }
        if(health <= 0)
        {
            Destroy(transform.gameObject);
        }
    }
}
