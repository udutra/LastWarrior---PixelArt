using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private GameController _GameController;
    private Rigidbody2D playerRb;
    private SpriteRenderer playerSr;
    public float speed, jumpForce;
    public bool isLookLeft, isGrounded;
    public Animator playerAnimator;
    public Transform groundCheck, mao;
    public bool isAttack;
    public GameObject hitBoxPrefab;
    public Color hitColor, noHitColor;

    private void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        _GameController.playerTransformer = this.transform;
        playerRb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerSr = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        playerAnimator.SetBool("isGrounded", isGrounded);

        if(_GameController.currentState != GameState.GAMEPLAY)
        {
            playerRb.velocity = new Vector2(0, playerRb.velocity.y);
            playerAnimator.SetInteger("horizontal", 0);
            return;
        }

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
            _GameController.PlaySFX(_GameController.sfxJump, 0.5f);
            playerRb.AddForce(new Vector2(0, jumpForce));
        }

        if (Input.GetButtonDown("Fire1") && !isAttack)
        {
            _GameController.PlaySFX(_GameController.sfxAttack, 0.5f);
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

        switch (col.gameObject.tag)
        {
            case "Coletavel":
                {
                    _GameController.PlaySFX(_GameController.sfxCoin, 0.5f);
                    _GameController.GetCoin();
                    Destroy(col.gameObject);
                    break;
                }
            case "Damage":
                {
                    _GameController.GetHit();
                    if (_GameController.vida > 0)
                    {
                        StartCoroutine("DamageController");
                    }
                    break;
                }
            case "Abismo":
                {
                    _GameController.GameOver();
                    break;
                }
            case "Flag":
                {
                    _GameController.TheEnd();
                    break;
                }
            case "Portal":
                {
                    break;
                }
            case "HitBox":
                {
                    break;
                }
            case "Untagged":
                {
                    Debug.LogWarning("Colisão sem Tag no OnTriggerEnter2D() na classe PlayerController. Nome: " + col.gameObject.name + " - Tag: " + col.gameObject.tag);
                    break;
                }
            default:
                {
                    Debug.LogWarning("Erro no switch no OnTriggerEnter2D() na classe PlayerController. Nome: " + col.gameObject.name + " - Tag: " + col.gameObject.tag);
                    break;
                }
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
        _GameController.PlaySFX(_GameController.sfxStep[Random.Range(0, _GameController.sfxStep.Length)], 1f);
    }

    private IEnumerator DamageController()
    {
        _GameController.PlaySFX(_GameController.sfxDamage,0.5f);

        this.gameObject.layer = LayerMask.NameToLayer("Invensivel");
        playerSr.color = hitColor;
        yield return new WaitForSeconds(0.2f);
        playerSr.color = noHitColor;

        for (int i = 0; i < 5; i++)
        {
            playerSr.enabled = false;
            yield return new WaitForSeconds(0.3f);
            playerSr.enabled = true;
            yield return new WaitForSeconds(0.3f);
        }
        this.gameObject.layer = LayerMask.NameToLayer("Player");
        playerSr.color = Color.white;
    }
}