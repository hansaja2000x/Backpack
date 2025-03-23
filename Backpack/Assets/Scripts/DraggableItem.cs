using UnityEngine;
using System.Collections;

public class DraggableItem : MonoBehaviour
{
    public enum ItemType
    {
        flask,
        sleepingBag,
        matchBox
    }

    [Header("Inventory Details")]
    public string itemName;
    public int identifier; // 0 = flask, 1 = sleepng bag, 2 = match box
    public ItemType itemType;
    public float weight;

    [Header("Movements")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private GameObject backpackParent;
    [SerializeField] private GameObject outsideParent;

    private Rigidbody rb;
    private bool isDragging = false;
    private Vector3 offset;
    
    private Plane dragPlane;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void StartDragging(Vector3 hitPoint)
    {
        isDragging = true;
        rb.useGravity = false;
        rb.velocity = Vector3.zero;
        offset = transform.position - hitPoint;

        // Plane parallel to camera's XY plane
        dragPlane = new Plane(Camera.main.transform.forward, transform.position);
    }

    public void StopDragging()
    {
        isDragging = false;
        rb.useGravity = true;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPos = GetMouseWorldPosition();
            rb.MovePosition(newPos);
        }
    }

    Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (dragPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance) + offset;
        }

        return transform.position;
    }

    public void MoveToBackpack(Transform placement)
    {
        SetParentWithoutMoving(this.transform, backpackParent.transform);
        StartCoroutine(SmoothMoveToBackpack(placement));
        StartCoroutine(SmoothRotateToBackpack(placement));
    }

    private void SetParentWithoutMoving(Transform child, Transform newParent)
    {
        child.SetParent(newParent, true); 
    }

    private IEnumerator SmoothMoveToBackpack(Transform placement)
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        while (Vector3.Distance(transform.position, placement.position) > 0.01f) 
        {
            transform.position = Vector3.Lerp(transform.position, placement.position, moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = placement.position;
    }

    private IEnumerator SmoothRotateToBackpack(Transform placement)
    {
        rb.useGravity = false;
        rb.velocity = Vector3.zero;

        while (Quaternion.Angle(transform.rotation, placement.rotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, placement.rotation, moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.rotation = placement.rotation;
    }
}
