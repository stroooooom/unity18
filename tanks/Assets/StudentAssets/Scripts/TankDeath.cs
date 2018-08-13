using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TankDeath : MonoBehaviour {

    const float verticalForceValue = 30000f;

    public ParticleSystem explosion;

	public void Death()
    {
        var tankRenderers = gameObject.transform.GetChild(0).gameObject;
        for (int i = 1; i < 5; i++)
        {
            tankRenderers.transform.GetChild(i).gameObject.SetActive(false);
        }
        tankRenderers.transform.GetChild(0).gameObject.SetActive(true);

        gameObject.GetComponent<Rigidbody>().AddForce(0f, verticalForceValue, 0f);

        var explosionInstance = Instantiate(explosion);
        explosionInstance.transform.position = gameObject.transform.position;
        explosionInstance.Play();
    }
}
