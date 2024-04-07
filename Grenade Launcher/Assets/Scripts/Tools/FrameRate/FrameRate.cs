using UnityEngine;

public class FrameRate : MonoBehaviour
{
    [SerializeField] [Range(1, 300)] private int _targetFrameRate = 60;

    private void Start()
    {
        Application.targetFrameRate = _targetFrameRate;
    }
}