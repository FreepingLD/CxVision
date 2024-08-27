using Smartray;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using HalconDotNet;

namespace Smartray.Sample
{

    //
    // holds the image data as reported by the sensor
    //
    public class SensorImageData 
	{

		public int Width;
		public int FullHeight;
		public int OriginX;
		public float OriginYMillimeters;
		Api.ImageDataType ImageType;

		// current height of the image
		public int CurrentHeight;

		// live image component
		public PackagedImage<byte> LiveImage = new PackagedImage<byte>();

		// PIL image components
		public PackagedImage<ushort> ProfileImage = new PackagedImage<ushort>();
		public PackagedImage<ushort> IntensityImage = new PackagedImage<ushort>();
		public PackagedImage<ushort> LaserLineThicknessImage = new PackagedImage<ushort>();

		// ZIL image components
		public PackagedImage<ushort> ZMapImage = new PackagedImage<ushort>();
		public PackagedImage<ushort> ZMapIntensityImage = new PackagedImage<ushort>();
		public PackagedImage<ushort> ZMapLaserLineThicknessImage = new PackagedImage<ushort>();


		//
		// default Ctor
		//
		public SensorImageData()
		{
        }

		//
		// Ctor
		//
		public SensorImageData(int width, int fullHeight, Api.ImageDataType imageType, int originX = 0, float originYMillimeters = 0)
		{
			Width = width;
			FullHeight = fullHeight;
			ImageType = imageType;
			OriginX = originX;
			OriginYMillimeters = originYMillimeters;
		}


		//
		// returns true when at least one valid image is contained matching the height
		//
		public bool ContainsFullImages()
		{
			int fullSize = Width * FullHeight;

			switch (ImageType)
			{
				case Api.ImageDataType.LiveImage:
					return (LiveImage.GetImageSize() == fullSize);

				case Api.ImageDataType.Profile:
					return (ProfileImage.GetImageSize() == fullSize);

				case Api.ImageDataType.Intensity:
					return (IntensityImage.GetImageSize() == fullSize);

				case Api.ImageDataType.ProfileIntensity:
					return (ProfileImage.GetImageSize() == fullSize
						&& IntensityImage.GetImageSize() == fullSize);

				case Api.ImageDataType.ProfileIntensityLaserLineThickness:
					return (ProfileImage.GetImageSize() == fullSize
						&& IntensityImage.GetImageSize() == fullSize
						&& LaserLineThicknessImage.GetImageSize() == fullSize);

				case Api.ImageDataType.ZMap:
					return (ZMapImage.GetImageSize() == fullSize);

				case Api.ImageDataType.ZMapIntensity:
					return (ZMapImage.GetImageSize() == fullSize
						&& ZMapIntensityImage.GetImageSize() == fullSize);

				case Api.ImageDataType.ZMapIntensityLaserLineThickness:
					return (ZMapImage.GetImageSize() == fullSize
						&& ZMapIntensityImage.GetImageSize() == fullSize
						&& ZMapLaserLineThicknessImage.GetImageSize() == fullSize);

				default:
					return false;
			}
		}

		/// <summary>
		/// saves an 8 bit png image
		/// </summary>
		private void SavePng(string filename, PackagedImage<byte> imageData)
		{
			unsafe
			{
				fixed (byte* ptr = imageData.GetImage())
				{
					var bitmap = new Bitmap(Width, CurrentHeight, Width, System.Drawing.Imaging.PixelFormat.Format8bppIndexed, new IntPtr(ptr));

					// define mono palette
					ColorPalette palette = bitmap.Palette;
					System.Drawing.Color[] entries = palette.Entries;
					for (int i = 0; i < 256; i++)
					{
						System.Drawing.Color b = new System.Drawing.Color();
						b = System.Drawing.Color.FromArgb((byte)i, (byte)i, (byte)i);
						entries[i] = b;
					}
					bitmap.Palette = palette;

					SaveBitmap(bitmap, filename + ".png");
				}
			}
			SmartRaySensorManager.Trace("saving image to:\n" + filename);
		}

        public HImage GetLiveImage()
        {
            HImage image;
            // IntPtr ptr = IntPtr.Zero;
            int length = this.LiveImage.GetImage().Length;
            IntPtr ptr = Marshal.AllocHGlobal(length);
            Marshal.Copy(this.LiveImage.GetImage(),0,ptr, length);
            image = new HImage("byte", Width, CurrentHeight, ptr);
            Marshal.FreeHGlobal(ptr);
           return image;          
        }

        /// <summary>			
        /// saves an 16 bit png image
        /// </summary>
        private void SavePng(string filename, PackagedImage<ushort> imageData)
		{
			if (0 == imageData.GetImageSize())
			{
				return;
			}
			unsafe
			{
				var image = imageData.GetImage();

				// align image width
				int align = Width % 2;
				int newWidth = Width - align;
				if (Width != newWidth)
				{
					ushort[] newImage = new ushort[newWidth * CurrentHeight];
					for (int line = 0; line < CurrentHeight; line++)
						for (int i = 0; i < newWidth; i++)
							newImage[i + line * newWidth] = image[i + line * Width];

					Width = newWidth;
					image = newImage;
				}

				fixed (ushort* ptr = image)
				{
					var bitmap = new Bitmap(Width, CurrentHeight, Width * 2, System.Drawing.Imaging.PixelFormat.Format16bppGrayScale, new IntPtr(ptr));
					SaveBitmap(bitmap, filename + ".png");
				}
			}
			SmartRaySensorManager.Trace("saving image to:\n" + filename);
		}
	
		private static void SaveBitmap(Bitmap bmp, string path)
		{
			Rectangle rect = new Rectangle(0, 0, bmp.Width, bmp.Height);
			BitmapData bitmapData = bmp.LockBits(rect, ImageLockMode.ReadOnly, bmp.PixelFormat);

			var pixelFormats = ConvertBmpPixelFormat(bmp.PixelFormat);
			BitmapSource source = BitmapSource.Create(bmp.Width,
													  bmp.Height,
													  bmp.HorizontalResolution,
													  bmp.VerticalResolution,
													  pixelFormats,
													  null,
													  bitmapData.Scan0,
													  bitmapData.Stride * bmp.Height,
													  bitmapData.Stride);

			bmp.UnlockBits(bitmapData);


			FileStream stream = new FileStream(path, FileMode.Create);
			PngBitmapEncoder encoder = new PngBitmapEncoder();
			encoder.Frames.Add(BitmapFrame.Create(source));
			encoder.Save(stream);

			stream.Close();
		}

		private static System.Windows.Media.PixelFormat ConvertBmpPixelFormat(System.Drawing.Imaging.PixelFormat pixelformat)
		{
			System.Windows.Media.PixelFormat pixelFormats = System.Windows.Media.PixelFormats.Default;
			switch (pixelformat)
			{
				case System.Drawing.Imaging.PixelFormat.Format32bppArgb:
					pixelFormats = PixelFormats.Bgr32;
					break;

				case System.Drawing.Imaging.PixelFormat.Format8bppIndexed:
					pixelFormats = PixelFormats.Gray8;
					break;

				case System.Drawing.Imaging.PixelFormat.Format16bppGrayScale:
					pixelFormats = PixelFormats.Gray16;
					break;
			}
			return pixelFormats;
		}		
		

		//
		// save live image data as png
		//
		public void SaveLiveImage(string filename)
		{
			SavePng(filename + "_live", LiveImage);
		}
		
		//
		// save PIL image data as png
		//
		public void SavePilImage(string filename)
		{
			SavePng(filename + "_P", ProfileImage);
			SavePng(filename + "_I", IntensityImage);
			SavePng(filename + "_L", LaserLineThicknessImage);
		}

		//
		// save ZIL image data as png
		//
		public void SaveZilImage(string filename)
		{
			SavePng(filename + "_Z", this.ZMapImage);
			SavePng(filename + "_I", this.ZMapIntensityImage);
			SavePng(filename + "_L", this.ZMapLaserLineThicknessImage);
		}

	}
}
