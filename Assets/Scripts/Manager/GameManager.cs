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

    private void Reset()
    {
        objectSpawner = FindObjectOfType<ObjectSpawner>();
    }

    private void Start()
    {
        objectSpawner.objectPrefabs = new List<GameObject>() { mapPreviewPrefab };
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
        }
    }
}
