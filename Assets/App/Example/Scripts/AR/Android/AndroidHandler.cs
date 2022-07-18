using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class AndroidHandler : MonoBehaviour
    {
        //private AndroidJavaObject javaObject;//MainActivity����

        // [SerializeField]
        //private Text messageText;//unity������ʾText
        //[SerializeField]
        // private Text receiveMessageText;//unity������ʾText

        // Start is called before the first frame update
        void Start()
        {
            //�ڶ���
            //AndroidJavaClass android = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            //javaObject = android.GetStatic<AndroidJavaObject>("currentActivity");
        }
        public void OnClickToJavaFunSum()
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.MainActivity");
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("GetInstance");
            string text = jo.Call<int>("Sum", 5, 3).ToString();
        }


        //unity�¼�������com.unity3d.player.MainActivityΪ�����࣬ʵ��OnUnityFinished�ӿڡ�
        //OnUnityMethod����ͨ��native��ʽ��ȡ��MainActivity���󣬵��õ���android�˵�OnUnityFinished������ʵ�ֲ�������
        public void OnUnityMethod(string msg)
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.MainActivity");
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("GetInstance");
            string text = jo.Call<int>("OnUnityFinished", msg).ToString();
        }

        //��������android�Ĳ���
        /**android�˵��÷���
        String receiveObj = "MainCamera";//unity�нű����ص�Object��name
        String receiveMethod = "Receive";//unity��Object���ؽű��ķ�����
        String params = str + " Android Call Unity.";//����Ҫ���Ĳ���
        //android����unity��
        UnityPlayer.UnitySendMessage(receiveObj, receiveMethod, params);
        */
        public void Receive(string str)
        {
            //receiveMessageText.text = str;//strΪ��android�˴�����������
        }

        /**
         * 
        public void OnClickCall1()//android����unity������ʾ����unity����android��CallUnityFun������ͨ��UnityPlayer.UnitySendMessage��unity������Ϣ
        {
            AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.MainActivity");//MainActivity�̳�UnityPlayerActivity
            AndroidJavaObject jo = jc.CallStatic<AndroidJavaObject>("GetInstance");//GetInstance������ȡ�ĵ�ǰ����
            jo.Call("CallUnityFun", "Test2");//����android�˷���CallUnityFun
        }
        public void OnClickCall2()
        {
            // javaObject.Call("CallUnityFun", "Test1");//����android�˷���CallUnityFun
        }

        */
    }
}