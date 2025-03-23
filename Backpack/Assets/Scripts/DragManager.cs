using UnityEngine;

public class DragManager : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Backpack backPack;
    private DraggableItem selectedItem = null;
    public LayerMask backpackLayer;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit))
            {
                DraggableItem item = hit.collider.GetComponent<DraggableItem>();
                if (item != null)
                {
                    selectedItem = item;
                    selectedItem.StartDragging(hit.point);
                }
            }
        }

        if (Input.GetMouseButtonUp(0) && selectedItem != null) 
        {
            if (IsMouseOverBackpack()) 
            {
                Transform plaement;
                plaement = backPack.AddItem(selectedItem);
                selectedItem.StopDragging();
                selectedItem.MoveToBackpack(plaement);
            }
            else
            {
                selectedItem.StopDragging();
            }

            selectedItem = null;
        }
    }

    private bool IsMouseOverBackpack()
    {
        RaycastHit hit;
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

        return Physics.Raycast(ray, out hit, Mathf.Infinity, backpackLayer);
    }
}
