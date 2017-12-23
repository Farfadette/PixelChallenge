using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1Movement : MonoBehaviour
{
    private GameObject player;
    private GameObject energyWave;
    private Rigidbody2D rigidBody;
    private float moveSpeed;
    private float rotationSpeed;
    private float moveForward;
    private bool canDash = false;
    private float timerDash = 1;
    private float nextDash = 0.0F;
    private bool movingBackward;
    private float healthPoints = 30F;
    private bool death = false;
    private float distanceFromCenter;
    private float damageTime = 1F;
    private float damageTake = 0.0F;
    private float doubleClickTime;
    private float timeToAdd = 0.25F;
    private bool waveActive = false;
    private float waveTime = 0.0F;
    private float waveNewTime = 0.5F;
    private Light playerGlow;
    private TrailRenderer playerTrail;


    // Use this for initialization
    private void Start()
    {
        player = GameObject.FindWithTag("Player1");
        energyWave = GameObject.FindWithTag("EnergyWave1");
        energyWave.SetActive(false);
        rigidBody = GetComponent<Rigidbody2D>();
        playerGlow = GetComponent<Light>();
        playerTrail = GetComponent<TrailRenderer>();

    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log(timerDash);
        Debug.Log(transform.rotation);

        distanceFromCenter = Vector2.Distance(Vector2.zero, player.transform.localPosition);

        if (distanceFromCenter > 7.5F)
        {
            death = true;
        }

        if (Input.GetKey(KeyCode.W))
        {
            movingBackward = false;
            moveSpeed = 75F;

            if (Input.GetKeyDown(KeyCode.W) && Time.time < doubleClickTime && Time.time > nextDash)
            {
                rigidBody.AddForce(transform.up * 1250F);
                nextDash = Time.time + timerDash;
                doubleClickTime = Time.time;
                energyWave.SetActive(true);
                waveTime = Time.time + waveNewTime;
            }
            if (Input.GetKeyDown(KeyCode.W))
            {
                doubleClickTime = Time.time + timeToAdd;
            }
        }
        else if (Input.GetKey(KeyCode.S))
        {
            movingBackward = true;
            moveSpeed = -75F;
        }
        else
        {
            moveSpeed = 0F;
        }

        if (Time.time > waveTime)
        {
            energyWave.SetActive(false);
        }

        if (Time.time < nextDash)
        {
            playerTrail.Clear();
        }

        if (Input.GetKey(KeyCode.A))
        {
            rotationSpeed = 8F;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            rotationSpeed = -8F;
        }
        else
        {
            rotationSpeed = 0F;
        }

        if (distanceFromCenter > 1)
        {
            if (Time.time > damageTake)
            {
                damageTake = Time.time + damageTime;
                healthPoints -= 1;
                playerGlow.intensity -= 0.06f;
            }
        }

        Debug.Log(healthPoints);

        if (healthPoints <= 0)
        {
            death = true;
        }

        if (death)
        {
            mort();
            Destroy(player);
        }

        rigidBody.AddForce(transform.up * moveSpeed);
        rigidBody.rotation = rigidBody.rotation + rotationSpeed;
    }

    public void mort()
    {
		Camera.main.GetComponent<Game_Manager>().Player1Death();
        Camera.main.GetComponent<CameraShake>().ShakeIt();
        Debug.Log("SHAKE");
    }

    private void OnCollisionEnter2D (Collision2D aCollider)
    {
		if (aCollider.gameObject.tag == "deadZone")
        {
            mort();
			Destroy (player);
        }
    }
}
