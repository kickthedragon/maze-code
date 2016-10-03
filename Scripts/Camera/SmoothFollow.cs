using UnityEngine;
using System.Collections;
using Prime31;


public class SmoothFollow : MonoBehaviour
{
	public Transform target;
	public float smoothDampTime = 0.2f;
	[HideInInspector]
	public new Transform transform;
	public Vector3 cameraOffset;
	public bool useFixedUpdate = false;
	
	private CharacterController _playerController;
	private Vector3 _smoothDampVelocity;

	public float orthoZoomSpeed = 2f;

	private Player player;

	private new Camera camera;

	public static float zoomMin = 20f;

    public float zoomAmount = 4f;
	
	void OnEnable()
	{
		MazeGenerator.OnGenerated += SetTarget;
	}

	void OnDisable()
	{
		MazeGenerator.OnGenerated -= SetTarget;
	}

	void Awake()
	{
		transform = gameObject.transform;
		//_playerController = target.GetComponent<CharacterController2D>();
		camera = GetComponent<Camera> ();
	}
	
	void SetTarget()
	{
		_playerController = GameObject.FindGameObjectWithTag ("Player").GetComponent<CharacterController>();
		player = _playerController.GetComponent<Player> ();
		target = _playerController.transform;
		transform.position = target.position - cameraOffset;
	}

	void LateUpdate()
	{
		if( !useFixedUpdate )
			updateCameraPosition();
	}
	
	
	void FixedUpdate()
	{
		if( useFixedUpdate )
			updateCameraPosition();
	}
	
	
	void updateCameraPosition()
	{
		if( _playerController == null )
		{
		//	transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
			return;
		}
		
		if( _playerController.velocity.x > 0 )
		{
			transform.position = Vector3.SmoothDamp( transform.position, target.position - cameraOffset, ref _smoothDampVelocity, smoothDampTime );
		}
		else
		{
			var leftOffset = cameraOffset;
			leftOffset.x *= -1;
			transform.position = Vector3.SmoothDamp( transform.position, target.position - leftOffset, ref _smoothDampVelocity, smoothDampTime );
		}

		float targetSize = zoomMin;

		if((Mathf.Abs(player.normalizedXSpeed) > 0) && (Mathf.Abs(player.normalizedYSpeed) == 0))
			targetSize = Mathf.Clamp ((Mathf.Abs(player.normalizedXSpeed) * (zoomMin + zoomAmount)), zoomMin, (zoomMin + zoomAmount));
		else if ((Mathf.Abs(player.normalizedYSpeed) > 0) && (Mathf.Abs(player.normalizedXSpeed) == 0))
			targetSize = Mathf.Clamp ((Mathf.Abs(player.normalizedYSpeed) * (zoomMin + zoomAmount)),zoomMin, (zoomMin + zoomAmount));
		else if ((Mathf.Abs(player.normalizedXSpeed) > 0) && (Mathf.Abs(player.normalizedYSpeed) > 0))
			targetSize = Mathf.Clamp((Mathf.Abs(player.normalizedXSpeed) + Mathf.Abs(player.normalizedYSpeed) * (zoomMin + zoomAmount)), zoomMin, (zoomMin + zoomAmount));
		camera.orthographicSize = Mathf.Lerp (camera.orthographicSize, targetSize, Time.deltaTime * orthoZoomSpeed);

	}
	
}