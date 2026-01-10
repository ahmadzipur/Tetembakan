// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Crosshair.
    /// </summary>
    public class Crosshair : Element
    {
        #region FIELDS SERIALIZED

        [Header("Pengaturan")]
        
        [Tooltip("Transisi perubahan visibilitas yang halus.")]
        [SerializeField]
        private float smoothing = 8.0f;

        [Tooltip("Skala minimum yang diperlukan Crosshair agar tetap terlihat. Berguna untuk menghindari gambar yang terlalu kecil dan terlihat aneh.")]
        [SerializeField]
        private float minimumScale = 0.15f;

        #endregion

        #region FIELDS
        
        /// <summary>
        /// Current.
        /// </summary>
        private float current = 1.0f;
        /// <summary>
        /// Target.
        /// </summary>
        private float target = 1.0f;

        /// <summary>
        /// Rect.
        /// </summary>
        private RectTransform rectTransform;

        #endregion
        
        #region UNITY
        
        protected override void Awake()
        {
            //Base.
            base.Awake();

            //Cache Rect Transform.
            rectTransform = GetComponent<RectTransform>();
        }

        #endregion
        
        #region METHODS
        
        protected override void Tick()
        {
            //Check Visibility.
            bool visible = playerCharacter.IsCrosshairVisible();
            //Update Target.
            target = visible ? 1.0f : 0.0f;

            //Interpolate Current.
            current = Mathf.Lerp(current, target, Time.deltaTime * smoothing);
            //Scale.
            rectTransform.localScale = Vector3.one * current;
            
            //Hide Crosshair Objects When Too Small.
            for (var i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(current > minimumScale);
        }
        
        #endregion
    }
}