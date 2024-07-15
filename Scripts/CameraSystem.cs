using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CameraSystem : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera virtualCamera;
    [SerializeField] private float moveSpeed = 20f;
    [SerializeField] private float rotateSpeed = 20f;
    [SerializeField] private float minFollowOffset = 5f;
    [SerializeField] private float maxFollowOffset = 50f;
    [SerializeField] private float zoomSpeed = 10f;

    float targetFOV;
    Vector3 followOffset;

    private void Awake()
    {
        followOffset = virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
    }
    // Update is called once per frame
    void Update()
    {
        HandleMovement();
        HandleRotation();
        Zoom();
    }
    private void HandleMovement()
    {
        Vector3 inputDir = new Vector3();

        if (Input.GetKey(KeyCode.W)) inputDir.z = 1f;
        if (Input.GetKey(KeyCode.S)) inputDir.z = -1f;
        if (Input.GetKey(KeyCode.D)) inputDir.x = 1f;
        if (Input.GetKey(KeyCode.A)) inputDir.x = -1f;

        Vector3 moveDir = transform.forward * inputDir.z + transform.right * inputDir.x;

        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    private void HandleRotation()
    {
        float rotateDir = 0f;
        if (Input.GetKey(KeyCode.Q)) rotateDir += 1f;
        if (Input.GetKey(KeyCode.E)) rotateDir -= 1f;

        transform.eulerAngles += new Vector3(0, rotateDir * rotateSpeed * Time.deltaTime, 0);
    }
    private void Zoom()
    {
        Vector3 zoomDir = followOffset.normalized;

        if(Input.mouseScrollDelta.y > 0)
        {
            followOffset -= zoomDir;
        }
        if (Input.mouseScrollDelta.y < 0)
        {
            followOffset += zoomDir;
        }

        if(followOffset.magnitude < minFollowOffset)
        {
            followOffset = zoomDir * minFollowOffset;
        }
        if (followOffset.magnitude > maxFollowOffset)
        {
            followOffset = zoomDir * maxFollowOffset;
        }

        virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = Vector3.Lerp(virtualCamera.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset, followOffset, Time.deltaTime * zoomSpeed);
    }
}
