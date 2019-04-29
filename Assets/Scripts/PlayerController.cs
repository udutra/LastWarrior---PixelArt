using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D playerRb;
    public float speed, jumpForce;
    public bool isLookLeft, isGrounded;
    public Animator playerAnimator;
    public Transform groundCheck, mao;
    public bool isAttack;
    public GameObject hitBoxPrefab;
    private GameController _gameController;

    private void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
        _gameController.playerTransformer = this.transform;
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if (isAttack && isGrounded)
        {
            h = 0;
        }

        if ((h > 0 && isLookLeft) || (h < 0 && !isLookLeft))
        {
            Flip();
        }

        float speedY = playerRb.velocity.y;

        if (Input.GetButtonDown("Jump") &&  isGrounded)
        {
            _gameController.PlaySFX(_gameController.sfxJump, 0.5f);
            playerRb.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButtonDown("Fire1") && !isAttack)
        {
            _gameController.PlaySFX(_gameController.sfxAttack, 0.5f);
            isAttack = true;
            playerAnimator.SetTrigger("attack");
        }

        playerRb.velocity = new Vector2(h * speed, speedY);

        playerAnimator.SetInteger("horizontal", (int)h);
        playerAnimator.SetBool("isGrounded", isGrounded);
        playerAnimator.SetFloat("speedY", speedY);
        playerAnimator.SetBool("isAttack", isAttack);
    }
    private void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.02f);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if(col.gameObject.tag == "Coletavel")
        {
            _gameController.PlaySFX(_gameController.sfxCoin, 0.5f);
            Destroy(col.gameObject);
        }
        else if(col.gameObject.tag == "Damage")
        {
            print("Dano");
        }
    }

    private void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    private void OnEndAttack()
    {
        isAttack = false;
    }

    private void HitBoxAttack()
    {
        GameObject hitBoxTemp = Instantiate(hitBoxPrefab, mao.position, transform.localRotation);
        Destroy(hitBoxTemp, 0.2f);
    }

    private void FootStep()
    {
        _gameController.PlaySFX(_gameController.sfxStep[Random.Range(0, _gameController.sfxStep.Length)], 1f);
    }

    
}
