using UnityEngine;

public interface IPaintable
{
    void Initialize();
    void Paint(PaintBrush paintBrush, Vector3 position);
}