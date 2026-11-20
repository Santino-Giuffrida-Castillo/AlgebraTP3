using UnityEngine;
using System.Collections.Generic;


public class Movement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float rotationSpeed = 90f;
    [SerializeField] private float scaleSpeed = 1f;
    private Vector3 _movementInput;

    [SerializeField] private List<GameObject> shapes;

    private int inControl = 0;


    private void Update()
    {
        HandleInput();
        HandleChangeControl();
        HandleMovement();
        HandleRotation();
        HandleScale();
    }
    
    
    private void HandleInput()
    {
        float yMovement = 0;
        //Voy para arriva
        if (Input.GetKey(KeyCode.Space))
        {
            yMovement = 1;
        }
        //Voy para abajo
        if (Input.GetKey(KeyCode.LeftControl))
        {
            yMovement = -1;
        }
        
        float zMovement = Input.GetAxisRaw("Vertical");
        float xMovement = Input.GetAxisRaw("Horizontal");
        
        _movementInput = new Vector3
        (
            xMovement,
            yMovement,
            zMovement
        ).normalized;
    }

    private void HandleMovement()
    {
        
        Vector3 movementVelocity = _movementInput * speed * Time.deltaTime;
        shapes[inControl].gameObject.transform.position += movementVelocity;
    }

        private void HandleRotation()
    {
        Transform trans = shapes[inControl].transform;
        float turn = rotationSpeed * Time.deltaTime;

        // Z / X rotan en el eje Y
        if (Input.GetKey(KeyCode.Z))
        {
            trans.Rotate(0, turn, 0, Space.World);
        } 
        if (Input.GetKey(KeyCode.X))
        {
            trans.Rotate(0, -turn, 0, Space.World);
        } 
        // C / V rotan en el eje X
        if (Input.GetKey(KeyCode.C))
        {
            trans.Rotate(turn, 0, 0, Space.World);
        } 
        if (Input.GetKey(KeyCode.V))
        {
            trans.Rotate(-turn, 0, 0, Space.World);
        } 
    }
        private void HandleScale()
    {
        Transform trans = shapes[inControl].transform;
        float change = scaleSpeed * Time.deltaTime;

        // R para agrandarlo 
        if (Input.GetKey(KeyCode.R))
        {
            trans.localScale += new Vector3(change, change, change);
        }
        // F para hacerlo mas chiquito 
        if (Input.GetKey(KeyCode.F) && trans.localScale.x > 0.2f)
        {
            trans.localScale -= new Vector3(change, change, change);
        }
    }
    private void HandleChangeControl()
    {   
        //cambio a otro modelo
        if (Input.GetKeyDown(KeyCode.Q))
        {
            inControl = (inControl + 1) % shapes.Count;
        }
        //cambio a otro modelo pero por la derecha 
        if (Input.GetKeyDown(KeyCode.E))
        {
            inControl = (inControl - 1 + shapes.Count) % shapes.Count;
        }
    }
}
