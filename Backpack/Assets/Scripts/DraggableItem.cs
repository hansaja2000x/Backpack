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
    
    [SerializeField] private float yThreshold;

    private Rigidbody rb;
    private MeshCollider coll;
    private bool isDragging = false;
    private Vector3 offset;
    
    private Plane dragPlane;

    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        coll = GetComponent<MeshCollider>();
    }

    public void StartDragging(Vector3 hitPoint)
    {
        isDragging = true;
        //coll.enabled = false;
        rb.useGravity = false;
        rb.constraints = RigidbodyConstraints.FreezeRotation;
        rb.velocity = Vector3.zero;
        offset = transform.position - hitPoint;

        // Plane parallel to camera XY plane
        dragPlane = new Plane(Camera.main.transform.forward, transform.position);
    }

    public void StopDragging()
    {
        rb.constraints = RigidbodyConstraints.None;
        isDragging = false;
        rb.useGravity = true;
        coll.enabled = true;
    }

    void Update()
    {
        if (isDragging)
        {
            Vector3 newPos = GetMouseWorldPosition();
            if (newPos.y > yThreshold)
            {
                rb.MovePosition(newPos);
            }
            else
            {
                Vector3 nextPosition = new Vector3(newPos.x, yThreshold, newPos.z);
                rb.MovePosition(nextPosition);
            }
        }
    }

    private Vector3 GetMouseWorldPosition()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float distance;

        if (dragPlane.Raycast(ray, out distance))
        {
            return ray.GetPoint(distance) + offset;
        }

        return transform.position;
    }



    #region Movements
    /// <summary>
    /// Handle movements when adding item from backpack.
    /// </summary>
    public void MoveToBackpack(Transform placement)
    {
        isDragging = false;
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = Vector3.zero;
        coll.enabled = false;
        StartCoroutine(SmoothMoveTo(placement));
        StartCoroutine(SmoothRotateTo(placement));
    }

    public void MoveToBackpackInstant(Transform placement) // for saved games
    {
        isDragging = false;
        rb.useGravity = false;
        rb.isKinematic = true;
        rb.constraints = RigidbodyConstraints.None;
        rb.velocity = Vector3.zero;
        coll.enabled = false;
        transform.position = placement.position;
        transform.rotation = placement.rotation;
    }

    private IEnumerator SmoothMoveTo(Transform placement)
    {      
        while (Vector3.Distance(transform.position, placement.position) > 0.01f) 
        {
            transform.position = Vector3.Lerp(transform.position, placement.position, moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = placement.position;
    }

    private IEnumerator SmoothRotateTo(Transform placement)
    {    
        while (Quaternion.Angle(transform.rotation, placement.rotation) > 1f)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, placement.rotation, moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.rotation = placement.rotation;
    }

    /// <summary>
    /// Handle movements when removing item from backpack.
    /// </summary>
    public void RemoveFromBackPack(Transform placement)
    {
        StopAllCoroutines();
        StartCoroutine(SmoothMoveToOutSide(placement));
    }

    private IEnumerator SmoothMoveToOutSide(Transform placement)
    {
        while (Vector3.Distance(transform.position, placement.position) > 0.01f)
        {
            transform.position = Vector3.Lerp(transform.position, placement.position, moveSpeed * Time.deltaTime);

            yield return null;
        }

        transform.position = placement.position;
        coll.enabled = true;
        rb.useGravity = true;
        rb.isKinematic = false;
    }

    #endregion
}
