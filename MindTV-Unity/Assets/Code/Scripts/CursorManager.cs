using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorManager : MonoBehaviour
{
    private Vector3 mousePointerHotSpot = new Vector3(5, 0, 0);
    public Texture2D mousePointerTexture;

    private Vector3 handPointerHotSpot = new Vector3(5, 0, 0);
    public Texture2D handPointerTexture;

    public void OnCursorEnter()
    {
        Cursor.SetCursor(handPointerTexture, handPointerHotSpot, CursorMode.Auto);
    }

    public void OnCursorExit()
    {
        Cursor.SetCursor(mousePointerTexture, mousePointerHotSpot, CursorMode.Auto);
    }
}