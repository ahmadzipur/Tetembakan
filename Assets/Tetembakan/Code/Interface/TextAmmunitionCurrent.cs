// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;
using System.Globalization;

namespace InfimaGames.LowPolyShooterPack.Interface
{
    /// <summary>
    /// Current Ammunition Text.
    /// </summary>
    public class TextAmmunitionCurrent : ElementText
    {
        #region FIELDS SERIALIZED
        
        [Header("Warna")]
        
        [Tooltip("Menentukan apakah warna teks akan berubah saat amunisi ditembakkan.")]
        [SerializeField]
        private bool updateColor = true;
        
        [Tooltip("Menentukan seberapa cepat warna berubah saat amunisi ditembakkan.")]
        [SerializeField]
        private float emptySpeed = 1.5f;
        
        [Tooltip("Warna yang digunakan pada teks ini ketika karakter pemain tidak memiliki amunisi.")]
        [SerializeField]
        private Color emptyColor = Color.red;
        
        #endregion
        
        #region METHODS
        
        /// <summary>
        /// Tick.
        /// </summary>
        protected override void Tick()
        {
            //Current Ammunition.
            float current = equippedWeapon.GetAmmunitionCurrent();
            //Total Ammunition.
            float total = equippedWeapon.GetAmmunitionTotal();
            
            //Update Text.
            textMesh.text = current.ToString(CultureInfo.InvariantCulture);

            //Determine if we should update the text's color.
            if (updateColor)
            {
                //Calculate Color Alpha. Helpful to make the text color change based on count.
                float colorAlpha = (current / total) * emptySpeed;
                //Lerp Color. This makes sure that the text color changes based on count.
                textMesh.color = Color.Lerp(emptyColor, Color.white, colorAlpha);   
            }
        }
        
        #endregion
    }
}