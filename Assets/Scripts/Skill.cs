using DTT.AreaOfEffectRegions;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Skill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("스킬 정보")]
    public string skillName = "메테오";
    [Range(1, 10)] public int requireMana = 4;
    [Range(1, 100)] public int damage = 50;
    [Range(0.1f, 15f)] public float radius = 9f;
    [Range(1f, 5f)] public float duration = 5f;
    private bool _isAiming;
    private bool _isRayHit;

    [Header("테두리 UI")]
    public Color activeColor = Color.white;
    private Color _readyColor = Color.clear;
    private Image _edgeImage;

    [Header("아이콘")]
    public Sprite iconSprite;
    private Image _iconImage;

    [Header("오브젝트")]
    public GameObject objectToSpawn;
    public GameObject effectToSpawn;
    private GameObject _draggingObject;
    public Vector3 offset;
    public LayerMask groundLayer = 1 << 8; // Ground 레이어
    private SRPCircleRegionProjector _circleRegion;

    private void Start()
    {
        _edgeImage = GetComponent<Image>();
        _edgeImage.color = _readyColor;

        _iconImage = transform.GetChild(0).GetComponent<Image>();
        if (_iconImage && iconSprite)
        {
            _iconImage.material = Instantiate(_iconImage.material);
            _iconImage.sprite = iconSprite;
        }

        _draggingObject = Instantiate(objectToSpawn);
        _draggingObject.SetActive(false);

        _circleRegion = _draggingObject.GetComponent<SRPCircleRegionProjector>();
        _circleRegion.Radius = radius;
    }

    /// <summary>
    /// 아이콘을 현재 마나에 따라 흑백으로 전환
    /// </summary>
    public void ChangeColor()
    {
        _iconImage.material.SetFloat("_Grayscale",
            GameManager.CurrentMana >= requireMana ? 0 : 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (GameManager.CurrentMana < requireMana) return;

        _edgeImage.color = activeColor;

        if (!objectToSpawn)
        {
            Debug.Log("드래그 중");
            return;
        }

        _isAiming = true;
    }

    public void OnDrag(PointerEventData data)
    {
        if (_draggingObject != null && _isAiming)
            SetDraggedPosition(data);
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        _isRayHit = Physics.Raycast(Camera.main.ScreenPointToRay(data.position), out RaycastHit hit, 100f, groundLayer);

        if (_isRayHit)
        {
            _draggingObject.SetActive(true);
            _draggingObject.transform.SetPositionAndRotation(hit.point + offset, hit.transform.rotation);
        }

        _circleRegion.FillProgress = Convert.ToInt32(_isRayHit);
        _circleRegion.GenerateProjector();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isAiming) return;

        _edgeImage.color = _readyColor;

        if (_draggingObject != null) _draggingObject.SetActive(false);

        if (_isRayHit)
        {
            SpawnEffect();
        }
        else
        {
            Debug.Log("스킬 취소");
        }

        if (_circleRegion.FillProgress == 0)
        {
            Debug.Log("스킬 취소");
        }
        else
        {
            Debug.Log("스킬 발동");
        }

        _isAiming = false;
    }

    public void SpawnEffect()
    {
        ParticleSystem effect = Instantiate
            (
                effectToSpawn,
                _draggingObject.transform.position - offset,
                _draggingObject.transform.rotation
            ).GetComponent<ParticleSystem>();

        Destroy(effect.gameObject, effect ? effect.main.duration : duration);
    }
}