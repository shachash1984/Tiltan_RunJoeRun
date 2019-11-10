using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MovementButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private float axisDirection;
    bool isPressed = false;
        
    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        StartCoroutine(MoveInHorizontalAxis());
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        StopCoroutine(MoveInHorizontalAxis());
    }

    IEnumerator MoveInHorizontalAxis()
    {
        while (isPressed)
        {            
            Player.S.MoveSideways(axisDirection);
            Player.S.axisDirection = this.axisDirection;
            yield return new WaitForFixedUpdate();
        }
        Player.S.axisDirection = 0;
    }
}
