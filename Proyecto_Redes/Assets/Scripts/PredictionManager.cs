using FishNet.Object;
using FishNet.Observing;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PredictionManager : MonoBehaviour
{
    //El prediction manager es para 2d, para convertir en 3d, solo hay que remover los "2d" del codigo.
    Scene currentScene;
    Scene predictionScene;

    PhysicsScene2D currentPhysicsScene;
    PhysicsScene2D predictionPhysicsScene;

    void Start()
    {
        Physics2D.simulationMode = SimulationMode2D.Script;

        currentScene = SceneManager.GetActiveScene();
        currentPhysicsScene = currentScene.GetPhysicsScene2D();

        CreateSceneParameters parameters = new CreateSceneParameters(LocalPhysicsMode.Physics2D);
        predictionScene = SceneManager.CreateScene("Prediction", parameters);
        predictionPhysicsScene = predictionScene.GetPhysicsScene2D();
    }

    void FixedUpdate()
    {
        if (currentPhysicsScene.IsValid())
        {
            currentPhysicsScene.Simulate(Time.fixedDeltaTime);
        }
    }

    GameObject AddDummyObjectToPhysicsScene(GameObject _go)
    {
        GameObject dummyGo = Instantiate(_go);
        /*dummyGo.transform.position = _go.transform.position;
        dummyGo.transform.rotation = _go.transform.rotation;*/
        Renderer fakeRender = dummyGo.GetComponent<Renderer>();
        if (fakeRender)
        {
            fakeRender.enabled = false;
        }

        SceneManager.MoveGameObjectToScene(dummyGo, predictionScene);
        return dummyGo;
    }

    public (Vector2, Vector2) Predict(GameObject _go, Vector2 _velocity, int _steps)
    {
        if (!currentPhysicsScene.IsValid() || !predictionPhysicsScene.IsValid())
        {
            Debug.LogError("Una escena no es valida");
        }

        GameObject dummy = AddDummyObjectToPhysicsScene(_go);
        Destroy(dummy.GetComponent<NetworkObserver>());
        Destroy(dummy.GetComponent<NetworkObject>());
        dummy.SetActive(true);
        Rigidbody2D dummyRigid2d = dummy.GetComponent<Rigidbody2D>();
        dummyRigid2d.velocity = _velocity;

        for (int i = 0; i <= _steps; i++)
        {
            predictionPhysicsScene.Simulate(Time.fixedDeltaTime);
            // dummyRigid2d.position // CUrrent position of this step
        }

        Vector2 lastPosition = dummyRigid2d.position;
        Vector2 lastVelocity = dummyRigid2d.velocity;
        Destroy(dummy);
        return (lastPosition, lastVelocity);
    }
}