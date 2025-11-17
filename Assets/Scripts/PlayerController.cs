using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float MovementSpeed = 200.0f;
    private Rigidbody2D Rigidbody;
    private Animator AnimationController;
    private Vector2 MovementVector;
    private int CurrentDirectionIndex;

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
        Rigidbody.linearVelocity = MovementVector * MovementSpeed * Time.fixedDeltaTime;
    }

    public void Move(InputAction.CallbackContext Context)
    {
        MovementVector = Context.ReadValue<Vector2>();
        // Only recalculate direction if we’re moving
        if (MovementVector.magnitude > 0.0f)
            CurrentDirectionIndex = CalculateDirectionIndex();
    }

    private int CalculateDirectionIndex()
    {
        const int NumDirections = 8;
        const float DegreesPerIndex = 360.0f / (float)NumDirections;
        const float HalfIndexOffset = DegreesPerIndex / 2.0f;
        float SignedAngle = Vector2.SignedAngle(MovementVector, Vector2.up);
        SignedAngle -= HalfIndexOffset;
        // Wrap around to positive
        SignedAngle = SignedAngle < 0.0f ? SignedAngle + 360.0f : SignedAngle;
        int Index = Mathf.FloorToInt(SignedAngle / DegreesPerIndex);
        return Index;
    }
}
