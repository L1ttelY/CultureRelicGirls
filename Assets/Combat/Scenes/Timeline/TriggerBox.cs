using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TriggerBox : MonoBehaviour
{
    bool activated;
    [SerializeField] UnityEvent onActivation;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (activated) return;
        if (!collision.gameObject.GetComponent<Combat.EntityFriendly>()) return;
        onActivation.Invoke();
        activated = true;
    }
}
