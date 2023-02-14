using System;

namespace ChaoticSkills.Content.Loader {
    public class PortalProjectile : MonoBehaviour {
        public GameObject portalPrefab;
        public ProjectileImpactEventCaller eventCaller => GetComponent<ProjectileImpactEventCaller>();

        public void Start() {
            eventCaller.impactEvent.AddListener(OnImpact);
        }

        public void OnImpact(ProjectileImpactInfo info) {
            if (!NetworkServer.active) {
                return;
            }

            Vector3 pos = base.transform.position;

            GameObject portal = GameObject.Instantiate(portalPrefab, pos, Quaternion.LookRotation(base.transform.forward * -1f, Vector3.down));
            NetworkServer.Spawn(portal);
            GameObject.Destroy(this.gameObject);
        }
    }
}