using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DissolveExample
{
    public class DissolveChilds : MonoBehaviour
    {
        // Start is called before the first frame update
        List<Material> materials = new List<Material>();
        //bool PingPong = false;
        void Start()
        {
            var renders = GetComponentsInChildren<Renderer>();
            for (int i = 0; i < renders.Length; i++)
            {
                materials.AddRange(renders[i].materials);
            }
        }

        private void Reset()
        {
            Start();
            SetValue(0);
        }

        // Update is called once per frame
        void Update()
        {

            var value = Mathf.PingPong(Time.time * 0.5f, 1f);
            SetValue(value);
            Debug.Log(value);
        }

        IEnumerator enumerator()
        {
            float value = 0;
            while (value <= 1)
            {
                Mathf.PingPong(value, 1f);
                value += Time.deltaTime;
                SetValue(value);
                yield return new WaitForEndOfFrame();
            }
        }

        public void SetValue(float value)
        {
            for (int i = 0; i < materials.Count; i++)
            {
                materials[i].SetFloat("_Dissolve", value);
            }
        }
    }
}