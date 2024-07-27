using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManagerEx
{
    [Tooltip("미리보기를 생성할 오브젝트 스포너")]
    public ObjectSpawner objectSpawner;

    [Tooltip("게임 시작 여부")]
    public bool isPlaying;

    public GameManagerEx()
    {
        objectSpawner = Utils.GetOrAddComponent<ObjectSpawner>(new GameObject("Object Spawner"));
        if (objectSpawner) objectSpawner.objectPrefabs
                = new List<GameObject>() { Managers.Resource.Load<GameObject>("Prefabs/MAP/MapPreview") };
    }

    public void StartGame()
    {

        Transform mapPreview = objectSpawner.transform.GetChild(0);

        if (!mapPreview) return;

        // TODO : Resource Manager 써야 함
        GameObject map = Managers.Resource.Instantiate("MAP/ProtoMap");
        map.transform.SetPositionAndRotation(mapPreview.position, mapPreview.rotation);
        map.transform.localScale = mapPreview.localScale;
        map.transform.Rotate(Vector3.up, -45f);

        // TODO : Resource Manager 써야 함.
        //objectSpawner.gameObject.SetActive = false;

        isPlaying = true;
    }
}
