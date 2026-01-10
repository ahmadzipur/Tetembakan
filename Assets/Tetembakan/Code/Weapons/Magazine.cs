// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Magazine.
    /// </summary>
    public class Magazine : MagazineBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Pengaturan")]
        
        [Tooltip("Total Amunisi.")]
        [SerializeField]
        private int ammunitionTotal = 10;

        [Header("Antarmuka")]

        [Tooltip("Gambar Antarmuka.")]
        [SerializeField]
        private Sprite sprite;

        #endregion

        #region GETTERS

        /// <summary>
        /// Ammunition Total.
        /// </summary>
        public override int GetAmmunitionTotal() => ammunitionTotal;
        /// <summary>
        /// Sprite.
        /// </summary>
        public override Sprite GetSprite() => sprite;

        #endregion
    }
}