using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Monster : MonoBehaviour {
    public int health = 10;
    private Rigidbody rb;

    public GameObject player;
    private Rigidbody player_rb;
    public Health player_health;
    public float stun_time = 4.0f;
    public float attack_range = 4.0f;
    public float attack_rate = 10f;
    public float last_attack = 0;
    private float total_stun_time = 0;
    private BoxCollider vision_square;
    public SphereCollider vision_circle;
    public Vector3 last_sight; //the target where the monster will charge to. 
    public float cast_time = 0; //as cast time hits some X value, cast the spell. 
    public float cast_time_increment = .01f; //how fast the cast time increments
    public float charge_time = 100f;
    public float charge_speed = 10f; //How fast the monster will charge at the player
    public float charge_scalar = 4f;
    public bool channeling_move = false; //waited cast time?
    public float chasing_speed = 2.0f; //regular chasing speed. 
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
        agent = GetComponent<NavMeshAgent>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        player = GameObject.FindGameObjectWithTag("Player");
        player_rb = player.GetComponent<Rigidbody>();
        player_health = player.GetComponent<Health>();
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
        if (collision.gameObject.tag == "Player")
        {
            player_rb.AddForce(rb.velocity);
        }
    }
    private void FixedUpdate()
    {
        if(this.health > 0 && player_health.health > 0)
        {
            if(state == ENEMY_STATES.IDLE)
            {
                agent.isStopped = true;
                rb.velocity = Vector3.zero;
            }
            else if (state == ENEMY_STATES.CHASING)
            {
               agent.isStopped = false;
               this.gameObject.transform.LookAt(player.gameObject.transform);
               agent.destination = player.gameObject.transform.position;
                //agent.Warp(player.gameObject.transform.position);
            }
            else if(state == ENEMY_STATES.STUNNED)
            {
               agent.isStopped = true;
               total_stun_time += Time.deltaTime;
               if (total_stun_time >= stun_time)
               {
                   total_stun_time = 0;
                   state = ENEMY_STATES.CHASING;
                   
               }
            }
            else if(state == ENEMY_STATES.ATTACKING)
            {
                Debug.Log("CHARGING "+ cast_time);
             
                charge();
            }
        }
    }
    /*
     * Waits certain time and the returns true.
     */
    private bool waiting_cast_time(float time)
    {
        cast_time += cast_time_increment* Time.deltaTime;
            if (cast_time >= time)
            {
                cast_time = 0;
                return true;
            }
        return false;
    }

    private void charge()
    {
        if (channeling_move && waiting_cast_time(charge_time))
        {
            agent.isStopped = false;
            agent.destination = (last_sight - this.transform.position).normalized * charge_scalar + this.transform.position;
            agent.speed = charge_speed;
            channeling_move = false;
        }
        else
        {
            if (channeling_move)
            {
                agent.isStopped = true;
            }
            else
            {
                if(agent.pathStatus == NavMeshPathStatus.PathComplete && agent.remainingDistance == 0)
                {
                    state = ENEMY_STATES.CHASING;
                }
            }
        }
    }
}
