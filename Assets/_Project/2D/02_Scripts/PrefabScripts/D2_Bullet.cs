using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using DataTypes;

public class D2_Bullet : MonoBehaviour
{
    D2_PlayerManager playerManager;
    D2_PlayerController playercontroller;
    

    Rigidbody2D rb;
    LineRenderer lineRenderer;

    public List<GameObject> itemParticles;

    bool isStarted = false;
    //bool isDragging = false;
    Vector2 releasePosition;

    public bool isDestroyed = false; // 필요없나?

    private bool isMouseOnBullet = false;


    private void Start()
    {
        playerManager = D2_PlayerManager.Instance;
        playercontroller = D2_PlayerController.Instance;

        rb = gameObject.GetComponent<Rigidbody2D>();
        lineRenderer = gameObject.GetComponent<LineRenderer>();


        _InitLineRenderer();
    }


    private void FixedUpdate()
    {
        // PlayerManager에 정의된 값만큼 속도 감소. 일정 속도 이하에서 완전 정지로 변경
        rb.linearVelocity *= playerManager.dampingAmount;
        if (rb.linearVelocity.magnitude < playerManager.stopSpeed)
        {
            rb.linearVelocity = Vector2.zero;
            // 멈추고 난 뒤 제거함수 호출
            if (isStarted)
            {
                playerManager.DestroyBullet();
                enabled = false;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
            //Debug.Log("충돌한 오브젝트 태그: " + collision.gameObject.tag);


            //if (collision.gameObject.CompareTag("BackGround")
            //    || collision.gameObject.CompareTag("Block"))
        if (collision.gameObject.CompareTag("Wall"))
        {
            // 이식을 위한 임시삭제
            //D2_GameManager.Instance.GetComponent<D2_SoundManager>().PlaySFX(D2_SoundManager.Instance.audios[(int)GameSound.BOUNCE_BALL]);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // 딱히 뭐 안함. 나중에 코인 먹는거 여기에 구현하자. 
    }


    private void OnMouseEnter()
    {
        if (!isMouseOnBullet)
        {
            playerManager.SetDragAvailable(true);
            isMouseOnBullet = true;
        }
    }

    private void OnMouseExit()
    {
        if (isMouseOnBullet)
        {
            playerManager.SetDragAvailable(false);
            isMouseOnBullet = false;
        }
    }


    // 라인렌더러 초기화
    private void _InitLineRenderer()
    {
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.red;
        lineRenderer.endColor = Color.white;
        lineRenderer.positionCount = 2; // 이건 머지
        lineRenderer.enabled = false;
    }

    public void SetLineRenderer(Vector2 start, Vector2 end)
    {
        //Debug.Log($"Line Renderer Start, End: {start}, {end}");
        lineRenderer.SetPosition(0, start);
        lineRenderer.SetPosition(1, end);
    }

    public void DisableLineRenderer()
    {
        lineRenderer.enabled = false;
    }

    public void MakeParticle(int index)
    {
        Instantiate(itemParticles[index], transform.position, itemParticles[index].transform.rotation);
    }



    public void Fire()
    {
        isStarted = true;
    }

    public bool GetFired()
    {
        return isStarted;
    }
}
