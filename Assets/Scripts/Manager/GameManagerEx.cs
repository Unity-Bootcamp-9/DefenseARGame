using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManagerEx
{
    private ObjectSpawner _objectSpawner;

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

    public void StartGame()
    {

        Transform mapPreview = _objectSpawner.transform.GetChild(0);

        if (!mapPreview) return;

        GameObject map = Managers.Resource.Instantiate("MAP/ProtoMap");
        map.transform.SetPositionAndRotation(mapPreview.position, mapPreview.rotation);
        map.transform.localScale = mapPreview.localScale * 1e-2f;

        Object.Destroy(mapPreview.gameObject);
        _objectSpawner.gameObject.SetActive(false);
    }
}
