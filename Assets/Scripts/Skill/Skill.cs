using DTT.AreaOfEffectRegions;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Skill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Mana _mana;
    public Mana mana
    {
        get => _mana;
        set
        {
            if (!_mana) _mana = value;
        }
    }

    [Header("UI")]
    public Sprite iconSprite;
    public Color activeColor = Color.white;
    private Color _inactiveColor = Color.clear;
    private Image _baseImage;
    private Image _iconImage;

    [Header("프로젝터")]
    public Vector3 offset = Vector3.up * 9f;
    public LayerMask groundLayer = 1 << 8; // Ground 레이어
    protected GameObject draggingObject;
    protected SRPCircleRegionProjector Circle;

    [Header("스킬 정보")]
    public string skillName = "Meteor";
    [Range(1, 10)] public int requireMana = 4;
    [Range(1, 100)] public int damage = 10;
    [Range(1f, 15f)] public float radius = 9f;
    private bool _isAiming;
    private bool _isAble;

    private void Awake()
    {
        _baseImage = GetComponent<Image>();
        _baseImage.color = _inactiveColor;

        _iconImage = transform.GetChild(0).GetComponent<Image>();
        if (_iconImage && iconSprite)
        {
            _iconImage.material = Instantiate(_iconImage.material);
            _iconImage.sprite = Managers.Resource.Load<Sprite>($"Sprites/{skillName}");
        }

        draggingObject = Managers.Resource.Instantiate($"Skill/Circle 1 Region");
        draggingObject.SetActive(false);

        Circle = draggingObject.GetComponent<SRPCircleRegionProjector>();
        Circle.Radius = radius;
    }

    private void OnDestroy()
    {
        _mana.ManaChanged -= ChangeColor;
    }

    /// <summary>
    /// 아이콘을 현재 마나에 따라 흑백으로 전환
    /// </summary>
    public void ChangeColor() =>
        _iconImage.material.SetFloat("_Grayscale", Convert.ToSingle(_mana.CurrentMana < requireMana));

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_mana.CurrentMana < requireMana) return;

        _baseImage.color = activeColor;

        _isAiming = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingObject != null && _isAiming)
        {
            _isAble = Physics.Raycast(Camera.main.ScreenPointToRay(eventData.position), out RaycastHit hit, 1000f, groundLayer)
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
            _mana.UpdateMana(-requireMana);
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