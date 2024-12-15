using IST360Tools;
using UnityEngine;

public class ChangeColor : CustomActionScript
{
    
    public Material StartMaterial;
    public Material EndMaterial;
    public GameObject Target;

    public void ChangeColors()
    {
      Target.GetComponent<Renderer>().material = EndMaterial;
    }
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Target.GetComponent<Renderer>().material = StartMaterial;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
}
