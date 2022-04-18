using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

using InputCallback = UnityEngine.InputSystem.InputAction.CallbackContext;

namespace TakoStories
{
	[RequireComponent(typeof(PlayerInput))]
	[RequireComponent(typeof(Rigidbody2D))]
	[RequireComponent(typeof(Collider2D))]
	[RequireComponent(typeof(GroundCheck))]
	public class Player : MonoSingleton<Player>
	{
		[Header("Movement")]
		[SerializeField] private float acceleration;
		[SerializeField] private float maxSpeed;
		//This is here to avoid the player sliding too much when the input is zero
		[SerializeField] private float neutralInputDeceleration;
		[SerializeField] private float decelerationThreshold;
		[Header("Jump")]
		[SerializeField] private float jumpForce;
		[SerializeField] private AnimationCurve jumpGravityCurve;
		[SerializeField] private Vector2 jumpCurveGraphScale;
		[SerializeField] [Delayed] private float jumpBufferTime;

		private PlayerInput input;

		private BufferedBoolean jumpInput;

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
		public float MoveInput { get; private set; }
		public Rigidbody2D Rb { get; private set; }
		public GroundCheck Gc { get; private set; }

		private float jumpTime = float.NegativeInfinity;

		#region Messages
		protected override void Awake()
		{
			base.Awake();
			InitializeInput();

			jumpInput = new BufferedBoolean(jumpBufferTime);
		}
		private void Start()
		{
			Rb = GetComponent<Rigidbody2D>();
			Gc = GetComponent<GroundCheck>();
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
			DebugEx.SetElement(0, Rb.velocity);
			if (PauseMenu.IsPaused) return;

			if (jumpInput) Jump();

			if (!Gc.IsGrounded)
			{
				float jumpCurveX = (Time.time - jumpTime) / jumpCurveGraphScale.x;
				if (jumpCurveX < 1f)
					Rb.gravityScale = jumpGravityCurve.Evaluate(jumpCurveX) * jumpCurveGraphScale.y;
			}

			Move();
		}
		private void Update()
		{
			if (PauseMenu.IsPaused) return;
		}
		private void OnValidate()
		{
			jumpInput = new BufferedBoolean(jumpBufferTime);
		}
		#endregion
		private void Jump()
		{
			if (!Gc.IsGroundedBuffer) return;

			if (Rb.velocity.y < 0f)
				Rb.velocity *= Vector2.right;
			Rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
			Jumping = true;

			jumpInput.Reset();

			Gc.Lock();
		}
		private void Move()
		{
			float force = 1f;
			if (MoveInput == 0f)
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
				}
			}
			else
			{
				if (MathEx.MaxSpeed(Rb.velocity, maxSpeed * MoveInput * Vector2.right))
				{
					force *= acceleration;
					Rb.AddForce(new Vector2(MoveInput * force, 0f));
				}
			}
		}

		#region Input
		private InputAction moveAction;
		private InputAction pauseAction;

		private delegate void InputHandler(InputCallback ctx);
		private Dictionary<InputAction, InputHandler> actionHandlers;

		private void InitializeInput()
		{
			input = GetComponent<PlayerInput>();

			actionHandlers = new Dictionary<InputAction, InputHandler>(8);

			moveAction = input.actions.FindAction("Move");
			actionHandlers.Add(moveAction, Move);

			moveAction = input.actions.FindAction("Jump");
			actionHandlers.Add(moveAction, Jump);

			pauseAction = input.actions.FindAction("Pause");
			actionHandlers.Add(pauseAction, Pause);
		}
		private void ReadInput(InputCallback ctx)
		{
			if (actionHandlers.TryGetValue(ctx.action, out InputHandler ih))
				ih(ctx);
		}
		private void Move(InputCallback ctx)
		{
			MoveInput = ctx.ReadValue<float>();
			MoveInput = Mathf.Clamp(MoveInput, -1f, 1f);
		}
		private void Jump(InputCallback ctx)
		{
			if (ctx.performed)
			{
				jumpInput.Buffer(true);
			}
		}
		private void Pause(InputCallback ctx)
		{
			if (!ctx.performed) return;
			PauseMenu.Inst.TogglePauseMenu();
		}
		#endregion
	}
}