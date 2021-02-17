using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject _trunkPrefab;
    [SerializeField] int minTrunks, maxTrunks;
    [SerializeField] int minTrees, maxTrees;
    [SerializeField] AnimationClip stemGrowthClip;
    [SerializeField] GameObject floorPlane;
    [SerializeField] CameraController cameraController;
    [SerializeField] List<Tree> forest = new List<Tree>();

    BoxCollider floorPlaneCollider;
    Vector3 newTrunkPosition, trunkSize;
    int currentTreeIndex;

    private void Start()
    {
        trunkSize = _trunkPrefab.GetComponent<BoxCollider>().size;
        floorPlaneCollider = floorPlane.GetComponent<BoxCollider>();

        for (int i = 0; i < Random.Range(minTrees, maxTrees); i++) // Creates random amount of trees. Can be choosed min/max from inspector
        {
            StartCoroutine("CreateTreeOnPlane");
        }
        StartCoroutine("StartDelay");
    }

    IEnumerator CreateTreeOnPlane()
    {

        yield return new WaitForSeconds(Random.Range(0, 2));
        float randomXPos = Random.Range(floorPlaneCollider.bounds.min.x, floorPlaneCollider.bounds.max.x); 
        float randomZPos = Random.Range(floorPlaneCollider.bounds.min.z, floorPlaneCollider.bounds.max.z); // Random points on plane floor

        Vector3 trunkPreviousPosition = new Vector3(randomXPos, floorPlane.transform.position.y - trunkSize.y, randomZPos);
        Tree newTree = new Tree(); // Creates a tree object in list of trees - forest.
        forest.Add(newTree);

        for (int i = 0; i < RandomTrunkAmount(); i++) // Creates trunks GameObjects position it right and adds it to Tree object.
        {
            newTrunkPosition = trunkPreviousPosition;
            newTrunkPosition.y += trunkSize.y;
            trunkPreviousPosition = newTrunkPosition;
            GameObject trunk = Instantiate(_trunkPrefab, newTrunkPosition, Quaternion.identity);
            trunk.transform.RotateAround(trunk.GetComponent<BoxCollider>().bounds.center, Vector3.up, Random.Range(-180, 180)); // Creates random rotation of a trunk
            forest[forest.IndexOf(newTree)].AddTrunkToTree(trunk);
            yield return new WaitForSeconds(stemGrowthClip.length / trunk.GetComponent<Animator>().GetFloat("growthAnim1Speed")); // Waits for how long 1st part of animation is. Speed of animation can be changed.
        }

        foreach (GameObject trunk in forest[forest.IndexOf(newTree)].GetTreeTrunks()) // Waits for the stem to sprout and then start to grow.
        {
            trunk.GetComponent<Animator>().SetBool("stemGrown", true);
        }
    }

    int RandomTrunkAmount()
    {
        int trunkAmount = Random.Range(minTrunks, maxTrunks + 1);
        return trunkAmount;
    }

    public void DestroyTrunk(GameObject trunkToDestroy)
    {
        foreach (Tree tree in forest)
        {
            if (tree.GetTreeTrunks().Contains(trunkToDestroy)) // Checks which tree contain trunk to destroy
            {
                Destroy(tree.GetTrunkFromTree(0)); // Destoys the trunk
                tree.GetTreeTrunks().RemoveAt(0); // Removering trunk from tree object
                if (tree.GetTreeTrunks().Count != 0) // Changing position of all trunks 1 step down.
                {
                    foreach (GameObject trunk in tree.GetTreeTrunks())
                    {
                        Vector3 newPosition = new Vector3(trunk.transform.position.x, trunk.transform.position.y - trunkSize.y, trunk.transform.position.z);
                        trunk.transform.position = newPosition;
                    }
                }
                else // If there is no more trunk, removes a tree object from the forest.
                {
                    forest.Remove(tree);
                    if (forest.Count > currentTreeIndex)
                        cameraController.SetNewCameraDestanation(forest[currentTreeIndex].GetTrunkFromTree(0).transform.GetChild(0).GetComponent<Renderer>().bounds.center);
                }
                break;
            }
        }
    }

    public void LookAtNextTree()
    {
        if (forest.Count > currentTreeIndex + 1) currentTreeIndex++; // Check if there is more tree to look.
        // Passing to CameraController position of 1 trunk in tree.
        cameraController.SetNewCameraDestanation(forest[currentTreeIndex].GetTrunkFromTree(0).transform.GetChild(0).GetComponent<Renderer>().bounds.center); 
    }
    public void LookAtPreviousTree()
    {
        if (currentTreeIndex > 0) // Check if there is more tree to look.
            currentTreeIndex--;
        cameraController.SetNewCameraDestanation(forest[currentTreeIndex].GetTrunkFromTree(0).transform.GetChild(0).GetComponent<Renderer>().bounds.center);
    }
    IEnumerator StartDelay() // Creates small pause to look how trees growing.
    {
        yield return new WaitForSeconds(2.5f);
        cameraController.SetNewCameraDestanation(forest[0].GetTrunkFromTree(0).transform.GetChild(0).GetComponent<Renderer>().bounds.center);
    }

}
