using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Skill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("테두리 UI")]
    public Color activeColor = Color.white;
    private Color _readyColor = Color.clear;
    private Image _edgeImage;

    [Header("아이콘")]
    public Sprite iconSprite;
    private Image _iconImage;

    [Header("오브젝트")]
    public GameObject objectToSpawn;
    private GameObject _draggingObject;
    public Vector3 offset;
    public LayerMask groundLayer = 1 << 8; // Ground 레이어

    private void Start()
    {
        _edgeImage = GetComponent<Image>();
        _edgeImage.color = _readyColor;

        _iconImage = transform.GetChild(0).GetComponent<Image>();
        if (_iconImage && iconSprite) _iconImage.sprite = iconSprite;

        _draggingObject = Instantiate(objectToSpawn);
        _draggingObject.SetActive(false);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        _edgeImage.color = activeColor;

        if (!objectToSpawn)
        {
            Debug.Log("드래그 중");
            return;
        }

        SetDraggedPosition(eventData);
    }

    public void OnDrag(PointerEventData data)
    {
        if (_draggingObject != null)
            SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        bool isHit = Physics.Raycast(Camera.main.ScreenPointToRay(data.position), out RaycastHit hit, 100f, groundLayer);

        if (isHit)
        {
            _draggingObject.transform.position = hit.point + offset;
            _draggingObject.transform.rotation = hit.transform.rotation; 
        }

        // TBD-73에서 수정 예정
        _draggingObject.SetActive(isHit);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        _edgeImage.color = _readyColor;

        if (_draggingObject != null)
            _draggingObject.SetActive(false);
    }
}