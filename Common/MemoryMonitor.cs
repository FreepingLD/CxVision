using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class MemoryMonitor
    {

        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <param name="meminfo"></param>
        [DllImport("kernel32")]
        public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);
        [DllImport("kernel32")]
        public static extern bool GlobalMemoryStatusEx(ref MEMORYSTATUSEX stat);
        /// <summary>
        /// 获取内存信息
        /// </summary>
        /// <returns></returns>
        public static MemoryInfo getMemoryInfo()
        {
            MEMORY_INFO memInfo = new MEMORY_INFO();
            MEMORYSTATUSEX memEx = new MEMORYSTATUSEX();
            memEx.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            GlobalMemoryStatusEx(ref memEx);
            GlobalMemoryStatus(ref memInfo);
            MemoryInfo memoryInfo = new MemoryInfo();
            memoryInfo.memoryLoad = memInfo.dwMemoryLoad;
            memoryInfo.availPhys = memInfo.dwAvailPhys;
            memoryInfo.totalPhys = memInfo.dwTotalPhys;
            return memoryInfo;
        }
        /// <summary>
        /// 获取内存占用率
        /// </summary>
        /// <returns></returns>
        public static uint GetMenoryLoad()
        {
            MEMORY_INFO memInfo = new MEMORY_INFO();
            MEMORYSTATUSEX memEx = new MEMORYSTATUSEX();
            memEx.dwLength = (uint)Marshal.SizeOf(typeof(MEMORYSTATUSEX));
            GlobalMemoryStatusEx(ref memEx);
            GlobalMemoryStatus(ref memInfo);
            return memInfo.dwMemoryLoad;
        }
        ///  <summary> 
        /// 获取指定驱动器的空间总大小(单位为B) 
        ///  </summary> 
        ///  <param name="str_HardDiskName">只需输入代表驱动器的字母即可 （大写）</param> 
        ///  <returns> </returns> 
        public static long GetHardDiskSpace(string str_HardDiskName)
        {
            long totalSize = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    totalSize = drive.TotalSize;
                }
            }
            return totalSize/ (long)Math.Pow(1024, 3);
        }
        ///  <summary> 
        /// 获取指定驱动器的剩余空间总大小(单位为B) 
        ///  </summary> 
        ///  <param name="str_HardDiskName">只需输入代表驱动器的字母即可 </param> 
        ///  <returns> </returns> 
        public static long GetHardDiskFreeSpace(string str_HardDiskName)
        {
            long freeSpace = new long();
            str_HardDiskName = str_HardDiskName + ":\\";
            System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
            foreach (System.IO.DriveInfo drive in drives)
            {
                if (drive.Name == str_HardDiskName)
                {
                    freeSpace = drive.TotalFreeSpace;
                }
            }
            return freeSpace / (long)Math.Pow(1024, 3);
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORY_INFO
    {
        public uint dwLength;
        public uint dwMemoryLoad;//内存占用比
        public UInt64 dwTotalPhys; //总的物理内存大小
        public UInt64 dwAvailPhys; //可用的物理内存大小 
        public UInt64 dwTotalPageFile;
        public UInt64 dwAvailPageFile; //可用的页面文件大小
        public UInt64 dwTotalVirtual; //返回调用进程的用户模式部分的全部可用虚拟地址空间
        public UInt64 dwAvailVirtual; // 返回调用进程的用户模式部分的实际自由可用的虚拟地址空间
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct MEMORYSTATUSEX
    {
        public uint dwLength;
        public uint dwMemoryLoad;
        public ulong ullTotalPhys;
        public ulong ullAvailPhys;
        public ulong ullTotalPageFile;
        public ulong ullAvailPageFile;
        public ulong ullTotalVirtual;
        public ulong ullAvailVirtual;
        public ulong ullAvailExtendedVirtual;
    }
    /// <summary>
    /// 存放内存信息
    /// </summary>
    public class MemoryInfo
    {
        public uint memoryLoad { get; set; }//返回00形式
        public ulong totalPhys { get; set; } //以Bite为单位
        public ulong availPhys { get; set; }//以Bite为单位
    }
}
