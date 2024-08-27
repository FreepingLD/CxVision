using Common;
using HalconDotNet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionBlock
{
    public class TileImageMethod
    {
        public static bool TileImage(ImageDataClass [] listImage,TileImageParam param, out ImageDataClass tiledImage)
        {
            bool result = false;
            tiledImage = null;
            if (listImage == null) throw new ArgumentNullException("listImage");
            if (param == null) throw new ArgumentNullException("param");
            if(listImage.Length != param.RowCount * param.ColCount) throw new ArgumentException("图像的数量不等于 RowCount * ColCount");
            int imageWidth, imageHeight;
            listImage.Last().Image.GetImageSize(out imageWidth, out imageHeight);
            int totalImageWidth = param.Width;
            int totalImageHeight = param.Height;
            HTuple offsetRow = new HTuple();
            HTuple offsetCol = new HTuple();
            HTuple row1 = new HTuple();
            HTuple col1 = new HTuple();
            HTuple row2 = new HTuple();
            HTuple col2 = new HTuple();
            //////////////////////////////////////////////////
            HImage grayImages = new HImage();
            grayImages.GenEmptyObj();
            for (int i = 0; i < param.RowCount; i++)
            {
                for (int j = 0; j < param.ColCount; j++)
                {
                    grayImages = grayImages.ConcatObj(listImage[i * param.ColCount + j].Image);
                    offsetCol.Append(((imageWidth)) * j + param.OffsetCol); 
                    offsetRow.Append(((imageHeight)) * i + param.OffsetRow); 
                    row1.Append(-1);
                    col1.Append(-1);
                    row2.Append(-1);
                    col2.Append(-1);
                }
            }
            ///////////////////////////////////
            HImage tiledImagesGray = grayImages.TileImagesOffset(offsetRow, offsetCol, row1, col1, row2, col2, totalImageWidth, totalImageHeight);
            // 计算拼接图像的相机内外参
            tiledImage = new ImageDataClass(tiledImagesGray);
            result = true;
            return result;
        }






    }
}
