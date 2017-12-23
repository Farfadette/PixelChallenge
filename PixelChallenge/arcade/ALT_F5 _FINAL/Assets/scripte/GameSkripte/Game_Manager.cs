using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Manager : MonoBehaviour
{
    public GameObject deadZone;
    public GameObject painZone;
    public GameObject safeZone;
    public GameObject deadObj;
    private bool objUN = true;
    private float timeLeft = 2f;
    private float timeLeftS = 8f;
    public GameObject playerUN;
    public GameObject playerDEUX;
    private bool Gstart = false;
    public bool p1IsDead = false;
    public bool p2IsDead = false;
    public bool p3IsDead = false;
    public bool p4IsDead = false;
    private bool canRestart = true;
    private GameObject splashRestart;

    void Start()
    {
        Instantiate(painZone, new Vector3(0f, 0f, -0.2f), Quaternion.identity);
        Instantiate(safeZone, new Vector3(0f, 0f, -0.3f), Quaternion.identity);
        Instantiate(deadZone, new Vector3(0f, 0f, -0.1f), Quaternion.identity);
        Instantiate(playerUN, new Vector3(0f, 3f, -1f), Quaternion.identity);
        Instantiate(playerDEUX, new Vector3(3f, -0f, -1f), Quaternion.identity);
        splashRestart = GameObject.FindWithTag("SpaceToRestart");
        splashRestart.gameObject.SetActive(false);
    }


    void Update()
    {

        timeLeft -= Time.deltaTime;
        if (timeLeft < 0)
        {
            RandObstacle();
            timeLeft = 2f;
        }


    }
    public void RandObstacle()
    {
        float randomRange = Random.Range(1.5f, 7f);
        float randomAngle = Random.Range(0f, 359f);

        float posX = Mathf.Cos(randomAngle) * randomRange;
        float posY = Mathf.Sin(randomAngle) * randomRange;

        Vector3 randomPoint = new Vector3(posX, posY, -1f);

        Instantiate(deadObj, randomPoint, Quaternion.identity);
    }

    public void Player1Death()
    {
        Debug.Log("imdead1");
        p1IsDead = true;
        CheckWin();
    }

    public void Player2Death()
    {
        Debug.Log("imdead2");
        p2IsDead = true;
        CheckWin();
    }

    public void Player3Death()
    {
        Debug.Log("imdead3");
        p3IsDead = true;
        CheckWin();
    }

    public void Player4Death()
    {
        Debug.Log("imdead4");
        p4IsDead = true;
        CheckWin();
    }

    public void CheckWin()
    {
        Debug.Log("tcheckwin");

        if (!p1IsDead && p2IsDead && p3IsDead && p4IsDead)
        {
            canRestart = true;
        }
        else if (p1IsDead && !p2IsDead && p3IsDead && p4IsDead)
        {
            canRestart = true;
        }
        else if (p1IsDead && p2IsDead && !p3IsDead && p4IsDead)
        {
            canRestart = true;
        }
        else if (p1IsDead && p2IsDead && p3IsDead && !p4IsDead)
        {
            canRestart = true;
        }

        if (canRestart)
        {
            splashRestart.gameObject.SetActive(true);
        }
    }
    void LateUpdate()
    {

        if (ArcadeInput.Instance.GetSelectKeyDown() && !Gstart)
        {
            Gstart = true;
            Application.LoadLevel("ALT_F5");
        }

    }
}
