using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class DragManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Backpack backPack;

    [SerializeField] private GameObject canvasObj;
    [SerializeField] private GameObject flaskImg;
    [SerializeField] private GameObject sleepingBagImg;
    [SerializeField] private GameObject matchBoxImg;

    private DraggableItem selectedItem = null;
    public LayerMask backpackLayer;

    private bool isOnCanvas;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                Backpack backpack = hit.collider.GetComponent<Backpack>();
                if (backpack != null)
                {
                    isOnCanvas = true;
                    canvasObj.SetActive(true);
                }
                else
                {
                    DraggableItem item = hit.collider.GetComponent<DraggableItem>();
                    if (item != null)
                    {
                        selectedItem = item;
                        selectedItem.StartDragging(hit.point);
                    }
                }
            }
        }

        if (Input.GetMouseButtonUp(0) ) 
        {
            if (isOnCanvas)
            {
                if (IsMouseOverUI())
                {
                    HandleUIAction();
                }
                canvasObj.SetActive(false);
                isOnCanvas = false;
            }
            else if (selectedItem != null)
            {
                if (IsMouseOverBackpack())
                {
                    Transform plaement;
                    plaement = backPack.AddItem(selectedItem);
                    selectedItem.MoveToBackpack(plaement);
                }
                else
                {
                    selectedItem.StopDragging();
                }

                selectedItem = null;
            }
        }
    }

    private bool IsMouseOverBackpack()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, Mathf.Infinity, backpackLayer);
    }

    private bool IsMouseOverUI()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        return raycastResults.Count > 0;
    }


    private void HandleUIAction()
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        var raycastResults = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, raycastResults);

        foreach (var result in raycastResults)
        {

            if (result.gameObject == flaskImg)
            {
                backPack.RemoveFlask();
            }
            else if (result.gameObject == sleepingBagImg)
            {
                backPack.RemoveSleepingBag();
            }
            else if (result.gameObject == matchBoxImg)
            {
                backPack.RemoveMatchBox();
            }
        }
    }
}
