using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

using InputContext = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TakoStories
{
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(GroundCheck))]
	public class Player : MonoSingleton<Player>, IKillable
	{
		[Header("Movement")]
		[SerializeField] private float acceleration;
		[SerializeField] private float maxSpeed;
		[SerializeField] private float airControl;
		//This is here to avoid the player sliding too much when the input is zero
		[SerializeField] private float neutralInputDeceleration;
		[SerializeField] private float decelerationThreshold;
		[Header("Jump")]
		[SerializeField] private float jumpForce;
		[SerializeField] private AnimationCurve jumpGravityCurve;
		[SerializeField] private Vector2 jumpCurveGraphScale;
		[SerializeField] [Delayed] private float jumpBufferTime;
		[SerializeField] private int maxAirJumps = 1;
		[SerializeField] private float jumpVelocityAttenuation;
		[Header("Misc")]
		[SerializeField] private PlayerCursor cursor;
		[SerializeField] private bool flyMode;

		//[HideInInspector] public List<Tako> takos;

		private int jumpCount;

		private Rigidbody2D rb;
		public Rigidbody2D Rb
		{
			get
			{
				if (rb == null)
					rb = GetComponent<Rigidbody2D>();
				return rb;
			}
			private set => rb = value;
		}
		private GroundCheck gc;
		public GroundCheck Gc
		{
			get
			{
				if (gc == null)
					gc = GetComponent<GroundCheck>();
				return gc;
			}
			private set => gc = value;
		}

		public Checkpoint Checkpoint { get; set; }

		private bool jumping;
		private bool Jumping
		{
			get => jumping;
			set
			{
				jumping = value;
				jumpTime = Time.time;
			}
		}

		private float jumpTime = float.NegativeInfinity;

		public bool FlyMode
		{
			get => flyMode;
			set
			{
				flyMode = value;
				if (Application.isPlaying)
				{
					if (value)
					{
						Rb.constraints = RigidbodyConstraints2D.FreezeAll;
						Rb.gravityScale = 0f;
					}
					else
					{
						Rb.constraints = RigidbodyConstraints2D.FreezeRotation;
						Rb.gravityScale = 1f;
					}
				}
			}
		}

		public bool Frozen
		{
			get => frozen; 
			set
			{
				//if (value)
				//{
				//	cursor.DragEnabled = false;
				//	cursor.PowerEnabled = false;
				//}
				frozen = value;
			}
		}


		#region Messages
		protected override void Awake()
		{
			base.Awake();
			InitializeInput();

			jumpInput = new BufferedBoolean(jumpBufferTime);
		}
		private void Start()
		{
			cursor = Instantiate(cursor);
			FlyMode = FlyMode;
		}
		private void OnEnable()
		{
			input.onActionTriggered += ReadInput;
		}
		private void OnDisable()
		{
			input.onActionTriggered -= ReadInput;
		}
		private void FixedUpdate()
		{
			//DebugEx.SetElement(0, Rb.velocity);
			if (PauseMenu.IsPaused) return;
			if (Frozen)
			{
				if (Gc.IsGrounded)
					rb.velocity *= Vector2.up;
				return;
			}

			if (flyMode)
			{
				transform.position += Time.fixedDeltaTime * maxSpeed * (Vector3)MoveInput;
			}
			else
			{
				if (Gc.IsGroundedBuffer)
					jumpCount = 0;
				if (jumpInput) Jump();

				if (!Gc.IsGrounded)
				{
					float jumpCurveX = (Time.time - jumpTime) / jumpCurveGraphScale.x;
					if (jumpCurveX < 1f)
						Rb.gravityScale = jumpGravityCurve.Evaluate(jumpCurveX) * jumpCurveGraphScale.y;
				}
			}
			Move();
		}
		private void OnValidate()
		{
			jumpInput = new BufferedBoolean(jumpBufferTime);
			FlyMode = FlyMode;
		}
		#endregion
		private void Jump()
		{
			if (jumpCount >= maxAirJumps) return;

			if (Rb.velocity.y < 0f)
				Rb.velocity *= Vector2.right;
			else
				Rb.velocity *= new Vector2(1f, jumpVelocityAttenuation);

			Rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
			Jumping = true;

			if (!Gc.IsGroundedBuffer)
				jumpCount++;

			jumpInput.Reset();
			Gc.Lock();
		}
		private void Move()
		{
			float force = 1f;
			if (MoveInput.x == 0f)
			{
				if (Gc.IsGrounded)
				{
					//Supposed to be for slopes but I'll keep it like this
					Vector2 pVelocity = Vector3.Project(Rb.velocity, transform.right);
					if (pVelocity.sqrMagnitude > decelerationThreshold * decelerationThreshold)
					{
						force *= neutralInputDeceleration;
						Rb.AddForce(-pVelocity.normalized * force);
					}
					else
					{
						Rb.velocity *= Vector2.up;
					}
				}
			}
			else
			{
				if (MathEx.MaxSpeed(Rb.velocity, maxSpeed * MoveInput * Vector2.right))
				{
					force *= acceleration;
					if (!Gc.IsGrounded) force *= airControl;
					Rb.AddForce(new Vector2(MoveInput.x * force, 0f));
				}
			}
		}


		public void Kill() => Respawn();
		public void Respawn()
		{
			transform.position = Checkpoint.SpawnPoint;
			Rb.velocity = Vector2.zero;
			//cursor.ResetPosition();
		}

		#region Input
		private delegate void InputHandler(InputContext ctx);
		private Dictionary<InputAction, InputHandler> actionHandlers;
		private PlayerInput input;

		private BufferedBoolean jumpInput;
		private bool frozen;

		public Vector2 MoveInput { get; private set; }

		private void InitializeInput()
		{
			input = GetComponent<PlayerInput>();

			actionHandlers = new Dictionary<InputAction, InputHandler>()
			{
				{ input.actions.FindAction("Move"), OnMove  },
				{ input.actions.FindAction("Jump"), OnJump  },
				{ input.actions.FindAction("Drag"), OnDrag  },
				//{ input.actions.FindAction("Power"), OnPower  },
				//{ input.actions.FindAction("Recall"), OnRecall  },
				//{ input.actions.FindAction("Reset"), OnReset  },
				{ input.actions.FindAction("Pause"), OnPause  }

			};
		}
		private void ReadInput(InputContext ctx)
		{
			if (actionHandlers.TryGetValue(ctx.action, out InputHandler ih))
				ih(ctx);
		}
		private void OnMove(InputContext ctx)
		{
			MoveInput = ctx.ReadValue<Vector2>();
			MoveInput = Vector2.ClampMagnitude(MoveInput, 1f);
		}
		private void OnJump(InputContext ctx)
		{
			if (ctx.performed)
			{
				jumpInput.Buffer(true);
			}
		}
		private void OnDrag(InputContext ctx)
		{
			if (ctx.performed)
				cursor.EnableDrag = true;
			if (ctx.canceled)
				cursor.EnableDrag = false;
		}
	//private void OnPower(InputContext ctx)
	//{
	//	if (ctx.performed)
	//		cursor.PowerEnabled = true;
	//	if (ctx.canceled)
	//		cursor.PowerEnabled = false;
	//}
	//private void OnRecall(InputContext ctx)
	//{
	//	if (ctx.performed)
	//		takos.ForEach(t => t.transform.position = transform.position);
	//}
	//private void OnReset(InputContext ctx)
	//{
	//	if (ctx.performed)
	//		GameManager.Inst.ResetPuzzle();
	//}
	private void OnPause(InputContext ctx)
		{
			if (!ctx.performed) return;
			PauseMenu.Inst.TogglePauseMenu();
		}
		#endregion
	}
}