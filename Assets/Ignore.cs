using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Rendering.PostProcessing;

public class Ignore : MonoBehaviour
{
    public GameObject player;
    public TextMeshProUGUI myTextMeshPro;
    public GameObject targetObject;
    public GameObject targetObject2;
    public AudioSource audioSource;

    public PostProcessVolume volume;
    private DepthOfField dof;  // Made private since it's initialized in Start

    public Camera camera;
    public float Speed;
    public List<GameObject> objectsToCheck;
    private List<Renderer> renderers;

    // Start is called before the first frame update
    void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player");
            if (player == null)
            {
                Debug.LogError("Player not found");
                return;
            }
        }

        if (volume.profile.TryGetSettings(out dof))
        {
            dof.active = false;  // Start with DoF disabled
        }

        if (camera == null)
        {
            camera = Camera.main;
            if (camera == null)
            {
                Debug.LogError("Main Camera not found.");
                return;
            }
        }

        renderers = new List<Renderer>();

        foreach (GameObject obj in objectsToCheck)
        {
            Renderer[] objRenderers = obj.GetComponentsInChildren<Renderer>();
            if (objRenderers != null)
            {
                renderers.AddRange(objRenderers);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (camera != null && renderers != null)
        {
            Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
            Bounds combinedBounds = new Bounds();

            if (renderers.Count > 0)
            {
                combinedBounds = renderers[0].bounds; // Start with the first renderer's bounds

                foreach (Renderer objRenderer in renderers)
                {
                    combinedBounds.Encapsulate(objRenderer.bounds); // Continuously update combined bounds to include all objects
                }
            }

            bool isVisible = GeometryUtility.TestPlanesAABB(planes, combinedBounds);

            if (audioSource != null)
            {
                if (!isVisible && audioSource.isPlaying)
                {
                    myTextMeshPro.enabled = true;
                    targetObject.SetActive(true);
                    targetObject2.SetActive(true);
                    audioSource.Pause();
                    dof.active = true;
                }
                else if (isVisible && !audioSource.isPlaying)
                {
                    myTextMeshPro.enabled = false;
                    targetObject.SetActive(false);
                    targetObject2.SetActive(false);
                    audioSource.UnPause();
                    dof.active = false;
                }
            }
        }
    }
}
