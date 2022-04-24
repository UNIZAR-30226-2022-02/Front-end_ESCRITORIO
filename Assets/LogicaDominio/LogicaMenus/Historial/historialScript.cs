using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class historialScript : MonoBehaviour
{
    public Button botonBack;


    private screenManager sm;
    
    //private List<Resultados> historial;

    void Start()
    {
        botonBack.onClick.AddListener(irHome);
        sm = transform.parent.GetComponent<screenManager>();
    }

    private void irHome()
    {
        sm.switchScreens(this.name, "Home");
    }


}
