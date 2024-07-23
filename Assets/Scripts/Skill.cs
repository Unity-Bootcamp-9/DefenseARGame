using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Skill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public GameObject objectToSpawn;
    public Sprite iconSprite;
    public Vector3 offset;

    private GameObject _draggingObject;
    private LayerMask groundLayer = 1 << 8; // Ground 레이어

    private void Start()
    {
        if (iconSprite) GetComponent<Image>().sprite = iconSprite;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!objectToSpawn)
        {
            Debug.Log("드래그 중");
            return;
        }

        _draggingObject = Instantiate(objectToSpawn);

        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData data)
    {
        if (_draggingObject != null)
            SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        var target = _draggingObject.transform;

        if (Physics.Raycast(Camera.main.ScreenPointToRay(data.position), out RaycastHit hit, 100f, groundLayer))
        {
            target.position = hit.point + offset;
            target.rotation = hit.transform.rotation;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (_draggingObject != null)
            Destroy(_draggingObject);
    }
}