
using UnityEngine;
using UnityEngine.UI;
public class PlayMoveCtr : MonoBehaviour
{
    /// <summary>
    /// 角色控制器
    /// </summary>
    private CharacterController _player;

    /// <summary>
    /// plane
    /// </summary>
    public GameObject terrain;
    /// <summary>
    /// 由于plane的长度未知所以通过代码获取各个方向上的长度，
    /// 虽然本程序plane是单位plane
    /// </summary>
    float _xlength;
    float _ylength;
    float _zlength;
    // Use this for initialization
    void Start ()
	{
	    _player = GetComponent<CharacterController>();
	    Vector3 length = GetComponent<MeshFilter>().mesh.bounds.size;
	    _xlength = length.x * transform.lossyScale.x;
	    _ylength = length.y * transform.lossyScale.y;
	    _zlength = length.z * transform.lossyScale.z;
    }
    /// <summary>
    /// 目标点坐标
    /// </summary>
    private Vector3 _targetpos;

    /// <summary>
    /// 方向向量
    /// </summary>
    private Vector3 _direction;
    /// <summary>
    /// 旋转四元数
    /// </summary>
    private Quaternion _dirRotation;

    /// <summary>
    /// 箭头对象
    /// </summary>
    public Image Arrow;

    // Update is called once per frame
    void Update () {
        //鼠标抬起时触发移动
	    if (Input.GetMouseButtonUp(0))
	    {
            //射线检测点击到的点
	        Ray hit=Camera.main.ScreenPointToRay(Input.mousePosition);
	        RaycastHit hitpoint;

	        if (Physics.Raycast(hit,out hitpoint))
	        {
                //移动的目标点为鼠标点击到的点
	            _targetpos = hitpoint.point;
                //获取移动的方向并归一化
	            _direction =new Vector3(_targetpos.x- transform.position.x,0, _targetpos.z - transform.position.z).normalized;
	        }
            //得到旋转四元数
	        _dirRotation = Quaternion.LookRotation(_direction);

	    }
        //当目标点为zero和距离目标点距离小于0.1(假如设置==0的话移动到目标点可能会一直抖动)时不移动
	    if (_targetpos!=Vector3.zero&&Vector3.Distance(new Vector3(_targetpos.x,0, _targetpos.z),new Vector3(transform.position.x,0,transform.position.z))>0.1)
	    {
            //插值旋转
            transform.rotation=Quaternion.Lerp(transform.rotation, _dirRotation, 10f * Time.deltaTime);
            //玩家控制器移动(我把速度设置为10大家可以把10公开出来自己调整)
	        _player.Move(_direction * 10 * Time.deltaTime);
            //由于箭头要指向玩家面朝方向所以y值取复数
	        Arrow.transform.rotation = new Quaternion(transform.rotation.x, transform.rotation.z,-transform.rotation.y, transform.rotation.w);
            //根据比例获取当前箭头所处位置(cube的x轴坐标/plane长度=arrow的x轴坐标/地图的x轴长度，y轴同理)计算得知当前箭头的坐标
            Arrow.transform.localPosition = new Vector3(transform.position.x/ _xlength*Arrow.GetComponent<RectTransform>().rect.x,0, transform.position.z / _zlength * Arrow.GetComponent<RectTransform>().rect.y);
        }
	}
}
