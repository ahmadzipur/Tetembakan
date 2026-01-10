using UnityEngine;
using System.Collections;

public class CasingScript : MonoBehaviour {

	[Header("Force X")]
	[Tooltip("Gaya minimum pada sumbu X")]
	public float minimumXForce;		
	[Tooltip("Gaya maksimum pada sumbu X")]
	public float maximumXForce;
	[Header("Force Y")]
	[Tooltip("Gaya minimum pada sumbu Y")]
	public float minimumYForce;
	[Tooltip("Gaya maksimum pada sumbu Y")]
	public float maximumYForce;
	[Header("Force Z")]
	[Tooltip("Gaya minimum pada sumbu Z")]
	public float minimumZForce;
	[Tooltip("Gaya Maksimum pada sumbu Z")]
	public float maximumZForce;
	[Header("Rotation Force")]
	[Tooltip("Nilai rotasi awal minimum")]
	public float minimumRotation;
	[Tooltip("Nilai rotasi awal maksimum")]
	public float maximumRotation;
	[Header("Waktu Despawn")]
	[Tooltip("Durasi selongsong bertahan setelah di-spawn sebelum dihancurkan.")]
	public float despawnTime;

	[Header("Audio")]
	public AudioClip[] casingSounds;
	public AudioSource audioSource;

	[Header("Pengaturan Putaran")]
	//How fast the casing spins
	[Tooltip("Kecepatan rotasi selongsong.")]
	public float speed = 2500.0f;

	//Launch the casing at start
	private void Awake () 
	{
		//Random rotation of the casing
		GetComponent<Rigidbody>().AddRelativeTorque (
			Random.Range(minimumRotation, maximumRotation), //X Axis
			Random.Range(minimumRotation, maximumRotation), //Y Axis
			Random.Range(minimumRotation, maximumRotation)  //Z Axis
			* Time.deltaTime);

		//Random direction the casing will be ejected in
		GetComponent<Rigidbody>().AddRelativeForce (
			Random.Range (minimumXForce, maximumXForce),  //X Axis
			Random.Range (minimumYForce, maximumYForce),  //Y Axis
			Random.Range (minimumZForce, maximumZForce)); //Z Axis		     
	}

	private void Start () 
	{
		//Start the remove/destroy coroutine
		StartCoroutine (RemoveCasing ());
		//Set random rotation at start
		transform.rotation = Random.rotation;
		//Start play sound coroutine
		StartCoroutine (PlaySound ());
	}

	private void FixedUpdate () 
	{
		//Spin the casing based on speed value
		transform.Rotate (Vector3.right, speed * Time.deltaTime);
		transform.Rotate (Vector3.down, speed * Time.deltaTime);
	}

	private IEnumerator PlaySound () 
	{
		//Wait for random time before playing sound clip
		yield return new WaitForSeconds (Random.Range(0.25f, 0.85f));
		//Get a random casing sound from the array 
		audioSource.clip = casingSounds
			[Random.Range(0, casingSounds.Length)];
		//Play the random casing sound
		audioSource.Play();
	}

	private IEnumerator RemoveCasing () 
	{
		//Destroy the casing after set amount of seconds
		yield return new WaitForSeconds (despawnTime);
		//Destroy casing object
		Destroy (gameObject);
	}
}