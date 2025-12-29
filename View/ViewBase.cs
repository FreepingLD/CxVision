using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;

namespace View
{
    [Serializable]
    public class ViewBase : IView
    {
        public virtual object GetViewParam(enViewParamType paramType)
        {
            throw new NotImplementedException();
        }
        public virtual void MoveObject(object Object, object Tx, object Ty)
        {
            throw new NotImplementedException();
        }
        public virtual void DisplayObject(params object []Object)
        {
            throw new NotImplementedException();
        }
        public virtual void DisplayObject(object Object)
        {
            throw new NotImplementedException();
        }
        public virtual void DisplayObject(object Object1, object Object2)
        {
            throw new NotImplementedException();
        }
        public virtual void ZoomObject(object Object, object CenterX, object CenterY, double scale)
        {
            throw new NotImplementedException();
        }
        public virtual void View3D(object Object)
        {
            throw new NotImplementedException();
        }
        public virtual void InitView(object view)
        {
            throw new NotImplementedException();
        }
        public virtual void ClearView()
        {
            throw new NotImplementedException();
        }
        public virtual void SetViewParam(enViewParamType paramType, object paramValue)
        {
            throw new NotImplementedException();
        }

    }
}
