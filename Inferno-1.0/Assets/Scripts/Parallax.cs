using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using System;
using Cinemachine;
public class Parallax : MonoBehaviour
{
    [SerializeField]CinemachineVirtualCamera cam;
    [FormerlySerializedAs("Number of Duplication")]
    [SerializeField] int nd = 10;
    [SerializeField] float ratio;
    Rigidbody2D playerRigidbody;
    GameObject background;
    float startPos;
    void Start()
    {
        playerRigidbody = FindObjectOfType<GameController>().GetComponent<Rigidbody2D>();
        background = this.transform.GetChild(0).gameObject;
        var size = background.GetComponent<SpriteRenderer>().bounds.size.x;
        for (int i = 1; i <nd; i++)
        {
            var temp = Instantiate(background, GameObject.Find("Background Manager").transform);
            temp.transform.position = background.transform.position;
            temp.transform.position += new Vector3(i * size, 0, 0);
        }
        startPos = transform.position.x;
    }

    // Update is called once per frame
    void Update()
    {
        // transform.Translate(new Vector3((float)Decimal.Divide((decimal)playerRigidbody.velocity.x, (decimal)ratio) * Time.deltaTime, 0, 0), Space.World);
        transform.position = new Vector3(startPos + cam.transform.position.x*ratio, transform.position.y, transform.position.z);
    }
}
