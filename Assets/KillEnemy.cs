using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemy : MonoBehaviour
{
<<<<<<< Updated upstream
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
=======
>>>>>>> Stashed changes
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Enemy")
        {
<<<<<<< Updated upstream
            other.gameObject.GetComponent<IADamage>().lives = -1;
        }
        
    }

=======
            other.GetComponent<IADamage>().lives = -1;
        }
    }
>>>>>>> Stashed changes
}
