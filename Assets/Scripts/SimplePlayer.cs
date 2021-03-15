using UnityEngine;

static class AxisName {
    public static string Horizontal = "Horizontal";
    public static string Vertical = "Vertical";
}

static class AnimationName {
    public static string Walking = "Walking";
}

public class SimplePlayer : MonoBehaviour {
    public float speed;
    public float movementSmoothing = 0.05f;
    private Rigidbody2D rb2d;
    private Vector3 velocity = Vector3.zero;
    private Animator anim;
    private bool facingRight = true;

    public void Start() {
        rb2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public void FixedUpdate() {
        float moveHorizontal = Input.GetAxis(AxisName.Horizontal);
        float moveVertical = Input.GetAxis(AxisName.Vertical);

        Vector2 direction = new Vector2(moveHorizontal, moveVertical);
        rb2d.AddForce(direction * speed);
        rb2d.velocity = Vector3.SmoothDamp(rb2d.velocity, direction, ref velocity, movementSmoothing);

        if(direction.sqrMagnitude > 0) {
            anim.SetBool(AnimationName.Walking, true);
        } else {
            anim.SetBool(AnimationName.Walking, false);
        }

        if(direction.x < 0 && facingRight) {
            Flip();
        } else if(direction.x > 0 && !facingRight) {
            Flip();
        }
    }

    public void Flip() {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }
}