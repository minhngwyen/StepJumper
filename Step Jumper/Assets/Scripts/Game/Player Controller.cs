using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Transform rayDown, rayLeft, rayRight;
    public LayerMask platformLayer, obstacleLayer;
    public float cameraFollowSpeed = 0.8f;

    private bool isMoveLeft = false;
    private bool isJumping = false;
    private ManagerVars vars;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Camera mainCamera;
    private Vector3 nextPlatformLeft, nextPlatformRight;



    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        vars = ManagerVars.GetManagerVars();
        mainCamera = Camera.main;
    }

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
        // Kiểm tra nút chuột trái được nhấn
        if (Input.GetMouseButtonDown(0) && isJumping == false && nextPlatformLeft != Vector3.zero)
        {

            // Lấy vị trí x của chuột so với màn hình
            float mousePosX = Input.mousePosition.x;

            // Lấy kích thước chiều rộng của màn hình
            float screenWidth = Screen.width;

            // So sánh vị trí x của chuột với giữa màn hình
            if (mousePosX < screenWidth / 2)
            {
                // Nếu vị trí x nhỏ hơn giữa màn hình, quay sang bên trái
                isMoveLeft = true;

            }
            else
            {
                // Ngược lại, quay sang bên phải
                isMoveLeft = false;
            }
                // Thực hiện nhảy
                Jump();
            // Kéo camera theo nhân vật
            FollowPlayer();
        }



     
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
                    lastHitGo = hit.collider.gameObject;
                }
                return true;
            }
        }
        return false;
    }
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

    private void FollowPlayer()
    {
        Vector3 targetPosition = new Vector3(transform.position.x, transform.position.y, mainCamera.transform.position.z);
        mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, targetPosition, cameraFollowSpeed * Time.deltaTime);
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
