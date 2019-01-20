// %BANNER_BEGIN%
// ---------------------------------------------------------------------
// %COPYRIGHT_BEGIN%
//
// Copyright (c) 2018 Magic Leap, Inc. All Rights Reserved.
// Use of this file is governed by the Creator Agreement, located
// here: https://id.magicleap.com/creator-terms
//
// %COPYRIGHT_END%
// ---------------------------------------------------------------------
// %BANNER_END%

#if UNITY_EDITOR || PLATFORM_LUMIN

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.MagicLeap;

namespace MagicLeap
{
    /// <summary>
    /// This represents all the runtime control over meshing component in order to best visualize the
    /// affect changing parameters has over the meshing API.
    /// </summary>
    public class MeshingExample : MonoBehaviour
    {
        #region Private Variables
        [SerializeField, Tooltip("The spatial mapper from which to update mesh params.")]
        private MLSpatialMapper _mlSpatialMapper;

        [SerializeField, Tooltip("Visualizer for the meshing results.")]
        private MeshingVisualizer _meshingVisualizer;

        [SerializeField, Space, Tooltip("Flag specifying if mesh extents are bounded.")]
        private bool _bounded = false;

        private MeshingVisualizer.RenderMode _renderMode = MeshingVisualizer.RenderMode.Occlusion;
        private int _renderModeCount;

        private static readonly Vector3 _boundedExtentsSize = new Vector3(2.0f, 2.0f, 2.0f);
        private static readonly Vector3 _boundlessExtentsSize = new Vector3(10.0f, 10.0f, 10.0f);

        private Camera _camera;
        #endregion

        #region Unity Methods
        /// <summary>
        /// Initializes component data and starts MLInput.
        /// </summary>
        void Awake()
        {
            if (_mlSpatialMapper == null)
            {
                Debug.LogError("Error: MeshingExample._mlSpatialMapper is not set, disabling script.");
                enabled = false;
                return;
            }
            if (_meshingVisualizer == null)
            {
                Debug.LogError("Error: MeshingExample._meshingVisualizer is not set, disabling script.");
                enabled = false;
                return;
            }
         
            _renderModeCount = System.Enum.GetNames(typeof(MeshingVisualizer.RenderMode)).Length;

            _camera = Camera.main;

            MagicLeapDevice.RegisterOnHeadTrackingMapEvent(OnHeadTrackingMapEvent);
        }

        /// <summary>
        /// Set correct render mode for meshing and update meshing settings.
        /// </summary>
        void Start()
        {
            _meshingVisualizer.SetRenderers(_renderMode);

            _mlSpatialMapper.gameObject.transform.position = _camera.gameObject.transform.position;
            _mlSpatialMapper.gameObject.transform.localScale = _bounded ? _boundedExtentsSize : _boundlessExtentsSize;
          
            UpdateStatusText();
        }

        /// <summary>
        /// Update mesh polling center position to camera.
        /// </summary>
        void Update()
        {
            _mlSpatialMapper.gameObject.transform.position = _camera.gameObject.transform.position;
        }

        /// <summary>
        /// Cleans up the component.
        /// </summary>
        void OnDestroy()
        {
            MagicLeapDevice.UnregisterOnHeadTrackingMapEvent(OnHeadTrackingMapEvent);
           
        }
        #endregion

        #region Private Methods
        /// <summary>
        /// Updates examples status text.
        /// </summary>
        private void UpdateStatusText() {
        }
        #endregion

        #region Event Handlers
       
      

        /// <summary>
        /// Handle in charge of refreshing all meshes if map gets lost.
        /// </summary>
        /// <param name="mapEvents"> Map Events that happened. </param>
        private void OnHeadTrackingMapEvent(MLHeadTrackingMapEvent mapEvents)
        {
            if (mapEvents.IsLost())
            {
                _mlSpatialMapper.DestroyAllMeshes();
                _mlSpatialMapper.RefreshAllMeshes();
            }
        }
        #endregion
    }
}

#endif
