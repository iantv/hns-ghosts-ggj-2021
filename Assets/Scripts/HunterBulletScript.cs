using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HunterBulletScript : MonoBehaviour
{
    [SerializeField] private float damage =10f;
    void Start()
    {
        //  StartCoroutine(Destroy());
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("HidingPlayer")) return;
        other.transform.GetComponent<HidingPlayerController>().GetDamaged(damage);
        this.GetComponent<Rigidbody>().detectCollisions = false;
        this.GetComponent<Rigidbody>().isKinematic = true;

    }

    private IEnumerator Destroy()
    {
        yield return  new WaitForSeconds(2f);
        Destroy(this.gameObject);
    }
}
