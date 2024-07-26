using DTT.AreaOfEffectRegions;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Skill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private GameManager gm;

    [Header("UI")]
    public Sprite iconSprite;
    public Color activeColor = Color.white;
    private Color _inactiveColor = Color.clear;
    private Image _baseImage;
    private Image _iconImage;

    [Header("프로젝터")]
    public GameObject objectToSpawn;
    protected GameObject draggingObject;
    public Vector3 offset;
    public LayerMask groundLayer = 1 << 8; // Ground 레이어
    protected SRPCircleRegionProjector Circle;

    [Header("스킬 정보")]
    public string skillName = "메테오";
    [Range(1, 10)] public int requireMana = 4;
    [Range(1, 100)] public int damage = 10;
    [Range(0.1f, 1f)] public float radius = 0.09f;
    [Range(1f, 5f), Tooltip("스킬이 피해를 가하는 지속 시간")]
    private bool _isAiming;
    private bool _isAble;

    public void Init(GameManager gm)
    {
        this.gm = gm;

        gm.ManaChanged += ChangeColor;
    }

    private void Awake()
    {
        _baseImage = GetComponent<Image>();
        _baseImage.color = _inactiveColor;

        _iconImage = transform.GetChild(0).GetComponent<Image>();
        if (_iconImage && iconSprite)
        {
            _iconImage.material = Instantiate(_iconImage.material);
            _iconImage.sprite = iconSprite;
        }

        draggingObject = Instantiate(objectToSpawn);
        draggingObject.SetActive(false);

        Circle = draggingObject.GetComponent<SRPCircleRegionProjector>();
        Circle.Radius = radius; // 생성된 맵의 사이즈에 따라 재조정 필요
    }

    private void OnDestroy()
    {
        gm.ManaChanged -= ChangeColor;
    }

    /// <summary>
    /// 아이콘을 현재 마나에 따라 흑백으로 전환
    /// </summary>
    public void ChangeColor()
    {
        _iconImage.material.SetFloat("_Grayscale",
            gm.CurrentMana >= requireMana ? 0 : 1);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (gm.CurrentMana < requireMana) return;

        _baseImage.color = activeColor;

        if (!objectToSpawn)
        {
            Debug.Log("드래그 중");
            return;
        }

        _isAiming = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingObject != null && _isAiming)
        {
            _isAble = Physics.Raycast(Camera.main.ScreenPointToRay(eventData.position), out RaycastHit hit, 100f, groundLayer)
                && !RectTransformUtility.RectangleContainsScreenPoint(_iconImage.rectTransform, eventData.position);

            if (_isAble)
            {
                draggingObject.SetActive(true);
                draggingObject.transform.SetPositionAndRotation(hit.point + offset, hit.transform.rotation);
            }

            Circle.FillProgress = Convert.ToInt32(_isAble);
            Circle.GenerateProjector();
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!_isAiming) return;

        _baseImage.color = _inactiveColor;

        if (draggingObject != null) draggingObject.SetActive(false);

        if (_isAble)
        {
            Activate();
            gm.DecreaseMana(requireMana);
        }
        else
        {
            Debug.Log("스킬 취소");
        }

        _isAiming = false;
    }

    protected virtual void Activate()
    {
        Debug.Log("스킬 발동");
    }
}