using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {
    public int health = 10;
    private Rigidbody rb;
    public GameObject player;
    public float stun_time = 4.0f;
    public float total_stun_time = 0;
    private SphereCollider vision_circle;
    private BoxCollider vision_square;
    public enum ENEMY_STATES
    {
        IDLE = 0, 
        ATTACKING = 1,
        STUNNED = 2,
        CHASING = 3
    }
    public ENEMY_STATES state = ENEMY_STATES.IDLE;
    NavMeshAgent agent;
    public void Start()
    {
        vision_circle = GetComponent<SphereCollider>();
        vision_square = GetComponent<BoxCollider>();
        agent = GetComponent<NavMeshAgent>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if(state != ENEMY_STATES.STUNNED) state = ENEMY_STATES.CHASING;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            state = ENEMY_STATES.IDLE;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "projectile") {
            state = ENEMY_STATES.STUNNED;
            health--;
            rb.AddForce(collision.gameObject.transform.forward);
            Destroy(collision.gameObject);
        }
        if(health <= 0) {
            Destroy(this.gameObject);
        }
    }
    private void FixedUpdate()
    {
        if(state == ENEMY_STATES.IDLE)
        {
           
        }
        else if (state == ENEMY_STATES.CHASING)
        {
            this.gameObject.transform.LookAt(player.gameObject.transform);
            agent.destination = player.gameObject.transform.position;
        }
        else if(state == ENEMY_STATES.STUNNED)
        {
            total_stun_time += Time.deltaTime;
            if (total_stun_time >= stun_time)
            {
                total_stun_time = 0;
                state = ENEMY_STATES.CHASING;
            }
        }
       
    }
}
