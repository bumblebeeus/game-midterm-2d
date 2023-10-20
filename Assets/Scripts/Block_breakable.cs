using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block_breakable : MonoBehaviour
{
    [SerializeField] float respawnDelay = 5;
    [SerializeField] float duration = 3;
    [SerializeField] bool isBreaking;
    [SerializeField] bool isRespawned = true;

    [SerializeField] Transform target;

    float timeToDestroy;
    SpriteRenderer spriteRenderer;
    PolygonCollider2D collider;

    void Start()
    {
        timeToDestroy = duration;
        spriteRenderer = target.GetComponent<SpriteRenderer>();
        collider = target.parent.GetComponent<PolygonCollider2D>();
    }

    void Update()
    {
        spriteRenderer.color = new Color(1, 1, 1, timeToDestroy / duration);

        if (isBreaking)
        {
            BreakingMovement();
        }
        else if (isRespawned)
        {
            timeToDestroy = duration;
        }
    }

    Vector3 finalPos;
    float moveSpeed;

    private void BreakingMovement()
    {
        timeToDestroy -= Time.deltaTime;

        target.localPosition = Vector3.Lerp(target.localPosition, finalPos, moveSpeed);

        if (Vector3.Distance(target.localPosition, finalPos) < 0.05f || finalPos == null)
        {
            finalPos = new Vector3(Random.Range(-0.2f, 0.2f), Random.Range(-0.2f, 0.2f), 0);
            moveSpeed = Random.Range(0.01f, 0.03f);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer == 6 && isRespawned && !isBreaking)
        {
            StartCoroutine(Break());
        }
    }

    private IEnumerator Break()
    {
        timeToDestroy = duration;
        isBreaking = true;

        yield return new WaitForSeconds(duration);
        collider.isTrigger = true;
        isBreaking = false;
        isRespawned = false;

        yield return new WaitForSeconds(respawnDelay);
        collider.isTrigger = false;
        target.localPosition = new Vector3(0, 0, 0);
        isRespawned = true;
    }
}
