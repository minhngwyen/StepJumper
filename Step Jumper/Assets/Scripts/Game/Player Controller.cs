using DG.Tweening;
using DG.Tweening.Core.Easing;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Pool;

public class PlayerController : MonoBehaviour
{
    public Transform rayDown, rayLeft, rayRight;
    public LayerMask platformLayer, obstacleLayer;
    public float cameraFollowSpeed = 0.8f;

    /// <summary>
    /// Di chuyển sang trái hay sang phải
    /// </summary>
    private bool isMoveLeft = false;
    /// <summary>c
    /// đang nhảy
    /// </summary>
    private bool isJumping = false;
    private Vector3 nextPlatformLeft, nextPlatformRight;
    private ManagerVars vars;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isMove = false;
    private AudioSource m_AudioSource;


    private void Awake()
    {
        
        vars = ManagerVars.GetManagerVars();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        m_AudioSource = GetComponent<AudioSource>();
    }
    
  

    
    
    private int count;
    private bool IsPointerOverGameObject(Vector2 mousePosition)
    {
        //Tạo sự kiện nhấp chuột
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        eventData.position = mousePosition;
        List<RaycastResult> raycastResults = new List<RaycastResult>();

        EventSystem.current.RaycastAll(eventData, raycastResults);
        return raycastResults.Count > 0;
    }
    private void Update()
    {
        Debug.DrawRay(rayDown.position, Vector2.down * 1, Color.red);
        Debug.DrawRay(rayLeft.position, Vector2.left * 0.15f, Color.red);
        Debug.DrawRay(rayRight.position, Vector2.right * 0.15f, Color.red);

        if (IsPointerOverGameObject(Input.mousePosition)) return;

       

        if (Input.GetMouseButtonDown(0) && isJumping == false && nextPlatformLeft != Vector3.zero)
        {
            if (isMove == false)
            {
                EventCenter.Broadcast(EventDefine.PlayerMove);
                isMove = true;
            }
            m_AudioSource.PlayOneShot(vars.jumpClip);
            EventCenter.Broadcast(EventDefine.DecidePath);
            isJumping = true;
            Vector3 mousePos = Input.mousePosition;

            if (mousePos.x <= Screen.width / 2)
            {
                isMoveLeft = true;
            }

            else if (mousePos.x > Screen.width / 2)
            {
                isMoveLeft = false;
            }
            Jump();
        }

        if (rb.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.fallClip);
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            StartCoroutine(DealyShowGameOverPanel());
        }
        if (isJumping && IsRayObstacle() && GameManager.Instance.IsGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.hitClip);
            GetDeathEffect();
            GameManager.Instance.IsGameOver = true;
            spriteRenderer.enabled = false;
            StartCoroutine(DealyShowGameOverPanel());
        }

        if (transform.position.y - Camera.main.transform.position.y < -5 && GameManager.Instance.IsGameOver == false)
        {
            m_AudioSource.PlayOneShot(vars.fallClip);
            GameManager.Instance.IsGameOver = true;
            StartCoroutine(DealyShowGameOverPanel());
        }
    }
    IEnumerator DealyShowGameOverPanel()
    {
        yield return new WaitForSeconds(1f);

        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
    }
  

    public void GetDeathEffect()
    {
       
    }
   
    private GameObject lastHitGo = null;
    /// <summary>
    /// </summary>
    /// <returns></returns>
    private bool IsRayPlatform()
    {
        RaycastHit2D hit = Physics2D.Raycast(rayDown.position, Vector2.down, 1f, platformLayer);
        if (hit.collider != null)
        {
            if (hit.collider.tag == "Platform")
            {
                if (lastHitGo != hit.collider.gameObject)
                {
                    if (lastHitGo == null)
                    {
                        lastHitGo = hit.collider.gameObject;
                        return true;
                    }
                    EventCenter.Broadcast(EventDefine.AddScore);
                    lastHitGo = hit.collider.gameObject;
                }
                return true;
            }
        }
        return false;
    }
    /// <summary>
    /// </summary>
    /// <returns></returns>
    private bool IsRayObstacle()
    {
        RaycastHit2D leftHit = Physics2D.Raycast(rayLeft.position, Vector2.left, 0.15f, obstacleLayer);
        RaycastHit2D rightHit = Physics2D.Raycast(rayRight.position, Vector2.right, 0.15f, obstacleLayer);

        if (leftHit.collider != null)
        {
            if (leftHit.collider.tag == "Obstacle")
            {
                return true;
            }
        }

        if (rightHit.collider != null)
        {
            if (rightHit.collider.tag == "Obstacle")
            {
                return true;
            }
        }
        return false;
    }
    private void Jump()
    {
        if (isJumping)
        {
            if (isMoveLeft)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.DOMoveX(nextPlatformLeft.x, 0.2f);
                transform.DOMoveY(nextPlatformLeft.y + 0.8f, 0.15f);
            }
            else
            {
                transform.DOMoveX(nextPlatformRight.x, 0.2f);
                transform.DOMoveY(nextPlatformRight.y + 0.8f, 0.15f);
                transform.localScale = Vector3.one;
            }
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Platform")
        {
            isJumping = false;
            Vector3 currentPlatformPos = collision.gameObject.transform.position;
            nextPlatformLeft = new Vector3(currentPlatformPos.x -
                vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
            nextPlatformRight = new Vector3(currentPlatformPos.x +
                vars.nextXPos, currentPlatformPos.y + vars.nextYPos, 0);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Pickup")
        {
            m_AudioSource.PlayOneShot(vars.diamondClip);
            EventCenter.Broadcast(EventDefine.AddDiamond);

            collision.gameObject.SetActive(false);
        }
    }

}
