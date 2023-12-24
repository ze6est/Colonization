using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ClickTracker : MonoBehaviour, IPointerClickHandler
{
    public event UnityAction ClickHappened;

    public void OnPointerClick(PointerEventData eventData)
    {        
        ClickHappened?.Invoke();
    }
}