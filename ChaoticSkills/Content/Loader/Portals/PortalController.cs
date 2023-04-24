/*using System;

namespace ChaoticSkills.Content.Loader {
    public class PortalController : MonoBehaviour {
        public PortalType portalType;
        private PortalController linkedPortal;
        public TeamIndex team = TeamIndex.Player;
        private bool isEnabled = true;
        private Camera portalCamera;

        private void Start() {
            ClearDuplicatePortals();
            UpdateLink();
            portalCamera = transform.root.Find("Camera").GetComponent<Camera>();
        } 

        private void ClearDuplicatePortals() {
            PortalController[] controllers = GameObject.FindObjectsOfType<PortalController>();
            for (int i = 0; i < controllers.Length; i++) {
                if (controllers[i].portalType == portalType && controllers[i] != this) {
                    Destroy(controllers[i].transform.parent.gameObject);
                }
            }
        }

        public void UpdateLink() {
            PortalController[] controllers = GameObject.FindObjectsOfType<PortalController>();
            for (int i = 0; i < controllers.Length; i++) {
                if (controllers[i].portalType != portalType) {
                    linkedPortal = controllers[i];
                    return;
                }
            }
        }

        public void OnTriggerEnter(Collider col) {
            UpdateLink();
            if (!linkedPortal || !isEnabled) {
                return;
            }

            linkedPortal.Deactivate();

            Transform root = col.transform.root;
            Debug.Log(root.gameObject);
            CharacterModel model = root.GetComponent<CharacterModel>();
            if (model) {
                root = model.body.transform;
            }
            Rigidbody rb = root.GetComponent<Rigidbody>();
            CharacterMotor motor = root.GetComponent<CharacterMotor>();
            TeamComponent tc = root.GetComponent<TeamComponent>();
            bool canEnter = true;
            if (tc && tc.teamIndex != team) {
                canEnter = false;
            }

            Vector3 pos = linkedPortal.transform.position + (1f * linkedPortal.transform.forward);

            CharacterDirection direction = root.GetComponent<CharacterDirection>();

            if (canEnter) {
                float velocity;
                if (rb && !motor) {
                    velocity = rb.velocity.magnitude;
                    TeleportHelper.TeleportGameObject(root.gameObject, linkedPortal.transform.position);
                    rb.AddForce(velocity * linkedPortal.transform.forward, ForceMode.VelocityChange);
                    if (direction) {
                        direction.forward = linkedPortal.transform.forward;
                    }
                    else {
                        rb.rotation = Quaternion.LookRotation(linkedPortal.transform.forward);
                    }
                }
                else if (motor) {
                    velocity = motor.velocity.magnitude;
                    Debug.Log(velocity);
                    TeleportHelper.TeleportGameObject(root.gameObject, linkedPortal.transform.position);
                    motor.ApplyForce(velocity * motor.mass * linkedPortal.transform.forward, true, false);
                    if (direction) {
                        direction.forward = linkedPortal.transform.forward;
                    }   
                }
            }
        }

        public void Deactivate() {
            isEnabled = false;
            Invoke(nameof(Reactivate), 0.2f);
        }

        private void Reactivate() {
            isEnabled = true;
        }
    }
}*/