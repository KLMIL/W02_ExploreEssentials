using DataTypes;
using UnityEngine;

public class D2_Coin : MonoBehaviour
{
    Rigidbody2D rb;
    SpriteRenderer spriteRenderer;

    float changeTime;

    D2_GameManager gameManager;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameManager = D2_GameManager.Instance;
    }

    private void Update()
    {
        if (rb.linearVelocity != Vector2.zero)
        {
            rb.linearVelocity -= rb.linearVelocity * Time.deltaTime; ;
        }
    }


    // 원래 Coin 밑의 Eat 객체에 따로 스크립트로 선언됐던 부분
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Coin Trigger Enter");
        if (collision.transform.CompareTag("Bullet"))
        {
            //gameManager.CollectCoin();

            // 이식을 위한 임시 삭제
            //D2_GameManager.Instance.GetComponent<D2_SoundManager>().PlaySFX(D2_SoundManager.Instance.audios[(int)GameSound.COIN_SOUND], 0.5f);
            D2_GameManager.Instance.CollectCoin();
            Destroy(gameObject);

            // 동전 먹는 소리 실행
        }
        if (collision.CompareTag("Wall"))
        {
            //Vector2 dir = (Vector2)rb.linearVelocity.normalized;
            //Vector2 startPos = transform.position;
            //Vector2 targetPos = startPos - dir;

            //transform.position = Vector2.Lerp()

            //collision.position


            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;

        }
    }
}
