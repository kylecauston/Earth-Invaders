using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;
    private GameObject cameraRoot;

    public float moveSpeed = 1.0f;
    public float moveRange = 0.1f;
    public float bottomOffset = 0;
    
    public GameObject lockTarget = null;

    public EventSystem eventSystem;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

        cameraRoot = Camera.main.transform.parent.gameObject;
    }

    public void Update()
    {
        if (lockTarget != null)
        {
            cameraRoot.transform.SetPositionAndRotation(new Vector3(lockTarget.transform.position.x - 40, cameraRoot.transform.position.y, lockTarget.transform.position.z - 40), cameraRoot.transform.rotation);
        }

        if (eventSystem.IsPointerOverGameObject())
            return;

        Vector3 viewportPos = Camera.main.ScreenToViewportPoint(Input.mousePosition);
        if (viewportPos.x > 1 || viewportPos.y > 1 || viewportPos.x < 0 || viewportPos.y < 0)
            return;

        if (viewportPos.x > (1.0 - moveRange) || viewportPos.x < moveRange)
        {
            if (viewportPos.x < moveRange)
            {
                cameraRoot.transform.Translate(-moveSpeed, 0, moveSpeed);
            }
            else
            {
                cameraRoot.transform.Translate(moveSpeed, 0, -moveSpeed);
            }

            LockTo(null);
        }

        if (viewportPos.y > (1.0 - moveRange) || viewportPos.y < moveRange + bottomOffset)
        {
            if (viewportPos.y < moveRange  + bottomOffset)
            {
                cameraRoot.transform.Translate(-moveSpeed, 0, -moveSpeed);
            }
            else
            {
                cameraRoot.transform.Translate(moveSpeed, 0, moveSpeed);
            }

            LockTo(null);
        }

    }

    public void LockTo(Entity e)
    {
        UIManager.instance.HideInteractionPane();
        if (e)
            lockTarget = e.gameObject;
        else
            lockTarget = null;
    }
}
