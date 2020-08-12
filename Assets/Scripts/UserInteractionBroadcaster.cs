using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UserInteractionBroadcaster : MonoBehaviour
{
    [Tooltip("GameOjects that have RaycastReceiver component.\nRaycastBroadcaster broadcasts raycast event to them.")]
    public List<UserInteractionReceiver> receivers = new List<UserInteractionReceiver>();
    public string attribute = "";

    private EventTrigger eventTrigger = null;

    void Start()
    {
        this.eventTrigger = this.GetComponent<EventTrigger>();
        if (this.eventTrigger == null)
        {
            this.eventTrigger = this.gameObject.AddComponent<EventTrigger>();
        }
        this.DelegatePointerDown();
    }

    void DelegatePointerDown()
    {
        if (!this.eventTrigger)
        {
            return;
        }

        // pointer down
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerDown;
            entry.callback.AddListener((data) => { this.OnPointerDown((PointerEventData)data); });
            this.eventTrigger.triggers.Add(entry);
        }

        // pointer up
        {
            EventTrigger.Entry entry = new EventTrigger.Entry();
            entry.eventID = EventTriggerType.PointerUp;
            entry.callback.AddListener((data) => { this.OnPointerUp((PointerEventData)data); });
            this.eventTrigger.triggers.Add(entry);
        }
    }


    public void OnPointerDown(PointerEventData data)
    {
        for (int i = 0; i < this.receivers.Count; i++)
        {
            if (this.receivers[i])
            {
                this.receivers[i].OnPointerDown(data, this.attribute);
            }
        }
    }

    public void OnPointerUp(PointerEventData data)
    {
        for (int i = 0; i < this.receivers.Count; i++)
        {
            if (this.receivers[i])
            {
                this.receivers[i].OnPointerUp(data, this.attribute);
            }
        }
    }
}
