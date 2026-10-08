using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARTrackedImageManager))]
public class ImagenMultiple : MonoBehaviour
{
    [System.Serializable]
    public struct ImagenPrefab
    {
        public string nombreImagen;
        public GameObject prefab;
    }

    public ImagenPrefab[] imagenes;

    ARTrackedImageManager manager;
    Dictionary<string, GameObject> instancias = new Dictionary<string, GameObject>();

    void Awake()
    {
        manager = GetComponent<ARTrackedImageManager>();
    }

    void OnEnable()
    {
        manager.trackablesChanged.AddListener(OnChanged);
    }

    void OnDisable()
    {
        manager.trackablesChanged.RemoveListener(OnChanged);
    }

    void OnChanged(ARTrackablesChangedEventArgs<ARTrackedImage> args)
    {
        foreach (var img in args.added) Actualizar(img);
        foreach (var img in args.updated) Actualizar(img);
    }

    void Actualizar(ARTrackedImage img)
    {
        string nombre = img.referenceImage.name;

        if (!instancias.ContainsKey(nombre))
        {
            foreach (var par in imagenes)
            {
                if (par.nombreImagen == nombre)
                {
                    var obj = Instantiate(par.prefab, img.transform);
                    instancias[nombre] = obj;
                }
            }
        }

        if (instancias.TryGetValue(nombre, out var go))
            go.SetActive(img.trackingState == TrackingState.Tracking);
    }
}
