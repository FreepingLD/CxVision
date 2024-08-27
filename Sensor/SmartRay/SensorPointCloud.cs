using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Smartray;

namespace Smartray.Sample
{
    public class PointCloud
    {
        private uint _numPoints;
        private Api.Point3d[] _point3d;
        private  ushort[] _intensity;
        private  ushort[]  _laserLineThickness;
        private  uint[] _profileIdx;
        private  uint[] _columnIdx;

        public Api.Point3d[] Point3d
        {
            get
            {
                return _point3d;
            }

            set
            {
                _point3d = value;
            }
        }

        public PointCloud()
        {}

        public PointCloud(uint numPoints, Api.Point3d[] point3d, ushort[] intensity, ushort[] laserLineThickess,
                            uint[] profileIdx, uint[] columnIdx)
        {
            _numPoints = numPoints;
            Point3d = new Api.Point3d[numPoints];
            _intensity = new ushort[numPoints];
            _laserLineThickness = new ushort[numPoints];
            _profileIdx = new uint[numPoints];
            _columnIdx = new uint[numPoints];
                
            System.Array.Copy(point3d, Point3d, numPoints);
            System.Array.Copy(intensity, _intensity, numPoints);
            System.Array.Copy(laserLineThickess, _laserLineThickness, numPoints);
            System.Array.Copy(profileIdx, _profileIdx, numPoints);
            System.Array.Copy(columnIdx, _columnIdx, numPoints);
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
            file.Write("Profile" + "\t");
            file.Write("Column" + "\t");
            file.Write("Intens" + "\t");
            file.WriteLine("LLT" + "\t");
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
                                uint[] profileIdx,
                                uint[] columnIdx)
        {
            // NOTE: The raw Point Cloud data received from the sensor are 2-dimensional (y and z coordinates only).
            //       To get 3-dimensional points (x, y, z) the x-coordinated is calculated using the transportResolution.
            //       The parameter transportResolution defines the constant x-axis displacement between two subsequent 
            //       profiles.
            //       Thus:       x = (profile-id - 1) * transportResolution

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
                    if (profileIdx != null) file.Write(profileIdx[i] + "\t");
                    if (columnIdx != null) file.Write(columnIdx[i] + "\t");
                    if (intensity != null) file.Write(intensity[i] + "\t");
                    if (laserLineThickness != null) file.WriteLine(laserLineThickness[i] + "\t");
              }
            }
            file.Close();
            System.Console.WriteLine("Done!");
           return validPointIdx;
        }

        public uint SavePointCloud(string filename, float transportResolution, uint startPointIdx, bool saveAllPoints = false)
        {
            return PrintPoints(filename, transportResolution, saveAllPoints, _numPoints,
                               startPointIdx, Point3d, _intensity, _laserLineThickness,
                               _profileIdx, _columnIdx);
 
        }
    }
}