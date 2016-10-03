using UnityEngine;
using System.Collections;
using Prime31;
using System;

[RequireComponent(typeof(PlayerEventManager))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(PlayerInput))]
public class Player : MonoBehaviour {

	private CharacterController controller;

	private Rigidbody rigidBody;

	public PlayerInput input { get; private set; }

	public float normalizedXSpeed;
	public float normalizedYSpeed;

	private Vector3 velocity;

	public Vector3 Velocity { get { return velocity; } }

	public float movementSpeed = 10f;

	public float groundDamp = 14f;

	public float mobileGroundDamp = 2f;

	private Vector3 lastPosition;

	void OnEnable()
	{
		PlayerEventManager.OnPlayerMove += move;


	}

	void OnDisable()
	{
		PlayerEventManager.OnPlayerMove -= move;

	}

	void Awake()
	{
		init ();
	}


	void init()
	{
		controller = GetComponent<CharacterController>();
		rigidBody = GetComponent<Rigidbody> ();
		input = GetComponent<PlayerInput>();
		lastPosition = transform.position;

	}

	void Start()
	{
		PlayerEventManager.FirePlayerSpawned (transform.position.x, transform.position.z);
		StartCoroutine (updatePositionForMaze ());
	}

	void OnControllerColliderHit(ControllerColliderHit hit)
	{
		//RumbleControl.FireRumble ();
	}

	void FixedUpdate()
	{
		velocity = controller.velocity;

		checkForYChange ();

		moveBehaviour ();
	}

	void LateUpdate()
	{
		recordPlayerPosition ();

	}

	IEnumerator updatePositionForMaze()
	{
		while (true) 
		{
			if ((Mathf.Abs (transform.position.x - lastPosition.x) > 1f) || (Mathf.Abs (transform.position.z - lastPosition.z) > 1f))
				PlayerEventManager.FireUpdatePosition (transform.position.x, transform.position.z);

			yield return new WaitForSeconds(.1f);
		}
	}

	void move(Vector3 direction)
	{
		normalizedXSpeed = direction.x;
		normalizedYSpeed = direction.z;
		//direction *= movementSpeed;
		//transform.Translate (direction * Time.deltaTime);
	}

	
	void recordPlayerPosition()
	{
		if ((Mathf.Abs (transform.position.x - lastPosition.x) > 1f) || (Mathf.Abs (transform.position.z - lastPosition.z) > 1f)) {
			//Debug.Log("Recording Position");
			// Parse the current position into the string && save it in player prefs
			Settings.SetPlayerPosition(Settings.Vector3ToString(transform.position));
			lastPosition = transform.position;

		}
	}





	void checkForYChange()
	{
		if (transform.position.y != 1.5f)
			transform.position = new Vector3 (transform.position.x, 1.5f, transform.position.z);
	}

	void moveBehaviour()
	{
#if UNITY_STANDALONE
		var smoothedMovementFactor = groundDamp;
#endif
#if UNITY_ANDROID || UNITY_IPHONE
		var smoothedMovementFactor = mobileGroundDamp;
#endif
		velocity.x = Mathf.Lerp(Velocity.x, normalizedXSpeed * movementSpeed, Time.deltaTime * smoothedMovementFactor);
		velocity.z = Mathf.Lerp(Velocity.z, normalizedYSpeed * movementSpeed, Time.deltaTime * smoothedMovementFactor);

		controller.Move(Velocity * Time.deltaTime);
	}
}
