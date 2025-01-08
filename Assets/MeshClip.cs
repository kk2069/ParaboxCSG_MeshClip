using Parabox.CSG;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshClip : MonoBehaviour
{
    public GameObject cubeObj;

    public GameObject sphereObj;

    void Start()
    {
        //GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
        //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        //sphere.transform.localScale = Vector3.one * 1.3f;
        //sphere.transform.position = new Vector3(0, 1, 0);


        //// Perform boolean operation
        ////Model result = CSG.Subtract(cube, sphere);


        ////Model result = CSG.Intersect(cube, sphere);

        //Model result = CSG.Union(cube, sphere);

        ////Model result = CSG.Perform( CSG.BooleanOp.Subtraction, cube, sphere);


        //// Create a gameObject to render the result
        //var composite = new GameObject();
        //composite.AddComponent<MeshFilter>().sharedMesh = result.mesh;
        //composite.AddComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();

        //composite.transform.position = new Vector3(3, 0, 0);

        ////GameObject.DestroyImmediate(cube);
        ////GameObject.DestroyImmediate(sphere);

    }

   

    void Update()
    {
        //相机射线选取世界中的Collider
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            var result = Physics.RaycastAll(ray, 1000f);
            for (int i = 0; i < result.Length; i++)
            {
                var hit = result[i];
                if (hit.transform.tag == "Player")
                {
                    //Debug.Log("选中了墙");
                    sphereObj.transform.position = hit.point;
                    sphereObj.transform.rotation = hit.transform.rotation;

                    Model meshResult = CSG.Subtract(cubeObj, sphereObj);


                    // Create a gameObject to render the result

                    GameObject composite = new GameObject();

                    composite.AddComponent<MeshFilter>().sharedMesh = meshResult.mesh;
                    composite.AddComponent<MeshRenderer>().sharedMaterials = meshResult.materials.ToArray();

                    //composite.transform.position = new Vector3(3, 0, 0);

                    DestroyImmediate(cubeObj);
                    DestroyImmediate(sphereObj);
                }

            }
        }
    }

    public static Transform ClipModel(Transform original, Transform clip, bool remainClip = false)
    {
        if (original == null || !original.gameObject.activeInHierarchy) return null;
        if (clip == null || !clip.gameObject.activeInHierarchy) return original;
        var oriRender = original.GetComponent<MeshRenderer>();
        var clipRender = clip.GetComponent<MeshRenderer>();
        if (oriRender == null || clipRender == null) return null;
        var oriTrans = oriRender.transform;
        var clipTrans = clipRender.transform;
        Material[] signMats = null;
        if (remainClip)
        {
            signMats = clipRender.sharedMaterials;
            clipRender.sharedMaterials = oriRender.sharedMaterials;
        }
        var result = CSG.Subtract(oriTrans.gameObject, clipTrans.gameObject);
        if (remainClip) clipRender.sharedMaterials = signMats;
        //result.convertMeshToLocal(oriTrans);
        Mesh mesh = result.mesh;
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        oriTrans.GetComponent<MeshFilter>().sharedMesh = mesh;
        oriTrans.GetComponent<MeshRenderer>().sharedMaterials = result.materials.ToArray();
        return original;
    }



}
