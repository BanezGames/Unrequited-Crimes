using UnityEngine;

[CreateAssetMenu(fileName = "itemData", menuName = "Scriptable Objects/itemData")]
public class itemData : ScriptableObject
{
    //public GameObject heldModel;
    public string itemName;
    public Sprite itemDisplay;
    public Mesh itemMesh;
    public Material itemMaterial;

    //public AudioClip[] itemSound;
    //[Range(0,1)] public float itemSoundVol;
}
