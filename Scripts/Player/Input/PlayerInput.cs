using UnityEngine;
using System.Collections;
using PlayerBindings;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerInput : IPlayerInput {
	
	
	public PlayerInputManager playerInputManger { get; private set; }


	public float moveStickDeadZone = .28f;


	float mobileXDeadZone = .05f;
	float mobileYDeadZone = .33f;
	float yBuffer = .1f;

	public bool allowInput = true;

	public bool madeFirstMove;

	public Vector3 direction;

	private float mobileSmooth = .1f;
	private float mobileHaxis = 0;
	private float mobileVaxis = 0;
	private Vector3 curAc;
	void OnEnable()
	{
		PlayerEventManager.OnPlayerOpenMenu += openMenuSwitch;
		PlayerEventManager.OnPlayerCloseMenu += closeMenuSwitch;
	}

	void OnDisable()
	{
		PlayerEventManager.OnPlayerOpenMenu -= openMenuSwitch;
		PlayerEventManager.OnPlayerCloseMenu -= closeMenuSwitch;
	}

	protected override void Awake()
	{
		base.Awake();
		playerInputManger = GetComponent<PlayerInputManager>();
	}
	
	
	void Update ()
	{

		//if(Input.GetMouseButtonDown(0))	{ PlayerMenu(allowInput); }

		if (playerInputManger.playerInput.Menu.WasPressed) { PlayerMenu(allowInput); }

		if (playerInputManger.playerInput.ToggleDebug.WasPressed) { ToggleDebug(); }

		if (playerInputManger.playerInput.ZoomIn.IsPressed) { ZoomIn(); }

		if (playerInputManger.playerInput.ZoomOut.IsPressed) { ZoomOut(); }

		if (!allowInput) return;

		if (playerInputManger.playerInput.ToggleTimer.WasPressed) { ToggleTimer(); }
#if !UNITY_ANDROID && !UNITY_IPHONE

		if (playerInputManger.playerInput.Move.X >= moveStickDeadZone) {
			if(!madeFirstMove)
			{
				PlayerEventManager.FirePlayerMadeFirstMove();
				madeFirstMove = true;
			}
			direction.x = 1;
		} else if (playerInputManger.playerInput.Move.X <= -moveStickDeadZone) {
			if(!madeFirstMove)
			{
				PlayerEventManager.FirePlayerMadeFirstMove();
				madeFirstMove = true;
			}
			direction.x = -1;
		}
		else if ((playerInputManger.playerInput.Move.X == 0) || (playerInputManger.playerInput.Move.X >= 0 && playerInputManger.playerInput.Move.X <= moveStickDeadZone) || (playerInputManger.playerInput.Move.X >= -moveStickDeadZone && playerInputManger.playerInput.Move.X <= 0))
			direction.x = 0;
		

		if (playerInputManger.playerInput.Move.Y >= moveStickDeadZone) {
			if(!madeFirstMove)
			{
				PlayerEventManager.FirePlayerMadeFirstMove();
				madeFirstMove = true;
			}
			direction.z = 1;
		} else if (playerInputManger.playerInput.Move.Y <= -moveStickDeadZone) {
			if(!madeFirstMove)
			{
				PlayerEventManager.FirePlayerMadeFirstMove();
				madeFirstMove = true;
			}
			direction.z = -1;
		}
		else if ((playerInputManger.playerInput.Move.Y == 0) || (playerInputManger.playerInput.Move.Y >= 0 && playerInputManger.playerInput.Move.Y <= moveStickDeadZone) || (playerInputManger.playerInput.Move.Y >= -moveStickDeadZone && playerInputManger.playerInput.Move.Y <= 0))
			direction.z = 0;
	

		if ((playerInputManger.playerInput.Move.X == 0 && playerInputManger.playerInput.Move.Y == 0) || (playerInputManger.playerInput.Move.X >= 0 && playerInputManger.playerInput.Move.X <= moveStickDeadZone) && (playerInputManger.playerInput.Move.Y >= 0 && playerInputManger.playerInput.Move.Y <= moveStickDeadZone) || (playerInputManger.playerInput.Move.X >= -moveStickDeadZone && playerInputManger.playerInput.Move.X <= 0) && (playerInputManger.playerInput.Move.Y >= -moveStickDeadZone && playerInputManger.playerInput.Move.Y <= 0)) {
			direction.x = 0;
			direction.z = 0;
		}


#endif
#if UNITY_ANDROID || UNITY_IPHONE

		if (Input.acceleration.x >= mobileXDeadZone) {
			if(!madeFirstMove)
			{
				PlayerEventManager.FirePlayerMadeFirstMove();
				madeFirstMove = true;
			}
			direction.x = 1;
		} else if (Input.acceleration.x <= -mobileXDeadZone) {
			if(!madeFirstMove)
			{
				PlayerEventManager.FirePlayerMadeFirstMove();
				madeFirstMove = true;
			}
			direction.x = -1;
		}
		else if ((Input.acceleration.x == 0) || (Input.acceleration.x >= 0 && Input.acceleration.x <= mobileXDeadZone) || (Input.acceleration.x >= -mobileXDeadZone && Input.acceleration.x <= 0))
			direction.x = 0;
		
		
		if (Input.acceleration.y >= -mobileYDeadZone) {
			if(!madeFirstMove)
			{
				PlayerEventManager.FirePlayerMadeFirstMove();
				madeFirstMove = true;
			}
			direction.z = 1;
		} else if (Input.acceleration.y <= -mobileYDeadZone) {
			if(!madeFirstMove)
			{
				PlayerEventManager.FirePlayerMadeFirstMove();
				madeFirstMove = true;
			}
			direction.z = -1;
		}
		else if ((Input.acceleration.y == -.4f) || (Input.acceleration.y >= -.4f && Input.acceleration.y <= -mobileYDeadZone) || (Input.acceleration.y >= -mobileYDeadZone && Input.acceleration.y <= -.4f))
			direction.z = 0;
		
		
		if ((Input.acceleration.x == 0 && Input.acceleration.y == -.4f) || (Input.acceleration.x >= 0 && Input.acceleration.x <= mobileXDeadZone) && (Input.acceleration.y >= -.4f && Input.acceleration.y <= -mobileYDeadZone) || (Input.acceleration.x >= -mobileXDeadZone && Input.acceleration.x <= 0) && (Input.acceleration.y >= -mobileYDeadZone && Input.acceleration.y <= -.4f)) {
			direction.x = 0;
			direction.z = 0;
		}

#endif
		   PlayerMove (direction);
	}

	void openMenuSwitch()
	{
		direction = Vector3.zero;
		PlayerMove (direction);
		UITimer.ShowTimer ();
		allowInput = false;
	}

	void closeMenuSwitch()
	{
		UITimer.HideTimer ();
		allowInput = true;
	}
}