using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : MonoBehaviour
{
    [SerializeField]
    //players movement speed
    private float MoveSpeed = 3f;

    [SerializeField]
    //Players index, used for when we add the second player, it will then increment by 1 for player 2
    private int playerIndex = 0;

    private CharacterController controller;
    private Vector3 moveDirection = Vector3.zero;//Stores the movement direction
    private Vector2 inputVector = Vector2.zero;//Stores input direction from the player

    private Vector3 velocity = Vector3.zero;//Keeps track of current velocity to smooth the movement
    private float smoothTime = 0.1f;//Time needed to smooth out movement

    private void Awake()
    {
        //gets the character controller, if its missing, gives an error
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogError("CharacterController component not found on " + gameObject.name);
        }
    }

    //getter for player index
    public int GetPlayerIndex()
    {
        return playerIndex;
    }

    //setter to update movement direction
    public void SetInputVector(Vector2 direction)
    {
        inputVector = direction;
    }

    void FixedUpdate()
    {
        if (controller == null)
        {
            Debug.LogError("CharacterController is still missing during FixedUpdate!");
            return;
        }

        //Converts input to 3D direction and apply player’s current rotation
        Vector3 targetMoveDirection = new Vector3(inputVector.x, 0, inputVector.y);
        targetMoveDirection = transform.TransformDirection(targetMoveDirection) * MoveSpeed;

        //Smoothly chnage the current velocity towards the new direction, making movement less jittery
        velocity = Vector3.SmoothDamp(velocity, targetMoveDirection, ref velocity, smoothTime);

        //Moves the player using delta time (so that the movement is frame independant)
        controller.Move(velocity * Time.fixedDeltaTime);
    }
}
