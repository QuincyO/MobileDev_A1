using System.Collections;
using System.Collections.Generic;
using Quincy.Structs;
using UnityEngine;

public class Cloud : MonoBehaviour
{
    

    public Boundary Bounds;
    public float CloudSpeed;

    private Vector3 direction = Vector3.left;

    [SerializeField] bool MoveLeft = true;

    public enum SpeedEnum
    {
        None,
        Slow,
        Medium,
        Fast
    }

    public SpeedEnum Speed;
    

    // Start is called before the first frame update
    void Start()
    {
        Reset();
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        CheckOutOfBounds();
    }

    private void Move()
    {
        transform.Translate(direction * (Time.deltaTime * CloudSpeed));
    }

    bool CheckOutOfBounds()
    {
        if (transform.position.x < Bounds.x.min || transform.position.x > Bounds.x.max ||
            transform.position.y < Bounds.y.min || transform.position.y > Bounds.y.max)
        {

            Reset();
            return true;
        }

        return false;
    }


    void OnEnable()
    {
        Reposition();
    }
    void Reposition()
    {
        MoveLeft = Random.Range(0, 2) == 1;
        #region Set Left or Right Direction
        direction = MoveLeft?  Vector3.left:Vector3.right;
        transform.rotation = MoveLeft? Quaternion.Euler(0,180,0) : Quaternion.Euler(0,0,0);
        var xPos = MoveLeft ? Bounds.x.min : Bounds.x.max;
        #endregion

        float scaleFactor;
        float yPosFactor;
        switch (Speed)
        {
            case SpeedEnum.None:
                return;
            case SpeedEnum.Slow:
                scaleFactor = Random.Range(0.5f, 0.8f);
                yPosFactor = Random.Range(.75f, 1f);
                break;
            case SpeedEnum.Medium:
                scaleFactor = Random.Range(0.8f, 1.2f);
                yPosFactor = Random.Range(.44f, .75f);
                break;
            case SpeedEnum.Fast:
                scaleFactor = Random.Range(1.2f, 1.5f);
                yPosFactor = Random.Range(.15f, 0.44f);
                break;
            default:
                scaleFactor = 1f;
                yPosFactor = 1f;
                return;
        }



        var yPos = Bounds.y.min + Bounds.y.max * yPosFactor;
        transform.position = new Vector3(xPos, yPos, 0);
        
        transform.localScale = new Vector3(scaleFactor, scaleFactor, scaleFactor);
    }
    public void Reset()
    {


        
        CloudSpawner.Instance.ReturnToPool((this,gameObject));
        gameObject.SetActive(false);
    }
}
