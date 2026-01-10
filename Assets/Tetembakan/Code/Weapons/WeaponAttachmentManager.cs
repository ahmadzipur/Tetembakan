// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon Attachment Manager. Handles equipping and storing a Weapon's Attachments.
    /// </summary>
    public class WeaponAttachmentManager : WeaponAttachmentManagerBehaviour
    {
        #region FIELDS SERIALIZED

        [Header("Teropong")]

        [Tooltip("Menentukan apakah iron sights harus ditampilkan pada model senjata.")]
        [SerializeField]
        private bool scopeDefaultShow = true;
        
        [Tooltip("Teropong bawaan!")]
        [SerializeField]
        private ScopeBehaviour scopeDefaultBehaviour;
        
        [Header("Muzzle")]

        [Tooltip("Indeks Muzzle yang dipilih.")]
        [SerializeField]
        private int muzzleIndex;

        [Tooltip("Semua kemungkinan attachment muzzle yang dapat digunakan oleh senjata ini!")]
        [SerializeField]
        private MuzzleBehaviour[] muzzleArray;
        
        [Header("Magazine")]

        [Tooltip("Indeks Magazine yang dipilih.")]
        [SerializeField]
        private int magazineIndex;

        [Tooltip("Semua kemungkinan attachment magazin yang dapat digunakan oleh senjata ini!")]
        [SerializeField]
        private Magazine[] magazineArray;

        #endregion

        #region FIELDS

        /// <summary>
        /// Equipped Scope.
        /// </summary>
        private ScopeBehaviour scopeBehaviour;
        /// <summary>
        /// Equipped Muzzle.
        /// </summary>
        private MuzzleBehaviour muzzleBehaviour;
        /// <summary>
        /// Equipped Magazine.
        /// </summary>
        private MagazineBehaviour magazineBehaviour;

        #endregion

        #region UNITY FUNCTIONS

        /// <summary>
        /// Awake.
        /// </summary>
        protected override void Awake()
        {
            //Check if we have no scope. This could happen if we have an incorrect index.
            if (scopeBehaviour == null)
            {
                //Select Default Scope.
                scopeBehaviour = scopeDefaultBehaviour;
                //Set Active.
                scopeBehaviour.gameObject.SetActive(scopeDefaultShow);
            }
            
            //Select Muzzle!
            muzzleBehaviour = muzzleArray.SelectAndSetActive(muzzleIndex);

            //Select Magazine!
            magazineBehaviour = magazineArray.SelectAndSetActive(magazineIndex);
        }        

        #endregion

        #region GETTERS

        public override ScopeBehaviour GetEquippedScope() => scopeBehaviour;
        public override ScopeBehaviour GetEquippedScopeDefault() => scopeDefaultBehaviour;

        public override MagazineBehaviour GetEquippedMagazine() => magazineBehaviour;
        public override MuzzleBehaviour GetEquippedMuzzle() => muzzleBehaviour;

        #endregion
    }
}