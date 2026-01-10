// Copyright 2021, Infima Games. All Rights Reserved.

using UnityEngine;

namespace InfimaGames.LowPolyShooterPack
{
    /// <summary>
    /// Weapon. This class handles most of the things that weapons need.
    /// </summary>
    public class Weapon : WeaponBehaviour
    {
        #region FIELDS SERIALIZED
        
        [Header("Tembakan")]

        [Tooltip("Apakah senjata ini otomatis? Jika ya, menahan tombol tembak akan membuat senjata menembak terus-menerus.")]
        [SerializeField] 
        private bool automatic;
        
        [Tooltip("Seberapa cepat proyektil bergerak.")]
        [SerializeField]
        private float projectileImpulse = 400.0f;

        [Tooltip("Jumlah tembakan yang dapat ditembakkan senjata ini dalam satu menit. Ini menentukan seberapa cepat senjata menembak.")]
        [SerializeField] 
        private int roundsPerMinutes = 200;

        [Tooltip("Mask dari objek/benda yang dikenali saat menembak.")]
        [SerializeField]
        private LayerMask mask;

        [Tooltip("Jarak maksimum di mana senjata ini dapat menembak dengan akurat. Tembakan di luar jarak ini tidak akan menggunakan linetracing untuk akurasi.")]
        [SerializeField]
        private float maximumDistance = 500.0f;

        [Header("Animasi")]

        [Tooltip("Transform yang merepresentasikan port ejeksi senjata, yaitu bagian senjata tempat selongsong peluru dikeluarkan.")]
        [SerializeField]
        private Transform socketEjection;

        [Header("Sumber Daya")]

        [Tooltip("Prefab Selongsong Peluru.")]
        [SerializeField]
        private GameObject prefabCasing;
        
        [Tooltip("Prefab Proyektil. Ini adalah prefab yang muncul saat senjata menembak.")]
        [SerializeField]
        private GameObject prefabProjectile;
        
        [Tooltip("AnimatorController yang perlu digunakan karakter pemain saat memegang senjata ini.")]
        [SerializeField] 
        public RuntimeAnimatorController controller;

        [Tooltip("Tekstur Bodinya Senjata.")]
        [SerializeField]
        private Sprite spriteBody;
        
        [Header("Klip Audio untuk Menyimpan Senjata (Holster).")]

        [Tooltip("Klip Audio Holster (untuk menyimpan senjata).")]
        [SerializeField]
        private AudioClip audioClipHolster;

        [Tooltip("Klip Audio Unholster (untuk mengeluarkan senjata).")]
        [SerializeField]
        private AudioClip audioClipUnholster;
        
        [Header("Klip Audio untuk Isi Ulang Peluru")]

        [Tooltip("Klip Audio untuk isi ulang peluru.")]
        [SerializeField]
        private AudioClip audioClipReload;
        
        [Tooltip("Klip Audio Reload Kosong (untuk mengisi ulang saat magasin/tembakan habis).")]
        [SerializeField]
        private AudioClip audioClipReloadEmpty;
        
        [Header("Klip Audio Lainnya")]

        [Tooltip("Klip audio yang diputar saat senjata ini ditembakkan tanpa amunisi.")]
        [SerializeField]
        private AudioClip audioClipFireEmpty;

        #endregion

        #region FIELDS

        /// <summary>
        /// Weapon Animator.
        /// </summary>
        private Animator animator;
        /// <summary>
        /// Attachment Manager.
        /// </summary>
        private WeaponAttachmentManagerBehaviour attachmentManager;

        /// <summary>
        /// Amount of ammunition left.
        /// </summary>
        private int ammunitionCurrent;

        #region Attachment Behaviours
        
        /// <summary>
        /// Equipped Magazine Reference.
        /// </summary>
        private MagazineBehaviour magazineBehaviour;
        /// <summary>
        /// Equipped Muzzle Reference.
        /// </summary>
        private MuzzleBehaviour muzzleBehaviour;

        #endregion

        /// <summary>
        /// The GameModeService used in this game!
        /// </summary>
        private IGameModeService gameModeService;
        /// <summary>
        /// The main player character behaviour component.
        /// </summary>
        private CharacterBehaviour characterBehaviour;

        /// <summary>
        /// The player character's camera.
        /// </summary>
        private Transform playerCamera;
        
        #endregion

        #region UNITY
        
        protected override void Awake()
        {
            //Get Animator.
            animator = GetComponent<Animator>();
            //Get Attachment Manager.
            attachmentManager = GetComponent<WeaponAttachmentManagerBehaviour>();

            //Cache the game mode service. We only need this right here, but we'll cache it in case we ever need it again.
            gameModeService = ServiceLocator.Current.Get<IGameModeService>();
            //Cache the player character.
            characterBehaviour = gameModeService.GetPlayerCharacter();
            //Cache the world camera. We use this in line traces.
            playerCamera = characterBehaviour.GetCameraWorld().transform;
        }
        protected override void Start()
        {
            #region Cache Attachment References
            
            //Get Magazine.
            magazineBehaviour = attachmentManager.GetEquippedMagazine();
            //Get Muzzle.
            muzzleBehaviour = attachmentManager.GetEquippedMuzzle();

            #endregion

            //Max Out Ammo.
            ammunitionCurrent = magazineBehaviour.GetAmmunitionTotal();
        }

        #endregion

        #region GETTERS

        public override Animator GetAnimator() => animator;
        
        public override Sprite GetSpriteBody() => spriteBody;

        public override AudioClip GetAudioClipHolster() => audioClipHolster;
        public override AudioClip GetAudioClipUnholster() => audioClipUnholster;

        public override AudioClip GetAudioClipReload() => audioClipReload;
        public override AudioClip GetAudioClipReloadEmpty() => audioClipReloadEmpty;

        public override AudioClip GetAudioClipFireEmpty() => audioClipFireEmpty;
        
        public override AudioClip GetAudioClipFire() => muzzleBehaviour.GetAudioClipFire();
        
        public override int GetAmmunitionCurrent() => ammunitionCurrent;

        public override int GetAmmunitionTotal() => magazineBehaviour.GetAmmunitionTotal();

        public override bool IsAutomatic() => automatic;
        public override float GetRateOfFire() => roundsPerMinutes;
        
        public override bool IsFull() => ammunitionCurrent == magazineBehaviour.GetAmmunitionTotal();
        public override bool HasAmmunition() => ammunitionCurrent > 0;

        public override RuntimeAnimatorController GetAnimatorController() => controller;
        public override WeaponAttachmentManagerBehaviour GetAttachmentManager() => attachmentManager;

        #endregion

        #region METHODS

        public override void Reload()
        {
            //Play Reload Animation.
            animator.Play(HasAmmunition() ? "Reload" : "Reload Empty", 0, 0.0f);
        }
        public override void Fire(float spreadMultiplier = 1.0f)
        {
            //We need a muzzle in order to fire this weapon!
            if (muzzleBehaviour == null)
                return;
            
            //Make sure that we have a camera cached, otherwise we don't really have the ability to perform traces.
            if (playerCamera == null)
                return;

            //Get Muzzle Socket. This is the point we fire from.
            Transform muzzleSocket = muzzleBehaviour.GetSocket();
            
            //Play the firing animation.
            const string stateName = "Fire";
            animator.Play(stateName, 0, 0.0f);
            //Reduce ammunition! We just shot, so we need to get rid of one!
            ammunitionCurrent = Mathf.Clamp(ammunitionCurrent - 1, 0, magazineBehaviour.GetAmmunitionTotal());

            //Play all muzzle effects.
            muzzleBehaviour.Effect();
            
            //Determine the rotation that we want to shoot our projectile in.
            Quaternion rotation = Quaternion.LookRotation(playerCamera.forward * 1000.0f - muzzleSocket.position);
            
            //If there's something blocking, then we can aim directly at that thing, which will result in more accurate shooting.
            if (Physics.Raycast(new Ray(playerCamera.position, playerCamera.forward),
                out RaycastHit hit, maximumDistance, mask))
                rotation = Quaternion.LookRotation(hit.point - muzzleSocket.position);
                
            //Spawn projectile from the projectile spawn point.
            GameObject projectile = Instantiate(prefabProjectile, muzzleSocket.position, rotation);
            //Add velocity to the projectile.
            projectile.GetComponent<Rigidbody>().linearVelocity = projectile.transform.forward * projectileImpulse;   
        }

        public override void FillAmmunition(int amount)
        {
            //Update the value by a certain amount.
            ammunitionCurrent = amount != 0 ? Mathf.Clamp(ammunitionCurrent + amount, 
                0, GetAmmunitionTotal()) : magazineBehaviour.GetAmmunitionTotal();
        }

        public override void EjectCasing()
        {
            //Spawn casing prefab at spawn point.
            if(prefabCasing != null && socketEjection != null)
                Instantiate(prefabCasing, socketEjection.position, socketEjection.rotation);
        }

        #endregion
    }
}