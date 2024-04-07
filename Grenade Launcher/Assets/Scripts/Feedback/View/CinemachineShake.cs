using Cinemachine;
using UnityEngine;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineShake : MonoBehaviour
{
    public static CinemachineShake Instance { get; private set;}

    private CinemachineVirtualCamera _cinemachine;
    private CinemachineBasicMultiChannelPerlin _cinemachineBasicMultiChannel;
    private float _shakeTimer;
    private float _shakeTimerTotal;
    private float _startingIntensity;

    private void Awake()
    {
        Instance = this;
        _cinemachine = GetComponent<CinemachineVirtualCamera>();

        _cinemachineBasicMultiChannel =
            _cinemachine.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void ShakeCamera(float intesity, float time)
    {
        _cinemachineBasicMultiChannel.m_AmplitudeGain = intesity;

        _startingIntensity = intesity;
        _shakeTimerTotal = time;
        _shakeTimer = time;
    }

    private void Update()
    {
        if(_shakeTimer > 0)
        {
            _shakeTimer -= Time.deltaTime;

            _cinemachineBasicMultiChannel.m_AmplitudeGain = Mathf.Lerp(_startingIntensity, 0f, 1 - _shakeTimer / _shakeTimerTotal);
        }
    }
}