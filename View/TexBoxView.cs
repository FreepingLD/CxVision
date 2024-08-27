using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace View
{
    public class TextBoxView : ViewBase
    {
        TextBox textBox;
        private string viewName;
        public TextBoxView(TextBox textBox,string name)
        {
            this.textBox = textBox;
            this.viewName = name;
        }

        #region 实现接口
        public override void DisplayObject( object  Object)
        {
            UpdataView((double [])Object);
        }
        public override void SetViewParam(enViewParamType paramType, object paramValue)
        {
            try
            {
                switch (paramType)
                {
                    default:
                        break;
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        public override object GetViewParam(enViewParamType paramType)
        {
            try
            {
                // 只有更改属性才会进入属性访问器，所在这里只能用属性，属性是对字段的包装
                switch (paramType)
                {
                    case enViewParamType.视图名字:
                        return this.viewName;
                    default:
                        return null;
                }
            }
            catch
            {
                throw new Exception();
            }
        }

        public override void InitView(object view)
        {

        }

        #endregion


        private void UpdataView(double[] data)
        {
            if (data != null && data.Length > 0)
                this.textBox.Invoke(new Action(() => this.textBox.Text = data[0].ToString()));
            else
                this.textBox.Invoke(new Action(() => this.textBox.Text = (-1024.0).ToString()));

            // 始终取反 GlobalVariable.pConfig.MeasureRange - 
        }
    }
}
