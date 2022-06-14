using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Player controlls and Player behaviour
/// </summary>
public class Player : GameCharacter
{
    public int currentHealth = 100;
    public int maxHealth = 100;
    public bool isDead = false;
    [SerializeField] private AudioClip hitSound;
    [SerializeField] private AudioClip deathSound;
    [SerializeField] private AudioClip throwSound;
    [SerializeField] private AudioClip healthSound;
    private AudioSource soundsSource;
    private float horizontalInput;
    private float verticalInput;
    private bool isAtacked = false;

    void Awake()
    {
        Speed = 25.0f;
        xRangeBound = 20.0f;
        zTopBound = 22.5f;
        zBotBound = 0f;
        characterRb = GetComponent<Rigidbody>();
        characterAnimator = GetComponent<Animator>();
        soundsSource = GameObject.Find("SoundsSource").GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isDead)
        {
            RotatingTowardsMouse();
            if (GameManager.gameInProgress)
            {
                GetInput();
                ChangeRunningAnimation();
                if (Input.GetMouseButtonDown(0))
                {
                    ThrowFood();
                }
            }
        }
        CheckMapBounds();
    }

    void FixedUpdate()
    {
        if (!isAtacked)
        {
            Move(new Vector3(horizontalInput, 0, verticalInput));
        }
        if (isDead)
        {
            StopMoving();
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            var enemy = collision.gameObject;
            var damage = enemy.GetComponent<Enemy>().damage;
            var forceDirection = transform.position - enemy.transform.position;
            var time = 0.5f;

            if (!isAtacked)
            {
                isAtacked = true;
                StartCoroutine(ApplyDamagePerTime(damage, forceDirection, time));
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("HealthBonus"))
        {
            currentHealth = maxHealth;
            other.gameObject.SetActive(false);
            soundsSource.PlayOneShot(healthSound);
        }
    }

    protected override void CheckMapBounds()
    {
        //Horizontal boundaries
        if (transform.position.x < -xRangeBound)
        {
            transform.position = new Vector3(-xRangeBound, transform.position.y, transform.position.z);
        }
        else if (transform.position.x > xRangeBound)
        {
            transform.position = new Vector3(xRangeBound, transform.position.y, transform.position.z);
        }
        //Vertical boundaries
        if (transform.position.z < zBotBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zBotBound);
        }
        else if (transform.position.z > zTopBound)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, zTopBound);
        }
    }

    private IEnumerator ApplyDamagePerTime(int damage, Vector3 forceDirection, float time)
    {
        ApplyDamage(damage);
        soundsSource.PlayOneShot(hitSound);
        characterRb.AddForce(forceDirection * 500.0f, ForceMode.Impulse);
        characterAnimator.SetBool("Grounded", false);
        yield return new WaitForSeconds(time);
        isAtacked = false;
        characterAnimator.SetBool("Grounded", true);
    }

    private void ApplyDamage(int damage)
    {
        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        int deathType = Random.Range(1, 3);

        soundsSource.PlayOneShot(deathSound);
        isDead = true;
        StopGettingInput();
        characterAnimator.SetInteger("DeathType_int", deathType);
        characterAnimator.SetBool("Death_b", true);
    }

    private void StopGettingInput()
    {
        horizontalInput = 0f;
        verticalInput = 0f;
    }

    private void StopMoving()
    {
        characterRb.angularVelocity = Vector3.zero;
        characterRb.velocity = Vector3.zero;
    }

    private void ThrowFood()
    {
        GameObject food = ObjectPooler.SharedInstance.GetPooledObject();

        if (food != null)
        {
            food.transform.position = transform.position + transform.forward * 1.5f;
            food.transform.rotation = transform.rotation;
            food.SetActive(true);
            soundsSource.PlayOneShot(throwSound);
        }
    }

    private void GetInput()
    {
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");
    }

    private void RotatingTowardsMouse()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength))
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            //Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);

            transform.LookAt(new Vector3(pointToLook.x, transform.position.y, pointToLook.z));
        }
    }

    private void ChangeRunningAnimation()
    {
        if (horizontalInput != 0 || verticalInput != 0)
        {
            characterAnimator.SetFloat("Speed_f", 0.55f);
        }
        else
        {
            characterAnimator.SetFloat("Speed_f", 0f);
        }
    }
}