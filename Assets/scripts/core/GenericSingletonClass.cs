using UnityEngine;

 public class GenericSingletonClass<Instance> : MonoBehaviour where Instance : GenericSingletonClass<Instance>
{
    public static Instance instance;
     public bool isPersistant;
     
     public virtual void Awake() {
         if(isPersistant) {
             if(!instance) {
                 instance = this as Instance;
             }
             else {
                 Destroy(gameObject);
             }
             DontDestroyOnLoad(gameObject);
            Initialize();
         }
         else {
             instance = this as Instance;
         }
     }

     public virtual void Initialize(){}
}