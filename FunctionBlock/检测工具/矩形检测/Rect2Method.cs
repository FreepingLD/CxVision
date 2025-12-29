using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using HalconDotNet;


namespace FunctionBlock
{

    [Serializable]
    public class Rect2Method
    {

        public static bool DetectRect2(HImage hImage, Rect2DetectParam param, out HRegion  hRegionNg)
        {
            bool result = false;
            hRegionNg = new HRegion();
            if (hImage == null )
            {
                throw new ArgumentNullException("hImage");
            }
            if (param == null)
            {
                throw new ArgumentNullException("param");
            }
            


            return result;

        }

        /// <summary>
        /// 检测缺口
        /// </summary>
        /// <param name="hImage"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static HRegion DetectGap(HImage hImage, Rect2DetectParam param)
        {
            HRegion hRegionGap = new HRegion();

            /// 分割出产品区域来
            HRegion hRegionBlob =  hImage.Threshold(param.LowTh, param.HighTh);
            HRegion hRegionFillUp =  hRegionBlob.FillUp();
            HRegion hRegionClosing = hRegionFillUp.ClosingRectangle1(param.ClosingWidth, param.ClosingHeight);
            HRegion hRegionOpening = hRegionFillUp.OpeningRectangle1(param.OpenWidth, param.OpenHeight);
            HRegion hRegionConnect = hRegionOpening.Connection();
            HRegion hRegionSelect = hRegionConnect.SelectShapeStd("max_area", 70);
            hRegionBlob?.Dispose();
            hRegionFillUp?.Dispose();
            hRegionClosing?.Dispose();
            hRegionOpening?.Dispose();
            hRegionConnect?.Dispose();
            ////// 分割检测区域
            HRegion RoiHregion = new HRegion();
            RoiHregion.GenEmptyRegion();
            foreach (var item in param.RoiParam)
            {
                RoiHregion = RoiHregion.ConcatObj(item.RoiShape.GetRegion());
            }
            HImage reduceImage = hImage.ReduceDomain(RoiHregion.Union1());
            RoiHregion.Dispose();
            hRegionBlob = reduceImage.Threshold(param.LowTh, param.HighTh);
            hRegionFillUp = hRegionBlob.FillUp(); // 将检测区域内的脏污屏蔽

            return hRegionGap;
        }

        /// <summary>
        /// 破片检测
        /// </summary>
        /// <param name="hRegion"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static HRegion GapDetect(HRegion hRegion, Rect2DetectParam param)
        {
            HRegion hRegionGap = new HRegion();
            if (hRegion == null)
                throw new ArgumentNullException("hRegion");
            if (param == null)
                throw new ArgumentNullException("param");
            int row1, col1, row2, col2;
            HRegion hRegion1Union = hRegion.Union1();
            hRegion1Union.SmallestRectangle1(out row1, out col1, out row2, out col2);
            HRegion hRegionRect1 = new HRegion(row1*1.0, col1, row2, col2);
            HRegion diffHregion =  hRegionRect1.Difference(hRegion);
            HRegion opemHRegion = diffHregion.OpeningRectangle1(param.OpenWidth, param.OpenHeight);
            hRegionGap = opemHRegion.Clone();
            hRegion1Union?.Dispose();
            hRegionRect1?.Dispose();
            diffHregion?.Dispose();
            opemHRegion?.Dispose();
            return hRegionGap;
        }

        /// <summary>
        /// 裂纹检测
        /// </summary>
        /// <param name="hRegion"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        private static HRegion CrackDetect(HRegion hRegion, Rect2DetectParam param)
        {
            HRegion hRegionGap = new HRegion();

            int row1, col1, row2, col2;
            HRegion hRegion1Union = hRegion.Union1();

            hRegion1Union.SmallestRectangle1(out row1, out col1, out row2, out col2);
            HRegion hRegionRect1 = new HRegion(row1 + param.RowOffset, col1 + param.ColOffset, row2 - param.RowOffset, col2 - param.ColOffset);


            return hRegionGap;
        }





    }

}
