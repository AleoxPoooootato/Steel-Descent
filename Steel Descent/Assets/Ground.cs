using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public float x;
    public float z;
    public float x1;
    public float z1;
    public double x2;
    public double z2;
    public float x3;
    public float z3;
    public bool isMoving;
    public float moveSpeed;
    public Vector3 move;
    public CharacterController player;

    // Update is called once per frame
    void Update()
    {

        //grabs direction of the player WASD, including diagonals
        x = Input.GetAxisRaw("Horizontal");
        z = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(x) == Mathf.Abs(z) && Mathf.Abs(x) == 1)
        {
            x /= Mathf.Sqrt(2);
            z /= Mathf.Sqrt(2);
        }

        //checks if movement input is happening
        if (x != 0 | z != 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }

        if (isMoving)
        {
            x1 += x * Time.deltaTime / 2;
            z1 += z * Time.deltaTime / 2;
        }
        else if (x1 > 0 | z1 > 0)
        {
            x1 -= Time.deltaTime;
            z1 -= Time.deltaTime;
        }
        x2 = System.Math.Tanh((double)x1);
        z2 = System.Math.Tanh((double)z1);

        x3 = (float)x2;
        z3 = (float)z2;

        if (x1 < 0 | z1 < 0)
        {
            x1 = 0;
            z1 = 0;
        }

        //creates the move Vector3
        move = (transform.right * x3 + transform.forward * z3) * moveSpeed;

        //moves the player by the amount specified by move
        player.Move(move);
    }
}
