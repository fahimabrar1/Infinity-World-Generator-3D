using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraZoom : MonoBehaviour
{
    [SerializeField, Range(0, 10f)] private float defaultCameraDistance = 6f;
    [SerializeField, Range(0, 10f)] private float minimumDistance = 1f;
    [SerializeField, Range(0, 10f)] private float maximumDistance = 6f;
    [SerializeField, Range(0, 10f)] private float smothing = 4f;
    [SerializeField, Range(0, 10f)] private float zoomSensitivity = 1f;

    private CinemachineFramingTransposer framingTransposer;
    private CinemachineInputProvider inputProvider;

    private float currenTargetDistance;

    /// <summary>
    /// Awake is called when the script instance is being loaded.
    /// </summary>
    void Awake()
    {
        framingTransposer = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineFramingTransposer>();
        inputProvider = GetComponent<CinemachineInputProvider>();
        currenTargetDistance = defaultCameraDistance;
    }



    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        Zoom();
    }

    private void Zoom()
    {
        float zoomValue = inputProvider.GetAxisValue(2) * zoomSensitivity;
        Debug.Log($"Zoom Value: {zoomValue}");
        currenTargetDistance = Mathf.Clamp(currenTargetDistance + zoomValue, minimumDistance, maximumDistance);

        float currentDistance = framingTransposer.m_CameraDistance;
        if (currentDistance == currenTargetDistance) return;

        float lerpedZoomValue = Mathf.Lerp(currentDistance, currenTargetDistance, smothing * Time.deltaTime);
        framingTransposer.m_CameraDistance = lerpedZoomValue;
    }
}
