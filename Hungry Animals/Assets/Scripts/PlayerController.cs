using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles Player input
/// </summary>
public class PlayerController : MonoBehaviour
{
    public float horizontalInput;
    public float verticalInput;
    private GameManager gameManager;
    private Player player;

    void Awake()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        player = GetComponent<Player>();
    }

    public void GetInput()
    {
        if (!GameManager.isGamePaused)
        {
            horizontalInput = Input.GetAxis("Horizontal");
            verticalInput = Input.GetAxis("Vertical");
            if (Input.GetMouseButtonDown(0))
            {
                player.ThrowFood();
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!GameManager.isGamePaused)
            {
                gameManager.PauseGame();
            }
            else
            {
                gameManager.UnpauseGame();
            }
        }
    }

    public void StopGettingInput()
    {
        horizontalInput = 0f;
        verticalInput = 0f;
    }

    public void GetMousePosition()
    {
        Ray cameraRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        Plane groundPlane = new Plane(Vector3.up, Vector3.zero);
        float rayLength;

        if (groundPlane.Raycast(cameraRay, out rayLength) && !GameManager.isGamePaused)
        {
            Vector3 pointToLook = cameraRay.GetPoint(rayLength);
            //Debug.DrawLine(cameraRay.origin, pointToLook, Color.cyan);

            player.RotateTowardsMouse(pointToLook);
        }
    }
}
