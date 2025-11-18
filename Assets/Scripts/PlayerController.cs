using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    // HACK: Remove this
    [SerializeField] TileSelectVisualisation TileSelectVisualisation;
    [SerializeField] private float MovementSpeed = 200.0f;
    [SerializeField] private bool UseWorldSpaceMovement;
    [SerializeField] private Transform FeetLocation;
    private Rigidbody2D Rigidbody;
    private Animator AnimationController;
    private Vector2 MovementInput;
    private int CurrentDirectionIndex = 0;
    private bool TrapPlacementEnabled = false;

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
        AnimationController.SetBool("IsWalking", Rigidbody.linearVelocity.magnitude > 0.0f);
    }

    private void FixedUpdate()
    {
        Vector2 Direction = CalculateMovementDirection();
        Rigidbody.linearVelocity = Direction * MovementSpeed * Time.fixedDeltaTime;
    }

    public void Move(InputAction.CallbackContext Context)
    {
        MovementInput = Context.ReadValue<Vector2>();
        // Only recalculate direction if we’re moving
        if (MovementInput.magnitude > 0.0f)
            CurrentDirectionIndex = CalculateDirectionIndex();
    }

    public void ToggleTrapPlacement(InputAction.CallbackContext Context)
    {
        TrapPlacementEnabled = !TrapPlacementEnabled;
        Events.Gameplay.BroadcastToggleTrapPlacementEvent(TrapPlacementEnabled);
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
}
