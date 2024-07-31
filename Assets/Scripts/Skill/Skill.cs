using DTT.AreaOfEffectRegions;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class Skill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Mana Mana { get; set; }

    public string SkillName { get; protected set; }
    public string SkillName_KR { get; protected set; }
    public string Description { get; protected set; }
    public int RequireMana { get; protected set; }
    public int Damage { get; protected set; }
    public float Radius { get; protected set; }
    private bool _isAiming;
    private bool _isAble;

    private Color _activeColor = Color.white;
    private Color _inactiveColor = Color.clear;
    private Image _baseImage;
    private Image _iconImage;

    protected GameObject draggingObject;
    protected GameObject effectToSpawn;
    protected SRPCircleRegionProjector Circle;
    protected Vector3 offset = Vector3.up * 9f;
    private LayerMask _groundLayer = 1 << 8 | 1 << 9; // Ground 레이어

    public virtual void Init()
    {
        _baseImage = GetComponent<Image>();
        _baseImage.color = _inactiveColor;

        _iconImage = transform.GetChild(0).GetComponent<Image>();
        if (_iconImage)
        {
            _iconImage.material = Instantiate(_iconImage.material);
            _iconImage.sprite = Managers.Resource.Load<Sprite>($"Sprites/{SkillName}");
        }

        draggingObject = Managers.Resource.Instantiate($"Skill/Circle 1 Region");
        draggingObject.SetActive(false);

        effectToSpawn = Managers.Resource.Load<GameObject>($"Prefabs/Skill/{SkillName}");

        Circle = draggingObject.GetComponent<SRPCircleRegionProjector>();
        Circle.Radius = Radius;
    }

    private void Awake()
    {
        Init();
    }

    private void OnDestroy()
    {
        Mana.ManaChanged -= ChangeColor;
        Destroy(draggingObject);
    }

    /// <summary>
    /// 아이콘을 현재 마나에 따라 흑백으로 전환
    /// </summary>
    public void ChangeColor() =>
        _iconImage.material.SetFloat("_Grayscale", Convert.ToSingle(Mana.CurrentMana < RequireMana));

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Mana.CurrentMana < RequireMana) return;

        _baseImage.color = _activeColor;

        _isAiming = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingObject != null && _isAiming)
        {
            _isAble = Physics.Raycast(Camera.main.ScreenPointToRay(eventData.position), out RaycastHit hit, 1000f, _groundLayer)
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
            Mana.UpdateMana(-RequireMana);
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