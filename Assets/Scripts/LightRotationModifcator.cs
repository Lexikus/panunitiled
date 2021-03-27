using UnityEngine;

public class LightRotationModifcator : MonoBehaviour {
    public float speed = 1;
    private struct State {
        public bool hold;
        public short direction;
    }
    private State x = new State();
    private State y = new State();

    public void Update() {
        if (x.hold) {
            OnXChange(x.direction * speed * Time.deltaTime);
        }

        if (y.hold) {
            OnYChange(y.direction * speed * Time.deltaTime);
        }
    }

    public void OnHoldXPositive(bool state) {
        x.hold = state;
        x.direction = 1;
    }

    public void OnHoldXNegative(bool state) {
        x.hold = state;
        x.direction = -1;
    }

    public void OnHoldYPositive(bool state) {
        y.hold = state;
        y.direction = 1;
    }

    public void OnHoldYNegative(bool state) {
        y.hold = state;
        y.direction = -1;
    }

    private void OnXChange(float x) {
        transform.Rotate(0, x, 0, Space.World);
    }

    private void OnYChange(float y) {
        transform.Rotate(y, 0, 0, Space.World);
    }
}
