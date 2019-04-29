using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstantiateRendererMat : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material = Instantiate<Material>(GetComponent<Renderer>().material);
    }
}
