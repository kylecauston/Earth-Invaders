using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager instance = null;

    public GameObject cameraRoot;
    public float moveSpeed = 1.0f;
    public float moveRange = 0.1f;
    
    public GameObject lockTarget = null;

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
    }

    public void Update()
    {
        if (lockTarget != null)
        {
            cameraRoot.transform.SetPositionAndRotation(new Vector3(lockTarget.transform.position.x - 40, cameraRoot.transform.position.y, lockTarget.transform.position.z - 40), cameraRoot.transform.rotation);
        }

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

        if (viewportPos.y > (1.0 - moveRange) || viewportPos.y < moveRange)
        {
            if (viewportPos.y < moveRange)
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

    public void LockTo(GameObject go)
    {
        lockTarget = go;
    }
}
