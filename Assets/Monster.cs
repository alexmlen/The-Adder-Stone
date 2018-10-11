﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster : MonoBehaviour {
    public int health = 10;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "projectile")
        {
            health--;
            Destroy(collision.gameObject);
        }
        if(health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}