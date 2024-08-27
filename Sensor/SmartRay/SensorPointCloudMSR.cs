using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smartray;

namespace Smartray.Sample
{
    public class PointCloudMSR
	{
		private uint _numPoints;
		private Api.Point3d[] _point3d;
		private ushort[] _intensity;
		private ushort[] _laserLineThickness;
		private uint[] _sensorIdx;
		private uint[] _profileIdx;
		private uint[] _pointIdx;

		public PointCloudMSR()
		{}

		public PointCloudMSR(uint numPoints, Api.Point3d[] point3d, ushort[] intensity,
			ushort[] laserLineThickness, uint[] sensorIdx, uint[] profileIdx, uint[] pointIdx)
		{
			_numPoints = numPoints;
			_point3d = new Api.Point3d[numPoints];
			System.Array.Copy(point3d, _point3d, numPoints);

			if (intensity != null)
			{
				_intensity = new ushort[numPoints];
				System.Array.Copy(intensity, _intensity, numPoints);
			}
			if (laserLineThickness != null)
			{
				_laserLineThickness = new ushort[numPoints];
				System.Array.Copy(laserLineThickness, _laserLineThickness, numPoints);
			}
			if(sensorIdx != null)
			{
				_sensorIdx = new uint[numPoints];
				System.Array.Copy(sensorIdx, _sensorIdx, numPoints);
			}
			if(profileIdx != null)
			{
				_profileIdx = new uint[numPoints];
				System.Array.Copy(profileIdx, _profileIdx, numPoints);
			}
			if(pointIdx != null)
			{
				_pointIdx = new uint[numPoints];
				System.Array.Copy(pointIdx, _pointIdx, numPoints);
			}
		}

		public static void PrintHeader(string fileName, float transportResolution)
		{
			System.IO.StreamWriter file = new System.IO.StreamWriter(fileName);
			file.WriteLine("");
			file.WriteLine("NOTE: transport Resolution = " + transportResolution);
			file.WriteLine("");
			file.Write("id" + "\t");
			file.Write("p.x" + "\t");
			file.Write("p.y" + "\t");
			file.Write("p.z" + "\t");
			file.Write("Intens" + "\t");
			file.Write("LLT" + "\t");
			file.Write("Sensor" + "\t");
			file.Write("Profile" + "\t");
			file.WriteLine("Point" + "\t");
			file.WriteLine("");
			file.Close();
		}

		static uint PrintPoints(string fileName, 
			float transportResolution, 
			bool saveAllPoints,
			uint numPoints,
			uint startPointIdx,
			Api.Point3d[] points,
			ushort[] intensity,
			ushort[] laserLineThickness,
			uint[] sensorIdx,
			uint[] profileIdx,
			uint[] pointIdx)
		{
			// NOTE: The raw Point Cloud data received from the sensor are 2-dimensional (y and z coordinates only).
			//	   To get 3-dimensional points (x, y, z) the x-coordinated is calculated using the transportResolution.
			//	   The parameter transportResolution defines the constant x-axis displacement between two subsequent 
			//	   profiles.
			//	   Thus:	   x = (profile-id - 1) * transportResolution

			System.Console.WriteLine("--- Saving Point Cloud data to file : " + fileName + " ---");
			uint validPointIdx = startPointIdx;

			System.IO.StreamWriter file = new System.IO.StreamWriter(fileName, true);

			for (uint i = 0; i < numPoints; ++i)
			{
				// Skip invalid points (unless printing of all data points is requested):
				if ((points[i].X > -999990.0) || saveAllPoints)
				{
					validPointIdx++;
				
					file.Write(validPointIdx + "\t");
					file.Write(points[i].X + "\t");
					file.Write(points[i].Y + "\t");
					file.Write(points[i].Z + "\t");
					if (intensity != null)
					{
						file.Write(intensity[i] + "\t");
					}
					else
					{
						file.Write("-1\t");
					}
					if (laserLineThickness != null)
					{
						file.Write(laserLineThickness[i] + "\t");
					}
					else
					{
						file.Write("-1\t");
					}
					if (sensorIdx != null)
					{
						file.Write(sensorIdx[i] + "\t");
					}
					else
					{
						file.Write("-1\t");
					}
					if (profileIdx != null)
					{
						file.Write(profileIdx[i] + "\t");
					}
					else
					{
						file.Write("-1\t");
					}
					if (pointIdx != null)
					{
						file.Write(pointIdx[i] + "\t");
					}
					else
					{
						file.Write("-1\t");
					}
					file.WriteLine("");
				}
			}
			file.Close();
			System.Console.WriteLine("Done!");
			return validPointIdx;
		}

		public uint SavePointCloud(string filename, float transportResolution, uint startPointIdx, bool saveAllPoints = false)
		{
			return PrintPoints(filename, transportResolution, saveAllPoints, _numPoints,
				startPointIdx, _point3d, _intensity, _laserLineThickness,
				_sensorIdx, _profileIdx, _pointIdx);

		}
	}
}