using UnityEngine;
using UnityEngine.EventSystems;

public class TouchField : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    private int _pointerId = -1;
    private bool _isPressed;
    private Vector2 _previousTouchPosition;
    private Vector2 _touchDistance;

    public Vector2 TouchDistance => _touchDistance;

    private void Update()
    {
        if (_isPressed)
            HandleTouchInput();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isPressed = true;
        _pointerId = eventData.pointerId;
        _previousTouchPosition = NormalizeTouchPosition(eventData.position);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.pointerId == _pointerId)
        {
            ResetTouchState();
        }
    }

    private void HandleTouchInput()
    {
        Vector2 currentPosition;
        if (_pointerId >= 0 && _pointerId < Input.touches.Length)
        {
            currentPosition = Input.touches[_pointerId].position;
        }
        else
        {
            currentPosition = (Vector2)Input.mousePosition;
        }
        _touchDistance = NormalizeTouchPosition(currentPosition) - _previousTouchPosition;
        _previousTouchPosition = NormalizeTouchPosition(currentPosition);
    }

    private Vector2 NormalizeTouchPosition(Vector2 position)
    {
        float normalizedX = position.x / Screen.width;
        float normalizedY = position.y / Screen.height;

        return new Vector2(normalizedX, normalizedY);
    }

    private void ResetTouchState()
    {
        _isPressed = false;
        _pointerId = -1;
        _touchDistance = Vector2.zero;
    }
}