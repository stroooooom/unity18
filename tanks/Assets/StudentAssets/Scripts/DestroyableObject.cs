using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableObject : MonoBehaviour {

    private Transform _transform;

    public float impulseToDestroy;
    public float timeToDestroy;
    public bool Explosion;
    public float ExplosionRadius;
    public float ExplosionForce;


	void Start () {
        _transform = GetComponent<Transform>();	
	}


	private void OnCollisionEnter(Collision collision)
	{
        if (Vector3.Magnitude(collision.impulse) < impulseToDestroy)
        {
            return;
        }

        if (!Explosion)
        {
            Destroy(gameObject, timeToDestroy);
            return;
        }

        Collider[] hitColliders = Physics.OverlapSphere(_transform.position, ExplosionRadius);

        for (int i = 0; i < hitColliders.Length; i++)
        {
            var attachedRigidbody = hitColliders[i].attachedRigidbody;
            if (attachedRigidbody != null)
            {
                attachedRigidbody.AddExplosionForce(ExplosionForce, _transform.position, ExplosionRadius);
            }
        }
        Destroy(gameObject, timeToDestroy);
	}
}
