using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class ExtendedButton : Button, IPointerDownHandler, IPointerUpHandler
{
    public event Action OnButtonDownEvent;
    public event Action OnButtonHoldEvent;
    public event Action OnButtonUpEvent;

    private bool _buttonPressed = false;

    public override void OnPointerDown(PointerEventData pointerEventData)
    {
        base.OnPointerDown(pointerEventData);
        _buttonPressed = true;
        OnButtonDownEvent?.Invoke();
    }

    public override void OnPointerUp(PointerEventData pointerEventData)
    {
        base.OnPointerUp(pointerEventData);
        _buttonPressed = false;
        OnButtonUpEvent?.Invoke();
    }

    private void Update()
    {
        if (_buttonPressed)
        {
            OnButtonHoldEvent?.Invoke();
        }
    }
}