using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartray.Sample
{


    //
    // contains sensor image data which is contructed out of certain profile packages
    //
    public class PackagedImage<intType>
    {
        //
        // stores the contents of a package
        //
        struct Package
        {
            public intType[] Data;
            public int Size;

            //
            // Ctor
            //
            public Package(intType[] data, int size)
            {
                Data = data;
                Size = size;
            }
        };

        List<Package> _packages = new List<Package>();


        //
        // returns the full image as an array of the given value type
        //
        public intType[] GetImage()
        {
            // return NULL when no image data is present
            int size = GetImageSize();
            if (size == 0)
                return null;

            intType[] image = new intType[size];
            int imageIndex = 0;
            lock (_packages)
            {
                foreach (var package in _packages)
                {
                    for (int i = 0; i < package.Size; i++)
                        image[imageIndex++] = package.Data[i];
                }
            }
            return image;
        }

        //
        // append profile package data to the image
        //
        public void AddPackageData(intType[] data, int size)
        {
            var dataCopy = data.ToArray<intType>();
            lock (_packages)
            {
                _packages.Add(new Package(dataCopy, size));
            }
        }

        //
        // return the size the complete image of image elements
        //
        public int GetImageSize()
        {
            int imageSize = 0;
            lock (_packages)
            {
                foreach (var package in _packages)
                    imageSize += package.Size;
            }
            return imageSize;
        }


    }

}
