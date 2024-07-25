using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManagerEx : MonoBehaviour
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
    public static int CurrentMana { get; private set; }

    private readonly float _manaRegenInterval = 2.0f;
    private float _manaTimer;

    [Tooltip("스킬 아이콘")]
    public Skill[] skills = new Skill[4];

    public event Action ManaChanged;

    private void AddObserverOfMana()
    {
        foreach (var skill in skills)
        {
            ManaChanged += skill.ChangeColor;
            skill.ChangeColor();
        }

        ManaChanged += () =>
        {
            Debug.Log($"Current Mana : {CurrentMana}/{MaxMana}");
        };
    }

    private void RemoveObserverOfMana()
    {
        foreach (var skill in skills)
        {
            ManaChanged += skill.ChangeColor;
            skill.ChangeColor();
        }
    }

    private void AddMana()
    {
        _manaTimer += Time.deltaTime;

        if (_manaTimer >= _manaRegenInterval && CurrentMana < MaxMana)
        {
            _manaTimer = 0;
            CurrentMana++;
            ManaChanged?.Invoke();
        }
    }

    #endregion

    private void Reset()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        skills = FindObjectsOfType<Skill>();
    }

    private void Start()
    {
        
    }

    private void OnDisable()
    {
        RemoveObserverOfMana();
    }

    private void Update()
    {
        if (isPlaying)
        {
            AddMana();
        }
    }

    public void StartGame()
    {
        if (objectSpawner) objectSpawner.objectPrefabs
                = new List<GameObject>() { mapPreviewPrefab };

        AddObserverOfMana();

        //
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