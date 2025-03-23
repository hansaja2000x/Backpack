using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.Networking;
using TMPro;

public class Backpack : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnityEvent addEvent;
    [SerializeField] private UnityEvent removeEvent;

    [Header("Plaement Transforms")]
    [SerializeField] private Transform flaskPosition;
    [SerializeField] private Transform sleepingBagPosition;
    [SerializeField] private Transform matchBoxPosition;

    [SerializeField] private Transform flaskRemovePosition;
    [SerializeField] private Transform sleepingBagRemovePosition;
    [SerializeField] private Transform matchBoxRemovePosition;

    [SerializeField] private GameObject backpackParent;
    [SerializeField] private GameObject outsideParent;

    [Header("GUI")]
    [SerializeField] private Image flaskImage;
    [SerializeField] private Image sleepingBagImage;
    [SerializeField] private Image matchBoxImage;

    [SerializeField] private TextMeshProUGUI weightText;

    private float weight;

    private List<DraggableItem> storedItems = new List<DraggableItem>();

    #region Add Items
    public Transform AddItem(DraggableItem item)
    {
        weight = weight + item.weight;
        UpdateTextUI();
        storedItems.Add(item);
        SetParentWithoutMoving(item.gameObject.transform, backpackParent.transform);
       // StartCoroutine(SendPostRequest(item.identifier.ToString(), "Add Item", item.itemType.ToString()));
        addEvent.Invoke();

        switch (item.itemType)
        {
            case DraggableItem.ItemType.flask:
                Color newColorFlask = flaskImage.color;
                newColorFlask.a = 1;
                flaskImage.color = newColorFlask;
                return flaskPosition;

            case DraggableItem.ItemType.sleepingBag:
                Color newColorBag = sleepingBagImage.color;
                newColorBag.a = 1;
                sleepingBagImage.color = newColorBag;
                return sleepingBagPosition;

            case DraggableItem.ItemType.matchBox:
                Color newColorBox = matchBoxImage.color;
                newColorBox.a = 1;
                matchBoxImage.color = newColorBox;
                return matchBoxPosition;

            default:
                Debug.Log("Unknown Item");
                return null;
        }
        

    }

    #endregion

    #region Remove Items
    public void RemoveFlask()
    {
        DraggableItem removeItem = storedItems.Find(item => item.itemType == DraggableItem.ItemType.flask);

        if (removeItem != null)
        {
            RemoveItem(removeItem);
        }
    }

    public void RemoveSleepingBag()
    {
        DraggableItem removeItem = storedItems.Find(item => item.itemType == DraggableItem.ItemType.sleepingBag);

        if (removeItem != null)
        {
            RemoveItem(removeItem);
        }
    }

    public void RemoveMatchBox()
    {
        DraggableItem removeItem = storedItems.Find(item => item.itemType == DraggableItem.ItemType.matchBox);

        if (removeItem != null)
        {
            RemoveItem(removeItem);
        }
    }

    public void RemoveItem(DraggableItem item)
    {
        weight = weight - item.weight;
        UpdateTextUI();
        storedItems.Remove(item);
        SetParentWithoutMoving(item.gameObject.transform, outsideParent.transform);
       // StartCoroutine(SendPostRequest(item.identifier.ToString(), "Remove Item", item.itemType.ToString()));
        removeEvent.Invoke();
        Transform removePosition;
        switch (item.itemType)
        {
            case DraggableItem.ItemType.flask:
                Color newColorFlask = flaskImage.color;
                newColorFlask.a = 0.5f;
                flaskImage.color = newColorFlask;
                removePosition = flaskRemovePosition;
                break;

            case DraggableItem.ItemType.sleepingBag:
                Color newColorBag = sleepingBagImage.color;
                newColorBag.a = 0.5f;
                sleepingBagImage.color = newColorBag;
                removePosition = sleepingBagRemovePosition;
                break;

            case DraggableItem.ItemType.matchBox:
                Color newColorBox = matchBoxImage.color;
                newColorBox.a = 0.5f;
                matchBoxImage.color = newColorBox;
                removePosition = matchBoxRemovePosition;
                break;

            default:
                Debug.Log("Unknown Item");
                removePosition = null;
                break ;
        }
        item.RemoveFromBackPack(removePosition);
    }

    #endregion

    /// <summary>
    /// Handle POST method.
    /// </summary>
    private IEnumerator SendPostRequest(string itemId, string action, string contentType)
    {
        string url = "https://wadahub.manerai.com/api/inventory/status";
        string json = "{\"itemId\":\"" + itemId + "\", \"action\":\"" + action + "\"}";

        using (UnityWebRequest www = UnityWebRequest.PostWwwForm(url, json))
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

    private void SetParentWithoutMoving(Transform child, Transform newParent)
    {
        child.SetParent(newParent, true);
    }

    private void UpdateTextUI()
    {
        weightText.text = weight.ToString("F0");
    }
}
