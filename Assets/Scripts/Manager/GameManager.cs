using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManager : MonoBehaviour
{
    [Tooltip("맵 미리보기 UI 프리팹")]
    public GameObject mapPreviewPrefab;

    [Tooltip("게임 시작 후 사용할 맵 프리팹")]
    public GameObject mapPrefab;

    [Tooltip("미리보기를 생성할 오브젝트 스포너")]
    public ObjectSpawner objectSpawner;

    [Tooltip("게임 시작 여부")]
    public bool isPlaying;

    #region 스킬

    public const int MaxMana = 10;
    public int CurrentMana { get; private set; }

    private readonly float _manaRegenInterval = 2.0f;
    private float _manaTimer;

    private Skill[] skills;

    public event Action ManaChanged;

    private void AddObserverOfMana()
    {
        foreach (var skill in skills)
        {
            if (!skill) return;
            skill.Init(this);
        }

        ManaChanged += () =>
        {
            Debug.Log($"Current Mana : {CurrentMana}/{MaxMana}");
        };

        ManaChanged?.Invoke();
    }

    private void RemoveObserverOfMana()
    {
        foreach (var skill in skills)
        {
            ManaChanged -= skill.ChangeColor;
            skill.ChangeColor();
        }
    }

    private void IncreaseMana()
    {
        _manaTimer += Time.deltaTime;

        if (_manaTimer >= _manaRegenInterval && CurrentMana < MaxMana)
        {
            _manaTimer = 0;
            CurrentMana++;
            ManaChanged?.Invoke();
        }
    }

    public void DecreaseMana(int value)
    {
        CurrentMana -= value;
        ManaChanged?.Invoke();
    }

    #endregion

    private void Start()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        skills = FindObjectsOfType<Skill>();

        AddObserverOfMana();
    }

    public void InitGame()
    {
        RemoveObserverOfMana();
    }

    private void Update()
    {
        if (isPlaying)
        {
            IncreaseMana(); 
        }
    }

    public void StartGame()
    {
        Transform mapPreview = objectSpawner.transform.GetChild(0);

        if (!mapPreview) return;

        if (mapPrefab)
        {
            GameObject map = Instantiate(mapPrefab, mapPreview.position, mapPreview.rotation);
            map.transform.localScale = mapPreview.localScale * 1e-2f;
            map.transform.Rotate(Vector3.up, -45f);
            Destroy(objectSpawner);

            isPlaying = true;
        }
    }
}
