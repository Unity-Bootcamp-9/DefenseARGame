using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
using UnityEngine.XR.Interaction.Toolkit.Samples.ARStarterAssets;
using UnityEngine.XR.Interaction.Toolkit.Samples.StarterAssets;

public class GameManagerEx
{
    private ObjectSpawner _objectSpawner;
    private GameObject _map;

    public PlayData PlayData { get; set; }
    public ARSession AR { get; private set; }

    public void Init()
    {
        _objectSpawner = Utils.GetOrAddComponent<ObjectSpawner>(Managers.Resource.Instantiate("MAP/Object Spawner"));
        AR = Object.FindObjectOfType<ARSession>();

        if (_objectSpawner)
        {
            _objectSpawner.objectPrefabs = new List<GameObject>()
            {
                Managers.Resource.Load<GameObject>("Prefabs/MAP/MapPreview")
            };

            _objectSpawner.GetComponent<ARInteractorSpawnTrigger>().arInteractor = Object.FindObjectOfType<XRRayInteractor>();
            AR.enabled = false;
        }
    }

    public bool ReadyGame()
    {
        if (!_objectSpawner) return false;

        AR.enabled = true;

        _objectSpawner.gameObject.SetActive(true);

        Managers.Resource.Load<Material>("Materials/M_Plane").color = new Color(1f, 1f, 0f, 0.05f);

        return true;
    }

    public bool StartGame()
    {
        if (_objectSpawner.transform.childCount == 0) return false;

        Transform mapPreview = _objectSpawner.transform.GetChild(0);

        if (_map)
        {
            NavMeshAgent[] minions = _map.GetComponentsInChildren<NavMeshAgent>();

            foreach (NavMeshAgent minion in minions)
            {
                minion.enabled = false;
            }

            _map.transform.SetPositionAndRotation(mapPreview.position, mapPreview.rotation);
            _map.transform.localScale = mapPreview.localScale;
            _map.transform.Rotate(Vector3.up, -45f);

            foreach (NavMeshAgent minion in minions)
            {
                minion.enabled = true;
            }

            _map.SetActive(true);
        }
        else
        {
            _map = Managers.Resource.Instantiate("MAP/FinalMap");

            _map.transform.SetPositionAndRotation(mapPreview.position, mapPreview.rotation);
            _map.transform.localScale = mapPreview.localScale;
            _map.transform.Rotate(Vector3.up, -45f);

            PlayData = new PlayData(10, 0, 0);
        }

        Managers.Resource.Load<Material>("Materials/M_Plane").color = Color.clear;

        Object.Destroy(mapPreview.gameObject);
        _objectSpawner.gameObject.SetActive(false);

        return true;
    }

    public bool FinishGame()
    {
        if (!_map) return false;
        AR.Reset();
        AR.enabled = false;
        Object.Destroy(_map);
        return true;
    }

    public bool PauseGame()
    {
        if (!_map || !_objectSpawner) return false;

        AR.Reset();

        _map.SetActive(false);
        _objectSpawner.gameObject.SetActive(true);

        Managers.Resource.Load<Material>("Materials/M_Plane").color = new Color(1f, 1f, 0f, 0.05f);

        return true;
    }
}
