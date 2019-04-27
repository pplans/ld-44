using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BLoodSpread : MonoBehaviour
{
	private Material m_material;
    // Start is called before the first frame update
    void Start()
    {
		m_material = GetComponent<Material> ();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
