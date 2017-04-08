using UnityEngine;
using System.Collections;

public class PlayerShooting : MonoBehaviour {

    public ParticleSystem muzzleFlash;
    private Animator anim;
    public GameObject bulletHitDecal;

    private GameObject[] _decals;
    int bulletHitIndex = 0;
    int maxImpacts = 5;

    bool shooting = false;

    // Use this for initialization
    void Start() {

        _decals = new GameObject[maxImpacts];
        for (int i = 0; i < maxImpacts; i++)
            _decals[i] = (GameObject)Instantiate(bulletHitDecal);

        anim = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetButtonDown("Fire1")) {
            muzzleFlash.Play();
            anim.SetTrigger("Fire");
            shooting = true;
        }

    }

    void FixedUpdate() {
        if (shooting) {
            shooting = false;

            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 50f)) {
                if (hit.transform.tag == "Enemy")
                    Destroy(hit.transform.gameObject);

                _decals[bulletHitIndex].transform.position = hit.point;
                _decals[bulletHitIndex].GetComponent<ParticleSystem>().Play();

                if (bulletHitIndex++ >= maxImpacts)
                    bulletHitIndex = 0;
            }
        }
    }

}