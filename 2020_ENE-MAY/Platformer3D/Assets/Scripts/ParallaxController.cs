using UnityEngine;

public class ParallaxController : MonoBehaviour
{
  public ParallaxLayer[] layers;
  [SerializeField]
  private bool _moveVertical = true;
  [SerializeField]
  private float _verticalDamp = 1;
  private float _yPosVelocity;
  private float _wrapperOffset;
  private float _deltaX;
  private float _deltaY;
  private Vector2[] _layersWidth;
  private Vector3 _lastPosition;
  private Transform _camera;
  private GameObject[][] _parallaxLayers;

  private void Start()
  {
    if (layers.Length == 0) return;

    _camera = Camera.main.transform;
    _lastPosition = _camera.position;
    _wrapperOffset = Mathf.Abs(transform.position.y - _camera.position.y);

    ContainerPositionUpdate();

    CreateLanes();
  }

  private void CreateLanes()
  {
    _layersWidth = new Vector2[layers.Length];
    _parallaxLayers = new GameObject[layers.Length][];
    for (int i = 0; i < layers.Length; i++)
    {
      _layersWidth[i] = new Vector2(layers[i].asset.GetComponent<MeshRenderer>().bounds.size.x, 0);
      _layersWidth[i].y = _layersWidth[i].x / 2;
      _parallaxLayers[i] = CreateLane(layers[i].asset, _layersWidth[i].x);
      Destroy(layers[i].asset);
    }
  }

  private GameObject[] CreateLane(GameObject asset, float blockWidth)
  {
    Vector3 position = new Vector3(0, asset.transform.position.y, asset.transform.position.z);
    GameObject[] lane = new GameObject[3];

    for (int i = 0; i < 3; i++)
    {
      position.x = blockWidth * (i - 1);
      GameObject newBlock = Instantiate(asset);
      newBlock.transform.parent = transform;
      newBlock.transform.localPosition = position;
      lane[i] = newBlock;
    }

    return lane;
  }

  private void LateUpdate()
  {
    UpdateDeltaValues();
    ContainerPositionUpdate();
    BlocksPositionUpdate();
  }

  private void UpdateDeltaValues()
  {
    _deltaX = _camera.position.x - _lastPosition.x;
    _deltaY = _camera.position.y - _lastPosition.y;

    _lastPosition = _camera.position;
  }

  private void ContainerPositionUpdate()
  {
    Vector3 newPosition = transform.position;
    newPosition.x = _lastPosition.x;

    if (_moveVertical) newPosition.y = Mathf.SmoothDamp(newPosition.y, _camera.position.y - _wrapperOffset, ref _yPosVelocity, _verticalDamp);

    transform.position = newPosition;
  }

  private void BlocksPositionUpdate()
  {
    if (_deltaX == 0) return;

    Vector3 position;

    for (int lane = 0; lane < _parallaxLayers.Length; lane++)
    {
      for (int i = 0; i < 3; i++)
      {
        position = _parallaxLayers[lane][i].transform.localPosition;
        position.x -= _deltaX * layers[lane].speed;
        _parallaxLayers[lane][i].transform.localPosition = position;
      }

      if (_deltaX > 0) _parallaxLayers[lane] = RightDisplacement(_parallaxLayers[lane], _layersWidth[lane]);
      else if (_deltaX < 0) _parallaxLayers[lane] = LeftDisplacement(_parallaxLayers[lane], _layersWidth[lane]);
    }
  }

  private GameObject[] LeftDisplacement(GameObject[] lane, Vector2 size)
  {
    if (lane[0].transform.localPosition.x > -size.y)
    {
      Vector3 positionSwapper = lane[0].transform.localPosition;
      positionSwapper.x -= size.x;
      lane[2].transform.localPosition = positionSwapper;

      GameObject temp = lane[2];
      lane[2] = lane[1];
      lane[1] = lane[0];
      lane[0] = temp;
    }

    return lane;
  }

  private GameObject[] RightDisplacement(GameObject[] lane, Vector2 size)
  {
    if (lane[2].transform.localPosition.x < size.y)
    {
      Vector3 positionSwapper = lane[2].transform.localPosition;
      positionSwapper.x += size.x;
      lane[0].transform.localPosition = positionSwapper;

      GameObject temp = lane[0];
      lane[0] = lane[1];
      lane[1] = lane[2];
      lane[2] = temp;
    }

    return lane;
  }
}

[System.Serializable]
public class ParallaxLayer
{
  public GameObject asset;
  public float speed = 0;
}
