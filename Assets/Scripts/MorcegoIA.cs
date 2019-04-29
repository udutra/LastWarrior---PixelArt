using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorcegoIA : MonoBehaviour
{
    private GameController _GameController;
    private Animator morcegoAnimator;
    private bool isFolow;
    public bool isLookLeft;
    public float speed;
    public GameObject hitBox;

    void Start()
    {
        _GameController = FindObjectOfType(typeof(GameController)) as GameController;
        morcegoAnimator = GetComponent<Animator>();
    }

    void Update()
    {
        if (_GameController.currentState != GameState.GAMEPLAY)
        {
            return;
        }

        if (isFolow)
        {
            transform.position = Vector3.MoveTowards(transform.position, _GameController.playerTransformer.position, speed * Time.deltaTime);
        }

        if(transform.position.x < _GameController.playerTransformer.position.x && isLookLeft)
        {
            Flip();
        }
        else if(transform.position.x > _GameController.playerTransformer.position.x && !isLookLeft)
        {
            Flip();
        }
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "HitBox")
        {
            isFolow = false;
            Destroy(hitBox);
            _GameController.PlaySFX(_GameController.sfxEnemyDeath, 0.3f);
            morcegoAnimator.SetTrigger("dead");
        }
    }

    private void OnBecameVisible()
    {
        isFolow = true;
    }

    private void Flip()
    {
        isLookLeft = !isLookLeft;
        float x = transform.localScale.x * -1;
        transform.localScale = new Vector3(x, transform.localScale.y, transform.localScale.z);
    }

    private void OnDead()
    {
        Destroy(this.gameObject);
    }
}
