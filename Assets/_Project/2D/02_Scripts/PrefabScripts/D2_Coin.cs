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


    // ���� Coin ���� Eat ��ü�� ���� ��ũ��Ʈ�� ����ƴ� �κ�
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log("Coin Trigger Enter");
        if (collision.transform.CompareTag("Bullet"))
        {
            //gameManager.CollectCoin();

            // �̽��� ���� �ӽ� ����
            //D2_GameManager.Instance.GetComponent<D2_SoundManager>().PlaySFX(D2_SoundManager.Instance.audios[(int)GameSound.COIN_SOUND], 0.5f);
            D2_GameManager.Instance.CollectCoin();
            Destroy(gameObject);

            // ���� �Դ� �Ҹ� ����
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
