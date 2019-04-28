using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public float speed, jumpForce;
    public bool isLookLeft;

    private void Start()
    {
        playerRb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if ((h > 0 && isLookLeft) || (h < 0 && !isLookLeft))
        {
            Flip();
        }

        float speedY = playerRb.velocity.y;

        if (Input.GetButtonDown("Jump"))
        {
            playerRb.AddForce(new Vector2(0, jumpForce));
        }

        playerRb.velocity = new Vector2(h * speed, speedY);
    }

    private void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
