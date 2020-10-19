using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColor : MonoBehaviour
{

    public MeshRenderer MeshRend;
    private Color[] color = { new Color(255, 0, 0, 1),
                                new Color(255, 128, 0, 1),
                                new Color(255, 255, 1, 1),
                                new Color(128, 255, 1, 1),
                                new Color(0, 255, 0, 1),
                                new Color(0, 255, 128, 1),
                                new Color(0, 255, 255, 1),
                                new Color(0, 128, 255, 1),
                                new Color(0, 0, 255, 1) };
    private int randNumber;
    // Start is called before the first frame update
    void Awake()
    {
        MeshRend = GetComponent<MeshRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        randNumber = Random.Range(0, 9);
        if (MeshRend != null)
        {
            MeshRend.material.color = color[randNumber];
        }
    }

}
