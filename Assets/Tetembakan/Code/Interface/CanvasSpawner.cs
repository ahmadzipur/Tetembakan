// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Player Interface.
    /// </summary>
    public class CanvasSpawner : MonoBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Pengaturan")]
        
        [Tooltip("Prefab Canvas dibuat (di-spawn) saat awal. Menampilkan antarmuka pengguna (UI) pemain.")]
        [SerializeField]
        private GameObject canvasPrefab;

        #endregion

        #region UNITY FUNCTIONS

        /// <summary>
        /// Awake.
        /// </summary>
        private void Awake()
        {
            //Spawn Interface.
            Instantiate(canvasPrefab);
        }

        #endregion
    }
}