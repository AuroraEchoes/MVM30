using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float MovementSpeed = 200.0f;
    [SerializeField] private bool UseWorldSpaceMovement;

    [Header("References")]
    [SerializeField] private Transform FeetLocation;
    [SerializeField] private WorldInteractionManager World;


    //Dash Settings
    [Header("Dash")]
    [SerializeField] private float DashDistance = 3.0f;
    [SerializeField] private float DashDuration = 0.2f;
    [SerializeField] private float DashCooldown = 1.0f;

    //Dash Variables
    private bool IsDashing = false;
    private float DashTimer = 0f;
    private float DashCooldownTimer = 0f;
    private Vector2 DashDirection = Vector2.zero;


    private Rigidbody2D Rigidbody;
    private Animator AnimationController;
    private Vector2 MovementInput;
    private int CurrentDirectionIndex = 0;
    private bool TrapPlacementEnabled = false;

    // HACK: I dont thiiiiink this should be serialized
    // TODO: We need to have some kind of active trap i thikn?
    [SerializeField] private List<TrapData> Traps = new List<TrapData>();
    private Dictionary<string, float> TrapCooldowns = new Dictionary<string, float>();
    // Sentinel value -1 (no selected trap)
    private int SelectedTrapIndex = -1;


    private static Matrix2D ScreenControlsMatrix = new Matrix2D(new Vector2(2.0f, 0.0f), new Vector2(0.0f, 1.0f));
    private static Matrix2D WorldControlsMatrix = new Matrix2D(new Vector2(2.0f, -1.0f), new Vector2(2.0f, 1.0f));




    private void Start()
    {
        Rigidbody = GetComponent<Rigidbody2D>();
        AnimationController = GetComponent<Animator>();
    }

    private void Update()
    {
        AnimationController.SetFloat("Direction", CurrentDirectionIndex);
        AnimationController.SetBool("IsWalking", Rigidbody.linearVelocity.magnitude > 0.0f && !IsDashing);

        // Handle dash timers
        if (IsDashing)
        {
            DashTimer -= Time.deltaTime;
            if (DashTimer <= 0f)
            {
                IsDashing = false;

            }
        }

        if (DashCooldownTimer > 0f)
        {
            DashCooldownTimer -= Time.deltaTime;
        }

        TickTrapCooldowns();
    }

    private void FixedUpdate()
    {
        if (IsDashing)
        {
           //Dashing things
            float dashSpeed = DashDistance / DashDuration;
            Rigidbody.linearVelocity = DashDirection * dashSpeed;
        }
        else
        {
            // Normal movement
            Vector2 Direction = CalculateMovementDirection();
            Rigidbody.linearVelocity = Direction * MovementSpeed * Time.fixedDeltaTime;
        }
    }

    public void Move(InputAction.CallbackContext Context)
    {
        MovementInput = Context.ReadValue<Vector2>();
        // Only recalculate direction if we’re moving
        if (MovementInput.magnitude > 0.0f)
            CurrentDirectionIndex = CalculateDirectionIndex();
    }

    public void PlaceTrapIndex1(InputAction.CallbackContext Context)
    {
        StartPlaceTrap(0);
    }

    public void PlaceTrapIndex2(InputAction.CallbackContext Context)
    {
        StartPlaceTrap(1);
    }

    public void ConfirmTrapLocation(InputAction.CallbackContext Context)
    {
        if (TrapPlacementEnabled && SelectedTrapIndex != -1 && CanUseTrap(SelectedTrapIndex))
        {
            Vector3 MousePosWorld = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TrapData TrapData = Traps[SelectedTrapIndex];
            bool SuccesfullyPlaced = World.TryPlaceTrap(FeetLocation.position, MousePosWorld, TrapData);
            if (SuccesfullyPlaced)
            {
                // Exit placement mode
                TrapPlacementEnabled = false;
                SelectedTrapIndex = -1;
                World.DisableTrapPlacementVisualisation();
            }
        }
    }

    private int CalculateDirectionIndex()
    {
        const int NumDirections = 8;
        const float DegreesPerIndex = 360.0f / (float)NumDirections;
        const float HalfIndexOffset = DegreesPerIndex / 2.0f;
        float SignedAngle = Vector2.SignedAngle(MovementInput, Vector2.up);
        SignedAngle -= HalfIndexOffset;
        if (UseWorldSpaceMovement)
            SignedAngle += DegreesPerIndex;
        // Wrap around to positive
        SignedAngle = SignedAngle < 0.0f ? SignedAngle + 360.0f : SignedAngle;
        int Index = Mathf.FloorToInt(SignedAngle / DegreesPerIndex);
        return Index;
    }

    private Vector2 CalculateMovementDirection()
    {
        Vector2 MovementInput = this.MovementInput;
        Matrix2D Matrix = UseWorldSpaceMovement ? WorldControlsMatrix : ScreenControlsMatrix;
        return Matrix.Mult(MovementInput).normalized;
    }

    private void TickTrapCooldowns()
    {
        foreach (string TrapIdentifier in TrapCooldowns.Keys)
        {
            TrapCooldowns[TrapIdentifier] -= Time.deltaTime;
            if (TrapCooldowns[TrapIdentifier] <= 0.0f)
                TrapCooldowns.Remove(TrapIdentifier);
        }
    }

    public void AddTrap(TrapData Data)
    {
        Traps.Add(Data);
    }

    private bool CanUseTrap(int Index)
    {
        // We don’t have a trap index this high
        if (Traps.Count <= Index)
            return false;
        TrapData Data = Traps[Index];
        // On cooldown
        if (TrapCooldowns.ContainsKey(Data.TrapIdentifier))
            return false;
        return true;
    }

    private void StartPlaceTrap(int Index)
    {
        // Trap placement is enabled. If it’s the same trap we’re already placing, toggle vis off
        // Otherwise, switch to that trap
        if (TrapPlacementEnabled && SelectedTrapIndex == Index)
        {
            TrapPlacementEnabled = false;
            World.DisableTrapPlacementVisualisation();
        }
        else if (CanUseTrap(Index))
        {
            SelectedTrapIndex = Index;
            TrapPlacementEnabled = true;
            World.EnableTrapPlacementVisualisation(Traps[Index]);
        }
    }

    public void Dash(InputAction.CallbackContext Context)
    {
        //Checking if we can dash (Cant if we are already dashing and dash cd) --> Could just remove isdashing but upgrades maybe??
        if (Context.started && !IsDashing && DashCooldownTimer <= 0f)
        {
            StartDash();
        }
    }

    private void StartDash()
    {
        // Look for our current mouse 
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPos.z = 0f;

        DashDirection = (mouseWorldPos - transform.position).normalized; 


        //Making sure not to dash on the same spot
        if (DashDirection.magnitude > 0.1f)
        {
            IsDashing = true;
            DashTimer = DashDuration;
            DashCooldownTimer = DashCooldown;

          //Add animation here
        }
    }


}
