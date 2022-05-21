using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class tiendaScript : MonoBehaviour
{
    private screenManager sm;
    public Button botonBack;

    void Start()
    {
        botonBack.onClick.AddListener(irHome);
        sm = transform.parent.parent.GetComponent<screenManager>();
    }

    private void irHome()
    {
        sm.switchScreens(this.name, "Home");
    }
}
