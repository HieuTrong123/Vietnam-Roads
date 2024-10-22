using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(20*Time.deltaTime,0,0);
    }
    private void OnTriggerEnter(Collider other){
        if(other.tag=="Player"){
            FindObjectOfType<AudioManager>().PlaySound("PickUpCoin");
            PlayerManager.numberOfCoins++;
            Destroy(gameObject);
        }
    }
}
