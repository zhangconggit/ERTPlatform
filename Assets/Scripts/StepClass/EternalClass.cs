using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

    /// <summary>
    /// 永恒工作类
    /// </summary>
    public class EternalClass : StepBase
    {
       
        public EternalClass()
        {

        }
        /// <summary>
        /// 任意步骤的开始都会先执行这里
        /// </summary>
        public override void stepBefore()
        {
            
        }
        /// <summary>
        /// 一直工作
        /// </summary>
        public override void stepRunner()
        {

        }
        /// <summary> 
        /// 任意步骤的结束都会先执行这里
        /// </summary>
        public override void stepAfter()
        {
            
        }

        public override void stepReSet()
        {
            base.stepReSet();
        }
        public override void stepSetEnd()
        {
        }

    }