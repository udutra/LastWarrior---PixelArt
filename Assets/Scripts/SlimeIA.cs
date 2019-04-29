using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlimeIA : MonoBehaviour
{
    private GameController _gameController;
    private Rigidbody2D slimeRb;
    private Animator slimeAnimator;
    private int h;
    public float speed, timeToWalk;
    public GameObject hitBox;
    public bool isLookLeft;

    void Start()
    {
        _gameController = FindObjectOfType(typeof(GameController)) as GameController;
        slimeRb = GetComponent<Rigidbody2D>();
        slimeAnimator = GetComponent<Animator>();
        StartCoroutine("SlimeWalk");
    }

    void Update()
    {
        
        if ((h > 0 && isLookLeft) || (h < 0 && !isLookLeft))
        {
            Flip();
        }

        slimeRb.velocity = new Vector2(h * speed, slimeRb.velocity.y);

        if (h != 0)
        {
            slimeAnimator.SetBool("isWalk", true);
        }
        else
        {
            slimeAnimator.SetBool("isWalk", false);
        }
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "HitBox")
        {
            h = 0;
            StopCoroutine("SlimeWalk");
            Destroy(hitBox);
            _gameController.PlaySFX(_gameController.sfxEnemyDeath, 0.3f);
            slimeAnimator.SetTrigger("dead");
        }
    }

    private IEnumerator SlimeWalk()
    {
        int rand = Random.Range(0, 100);

        if (rand < 33)
        {
            h = -1;
        }
        else if(rand < 66)
        {
            h = 0;
        }
        else
        {
            h = 1;
        }


        yield return new WaitForSeconds(timeToWalk);
        StartCoroutine("SlimeWalk");
    }

    private void OnDead()
    {
        Destroy(this.gameObject);
    }

    private void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }
}
