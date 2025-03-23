using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class Backpack : MonoBehaviour
{
    [SerializeField] private UnityEvent addEvent;
    [SerializeField] private UnityEvent removeEvent;

    private List<DraggableItem> storedItems = new List<DraggableItem>();

    [SerializeField] private Transform flaskPosition;
    [SerializeField] private Transform sleepingBagPosition;
    [SerializeField] private Transform matchboxPosition;

    public Transform AddItem(DraggableItem item)
    {
        storedItems.Add(item);
        //StartCoroutine(SendPostRequest(item.identifier.ToString(), "Add Item", item.itemType.ToString()));
        addEvent.Invoke();

        switch (item.itemType)
        {
            case DraggableItem.ItemType.flask:
                return flaskPosition;

            case DraggableItem.ItemType.sleepingBag:
                return flaskPosition;

            case DraggableItem.ItemType.matchBox:
                return flaskPosition;

            default:
                Debug.Log("Unknown Item");
                return null;
        }
    }

    public void RemoveItem(DraggableItem item)
    {
        storedItems.Remove(item);
        StartCoroutine(SendPostRequest(item.identifier.ToString(), "Remove Item", item.itemType.ToString()));
        removeEvent.Invoke();
    }

    private IEnumerator SendPostRequest(string itemId, string action, string contentType)
    {
        string url = "https://wadahub.manerai.com/api/inventory/status";
        string json = "{\"itemId\":\"" + itemId + "\", \"action\":\"" + action + "\"}";

        using (UnityWebRequest www = UnityWebRequest.Post(url, json))
        {
            www.SetRequestHeader("Authorization", "Bearer kPERnYcWAY46xaSy8CEzanosAgsWM84Nx7SKM4QBSqPq6c7StWfGxzhxPfDh8MaP");
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Success: " + www.downloadHandler.text);
            }
            else
            {
                Debug.Log("Error: " + www.error);
            }
        }
    }

}
