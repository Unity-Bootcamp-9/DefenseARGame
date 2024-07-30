using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManagerEx
{
    private ObjectSpawner _objectSpawner;
    private GameObject _map;

    public void Init()
    {
        _objectSpawner = Utils.GetOrAddComponent<ObjectSpawner>(Managers.Resource.Instantiate("MAP/Object Spawner"));

        if (_objectSpawner)
        {
            _objectSpawner.objectPrefabs = new List<GameObject>()
            {
                Managers.Resource.Load<GameObject>("Prefabs/MAP/MapPreview")
            };

            _objectSpawner.GetComponent<ARInteractorSpawnTrigger>().arInteractor = Object.FindObjectOfType<XRRayInteractor>();
        }
    }

    public bool ReadyGame()
    {
        if (!_objectSpawner) return false;

        _objectSpawner.gameObject.SetActive(true);

        Managers.Resource.Load<Material>("Materials/M_Plane").color = new Color(1f, 1f, 0f, 0.05f);

        return true;
    }

    public bool StartGame()
    {
        if (_objectSpawner.transform.childCount == 0) return false;

        Transform mapPreview = _objectSpawner.transform.GetChild(0);

        _map = Managers.Resource.Instantiate("MAP/FinalMap");
        _map.transform.SetPositionAndRotation(mapPreview.position, mapPreview.rotation);
        _map.transform.localScale = mapPreview.localScale;

        Object.Destroy(mapPreview.gameObject);
        _objectSpawner.gameObject.SetActive(false);

        return true;
    }

    public bool FinishGame()
    {
        if (!_map) return false;
        Object.Destroy(_map);
        return true;
    }
}
