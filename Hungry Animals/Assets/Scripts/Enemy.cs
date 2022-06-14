using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Enemy behaviour
/// </summary>
public class Enemy : GameCharacter
{
    public override float Speed
    {
        get { return speed; }
        protected set { speed = value; }
    }
    public int damage;
    [SerializeField] private float speed;
    [SerializeField] private int hunger;
    private Collider characterCollider;
    private Player player;
    private bool isHungry = true;
    private bool isBittingPlayer = false;

    void Awake()
    {
        xRangeBound = 30.0f;
        zTopBound = 30.0f;
        zBotBound = -10.0f;
        characterRb = GetComponent<Rigidbody>();
        characterAnimator = GetComponent<Animator>();
        characterCollider = gameObject.GetComponent<Collider>();
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    void Update()
    {
        CheckMapBounds();
        if (player.isDead && isHungry)
        {
            OnAFullStomach();
        }
    }

    void FixedUpdate()
    {
        if (isHungry)
        {
            LookAtPlayer();
            if (!isBittingPlayer)
            {
                Move((player.transform.position - transform.position).normalized);
            }
            else
            {
                Stop();
            }
        }
        else
        {
            float newSpeed = 0.1f;

            GoAway(transform.forward * newSpeed);
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isBittingPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            isBittingPlayer = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Food"))
        {
            other.gameObject.SetActive(false);
            ReduceHunger();
        }
    }

    protected override void CheckMapBounds()
    {
        Vector3 enemyPos = transform.position;

        if (enemyPos.z > zTopBound || enemyPos.z < zBotBound)
        {
            Destroy(gameObject);
        }
        else if (enemyPos.x < -xRangeBound || enemyPos.x > xRangeBound)
        {
            Destroy(gameObject);
        }
    }

    private void ReduceHunger()
    {
        hunger--;
        if (hunger == 0)
        {
            OnAFullStomach();
        }
    }

    private void OnAFullStomach()
    {
        isHungry = false;
        characterRb.isKinematic = true;
        characterCollider.enabled = false;
        characterAnimator.SetFloat("Speed_f", 0.5f);
        transform.Rotate(new Vector3(0, 180, 0));
    }

    private void Stop()
    {
        characterRb.velocity = Vector3.zero;
    }

    private void GoAway(Vector3 direction)
    {
        characterRb.MovePosition(transform.position + direction);
    }

    private void LookAtPlayer()
    {
        transform.LookAt(player.transform.position);
    }
}
