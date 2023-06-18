﻿using DG.Tweening;
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
    /// <summary>
    /// đang nhảy
    /// </summary>
    private bool isJumping = false;
    private Vector3 nextPlatformLeft, nextPlatformRight;
    private ManagerVars vars;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private bool isMove = false;
    private Camera mainCamera;

    private void Awake()
    {
        
        vars = ManagerVars.GetManagerVars();
        spriteRenderer = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
        mainCamera = Camera.main;
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
            FollowPlayer();
        }

        if (rb.velocity.y < 0 && IsRayPlatform() == false && GameManager.Instance.IsGameOver == false)
        {
            spriteRenderer.sortingLayerName = "Default";
            GetComponent<BoxCollider2D>().enabled = false;
            GameManager.Instance.IsGameOver = true;
            StartCoroutine(DealyShowGameOverPanel());
        }
        
        if (transform.position.y - Camera.main.transform.position.y < -5 && GameManager.Instance.IsGameOver == false)
        {
            GameManager.Instance.IsGameOver = true;
            StartCoroutine(DealyShowGameOverPanel());
        }
    }
    IEnumerator DealyShowGameOverPanel()
    {
        yield return new WaitForSeconds(1f);

        EventCenter.Broadcast(EventDefine.ShowGameOverPanel);
    }
    private void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
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
        return true;
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

}
