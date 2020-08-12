using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventData
{
    public Vector3 position;
    public PointerEventData data;
    public string tag;

    public EventData(Vector3 p, PointerEventData d, string t)
    {
        this.position = p;
        this.data = d;
        this.tag = t;
    }
}

public class UserInteractionController : UserInteractionReceiver
{
    public enum InteractionEvent
    {
        None = 0,
        Down = 1,
        Hold = 1 << 1,
    }

    // add accessor if needed
    public Dictionary<string, System.Action<EventData>> pointerDownCallbackMap = new Dictionary<string, System.Action<EventData>>();
    public Dictionary<string, System.Action<EventData>> pointerHoldCallbackMap = new Dictionary<string, System.Action<EventData>>();

    private UserInteractionDevice device = null;
    private Dictionary<InteractionEvent, List<EventData>> triggeredEvents = new Dictionary<InteractionEvent, List<EventData>>();

    public void Trigger(InteractionEvent type, Vector3 position, PointerEventData data, string tag)
    {
        List<EventData> events;
        if (!this.triggeredEvents.TryGetValue(type, out events))
        {
            events = new List<EventData>();
            this.triggeredEvents.Add(type, events);
        }
        events.Add(new EventData(position, data, tag));
        this.triggeredEvents[type] = events;
    }
    public bool IsTriggered(InteractionEvent type)
    {
        List<EventData> events;
        if (!this.triggeredEvents.TryGetValue(type, out events))
        {
            return false;
        }

        return events.Count > 0;
    }
    public void EachTriggeredEvents(InteractionEvent type, System.Action<EventData> action)
    {
        List<EventData> events;
        if (!this.triggeredEvents.TryGetValue(type, out events))
        {
            return;
        }
        for (int i = 0; i < events.Count; i++)
        {
            action(events[i]);
        }
    }
    public void ClearTrigger()
    {
        this.triggeredEvents = new Dictionary<InteractionEvent, List<EventData>>();
    }

    private void Start()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.Android:
            case RuntimePlatform.IPhonePlayer:
                {
                    this.device = new UserInteractionDeviceTouch();
                    break;
                }
            default:
                {
                    this.device = new UserInteractionDeviceMouse();
                    break;
                }
        }
    }

    private void Update()
    {
        if (this.device.IsInteracting() && !this.IsTriggered(InteractionEvent.Down))
        {
            this.Trigger(InteractionEvent.Hold, this.device.InteractingPosition(), null, "");
        }

        this.EachTriggeredEvents(InteractionEvent.Down, (EventData data) =>
        {
            foreach (KeyValuePair<string, System.Action<EventData>> kv in this.pointerDownCallbackMap)
            {
                kv.Value(data);
            }
        });
        this.EachTriggeredEvents(InteractionEvent.Hold, (EventData data) =>
        {
            foreach (KeyValuePair<string, System.Action<EventData>> kv in this.pointerHoldCallbackMap)
            {
                kv.Value(data);
            }
        });

        this.ClearTrigger();
    }

    override public void OnPointerDown(PointerEventData data, string tag)
    {
        Vector3 pointerPosition = this.device.InteractingPosition();
        this.Trigger(InteractionEvent.Down, pointerPosition, data,  tag);
    }

}

interface UserInteractionDevice
{
    bool IsInteracting();
    Vector3 InteractingPosition();
    List<Vector3> InteractingPositions();
}

public class UserInteractionDeviceTouch : UserInteractionDevice
{
    public bool IsInteracting()
    {
        Touch[] touches = Input.touches;
        return touches.Length > 0;
    }

    public Vector3 InteractingPosition()
    {
        Touch[] touches = Input.touches;
        if (touches.Length == 0)
        {
            return Vector3.zero;
        }
        Touch touch = Input.GetTouch(0);

        return new Vector3(touch.position.x, touch.position.y, 0);
    }
    public List<Vector3> InteractingPositions()
    {
        List<Vector3>  list = new List<Vector3>();
        Touch[] touches = Input.touches;
        if (touches.Length == 0)
        {
            return list;
        }

        for (int i = 0; i < touches.Length; i++)
        {
            Touch touch = Input.GetTouch(i);
            list.Add(new Vector3(touch.position.x, touch.position.y, 0));
        }

        return list;
    }
}

public class UserInteractionDeviceMouse : UserInteractionDevice
{
    public bool IsInteracting()
    {
        return !!Input.GetMouseButton(0);
    }

    public Vector3 InteractingPosition()
    {
        return Input.mousePosition;
    }
    public List<Vector3> InteractingPositions()
    {
        return new List<Vector3>()
        {
            Input.mousePosition
        };
    }
}