using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardFOV : MonoBehaviour
{

    public float fov = 90f;
    public int rayCount = 2;
    public float ViewDistance = 50f;
    public float meshHeight = 0.5f;
    public Transform guardTransform;
    public MoveTo guard;
    public LayerMask layerMask;

    Vector3 origin;
    
    float angleIncrease;
    Vector3[] vertices;
    Vector2[] uv;
    int[] triangles;
    Mesh mesh;
    bool isServer;
    

    // Start is called before the first frame update
    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        GetComponent<MeshCollider>().sharedMesh = mesh;
        // isServer = guard.GetComponent<GuardNetworkBehaviour>().isServer;
    }

    // Update is called once per frame
    void Update()
    {

        float angle = GetAngleFromVector(guardTransform.forward)+fov/2;//offset the angel to center 
        angleIncrease = fov/rayCount;
        vertices = new Vector3[rayCount+1+1];
        uv = new Vector2[vertices.Length];
        triangles = new int[rayCount*3];
        setOrigin(new Vector3(guardTransform.position.x , meshHeight, guardTransform.position.z));

        vertices[0] = origin;

        int vertexIndex = 1;
        int triangleIndex = 0;
        for(int i = 0; i<=rayCount; i++){
            Vector3 vertex;
            // RaycastHit raycastHit = Physics.Raycast(origin, GetVectorFromAngle(angle), ViewDistance);
            RaycastHit raycastHit; 
            Physics.Raycast(origin, GetVectorFromAngle(angle), out raycastHit, ViewDistance, layerMask);
            Debug.DrawRay(origin, GetVectorFromAngle(angle)* ViewDistance, Color.cyan);

            // print(raycastHit.collider);
            if(raycastHit.collider == null){
                vertex = origin + GetVectorFromAngle(angle) * ViewDistance;
            }
            else {
                vertex = raycastHit.point;
                if (checkAlert(raycastHit.collider.gameObject) && guard.hasAuthority) { //TODO: clean the null part up
                    // print(guard);
                    guard.Alert(raycastHit.collider.gameObject);
                }
            }

            vertices[vertexIndex] = vertex;

            if(i>0)
            {
                //connect our triangles
                triangles[triangleIndex + 0] = 0;
                triangles[triangleIndex + 1] = vertexIndex - 1;
                triangles[triangleIndex + 2] = vertexIndex;

                triangleIndex += 3;
            }

            vertexIndex ++;
            angle -= angleIncrease; //increasing an angle in unity goes counter clockwise and we want to go clockwise, so we subtract
        }
        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateBounds();
    }
    
    void setOrigin(Vector3 newOrigin){
        origin = newOrigin;
    }

    bool checkAlert(GameObject obj){
        // print(obj.tag);

        if(obj.tag == "Player") return true;
        return false;
    }

    Vector3 GetVectorFromAngle(float angle){
        float angleRad = angle * (Mathf.PI/180f);
        return new Vector3(Mathf.Cos(angleRad), 0, Mathf.Sin(angleRad));
    }
    
    float GetAngleFromVector(Vector3 dir){
        dir = dir.normalized;
        float n = (Mathf.Atan2(dir.z, dir.x)) * Mathf.Rad2Deg; //FIX THIS
        if(n < 0) n+=360;
        return n;
        // return Vector3.Angle(dir, Vector3.forward) +135;
    }
}
