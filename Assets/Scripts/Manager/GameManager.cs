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

    #region UI

    public UIManager ui;

    private bool _isStartGame;
    private float _playTimer;

    private void RefreshUI()
    {
        ui.UpdateMana(CurrentMana);
    }

    #endregion

    #region 스킬

    public const int MaxMana = 10;
    public static int CurrentMana { get; private set; }

    private readonly float _manaRegenInterval = 2.0f;
    private float _manaTimer;

    public GameObject[] skillPrefabs;

    private void AddMana()
    {
        _manaTimer += Time.deltaTime;

        if (_manaTimer >= _manaRegenInterval && CurrentMana < MaxMana)
        {
            _manaTimer = 0;
            CurrentMana++;
            RefreshUI();
        }
    }

    public void UseSkill(int index)
    {
        // 스킬
    }

    #endregion

    private void Reset()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
        ui = FindObjectOfType<UIManager>();
    }

    private void Start()
    {
        objectSpawner.objectPrefabs = new List<GameObject>() { mapPreviewPrefab };

        if (skillPrefabs.Length > 0)
        {
            foreach (GameObject obj in skillPrefabs)
            {
                objectSpawner.objectPrefabs.Add(obj);
            }
        }

        ui.ChangeTo(Popup.placeMapPopup);
    }

    private void Update()
    {
        if (!_isStartGame) return;

        _playTimer += Time.deltaTime;

        AddMana();
    }

    /// <summary>
    /// 시작 버튼 누르면 맵 생성 후 게임 시작 (버튼의 인스펙터에 이벤트로 추가하여 사용할 것)
    /// </summary>
    public void StartGame()
    {
        Transform mapPreview = objectSpawner.transform.GetChild(0);

        if (_isStartGame || !mapPreview) return;

        if (mapPrefab)
        {
            GameObject map = Instantiate(mapPrefab, mapPreview.position + Vector3.up * 1e-2f, mapPreview.rotation);
            map.transform.localScale = mapPreview.localScale * 1e-2f;
            map.transform.Rotate(Vector3.up, 45f);
            Destroy(mapPreview.gameObject);
        }

        _isStartGame = true;
        ui.ChangeTo(Popup.inGamePopup);
        RefreshUI();
    }

    public void Quit()
    {
        Application.Quit();
    }
}
