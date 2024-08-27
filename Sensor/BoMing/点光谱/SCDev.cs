using System;
using System.Runtime.InteropServices;

namespace Sensor
{
    /// <summary>
    /// 代表一个点白光传感器设备对象
    /// </summary>
    public class SCDev : IDisposable
    {
        #region 字段
        /// <summary>
        /// 设备指针
        /// </summary>
        private IntPtr mDevPtr = IntPtr.Zero;
        private bool disposed = false;
 
        #endregion

        #region 构造函数
        /// <summary>
        /// 创建一个 SCDev 对象实例,完毕后应调用Dispose()方法销毁对象
        /// </summary>
        public SCDev()
        {
            mDevPtr = SCDev_Native.Create();
        }
        /// <summary>
        /// 创建一个 SCDev 对象实例，并指定接下来的属性设置和获取的超时时间，完毕应后调用Dispose()方法销毁对象
        /// </summary>
        /// <param name="in_setTimeout"></param>
        /// <param name="in_getTimeout"></param>
        public SCDev(int in_setTimeout, int in_getTimeout)
            :this()
        {
            SCDev_Native.SetTimeout(mDevPtr, in_setTimeout, in_getTimeout);
        }

        ~SCDev()
        {
            Dispose(false); //必须为false
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 获取设备最后一次错误码,非设备错误码，请参考错误日志
        /// </summary>
        /// <returns>返回错误码，参见 DeviceError 枚举</returns>
        public enDeviceError GetDeviceLastError()
        {
            return SCDev_Native.GetDeviceLastError(mDevPtr);
        }

        /// <summary>
        /// 打开设备.设备使用完毕，应当调用close()函数关闭设备，释放资源。在进行设备操作前应当调用此接口打开设备。默认超时5000ms
        /// </summary>
        /// <param name="in_connectionSettings">指定设备以何种连接方式打开；参见ConnectionSetting_s结构体</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Open(ConnectionSetting_s in_connectionSettings)
        {
            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(in_connectionSettings));
            Marshal.StructureToPtr(in_connectionSettings, p, true);
            bool ok= SCDev_Native.Open(mDevPtr, p, 5000);
            Marshal.FreeHGlobal(p);
            return ok;
        }

        /// <summary>
        /// 打开设备.设备使用完毕，应当调用close()函数关闭设备，释放资源。
        /// </summary>
        /// <param name="in_connectionSettings">指定设备以何种连接方式打开；参见ConnectionSetting_s结构体</param>
        /// <param name="TimeOut">指定设备以网口方式打开时的超时时间，单位ms</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Open(ConnectionSetting_s in_connectionSettings, int TimeOut)
        {
            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(in_connectionSettings)); // 分配一个指定大小的内存
           // int x = Marshal.SizeOf(in_connectionSettings);
           // string s = "10.10.1.32";
            Marshal.StructureToPtr(in_connectionSettings, p, true); // 复制一个结构体到指针指向的内容中
            bool ok = SCDev_Native.Open(mDevPtr, p, TimeOut);
            Marshal.FreeHGlobal(p);
            return ok;
        }

        /// <summary>
        /// 关闭设备.此操作将强行终止设备连接；请确保执行此操作前没有正在进行的数据传输动作。
        /// </summary>
        /// <returns>成功返回true，失败返回false</returns>
        public bool Close()
        {
            return SCDev_Native.Close(mDevPtr);
        }

        public int GetFirmwareVersion()
        {
            bool ok = false;
            object obj = new object();
            ok = GetProperty(enPropId.FirmwareVersion, typeof(FirmwareVersion_s), ref obj);
            FirmwareVersion_s version = (FirmwareVersion_s)obj;
            string v = string.Format("{0:D}{1:D}{2:D2}", version.fimwareVersion[2], version.fimwareVersion[1], version.fimwareVersion[0]);
            return Int32.Parse(v);
        }
        public bool StartAcquisition()
        {
            bool ok = false;
             // 这里加停止，是为了在流采集前必需先停止往Buffer中存数据
            ok = SetProperty(enPropId.SensorStatus, new SensorStauts_s { value = enSensorStauts.Stop });
            // 在开始采集前必需清空Buffer中的数据
            ok = SetProperty(enPropId.ClearStoredMeasureValuesCount, null);
            /// 启动流采集
            ok = SetProperty(enPropId.SensorStatus, new SensorStauts_s { value = enSensorStauts.Start });
            return ok;
        }
        public bool StopAcquisition()
        {
            bool ok = false;
            SensorStauts_s data = new SensorStauts_s { value = enSensorStauts.Stop };
            ok = SetProperty(enPropId.SensorStatus, data);
            return ok;
        }

        /// <summary>
        /// 设置设备属性.注意，一些属性的设置要在设置传感器采集动作启动或停止状态才可以.
        /// </summary>
        /// <param name="in_propId">要设置的source属性索引，参见 PropId 枚举</param>
        /// <param name="structure">要设置的属性对象，根据PropId的不同而不同；有些属性不需要提供数据，则此参数应为NULL.</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool SetProperty(enPropId in_propId, object structure)
        {
            bool ok = false;
            if (structure!=null)
            {
                IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(structure));
                Marshal.StructureToPtr(structure, p, true);
                ok = SCDev_Native.SetProperty(mDevPtr, in_propId, p);
                Marshal.FreeHGlobal(p);
            }
            else // 没有 data 字段被set下去！
            {
                ok = SCDev_Native.SetProperty(mDevPtr, in_propId, IntPtr.Zero);  
            }
            return ok;
        }

        public bool SetProperty(enPropId in_propId, Type in_structureType, object in_structure=null)
        {
            bool ok = false;
            if (in_structure != null)
            {
                IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(in_structureType));
                Marshal.StructureToPtr(in_structureType, p, true);
                ok = SCDev_Native.SetProperty(mDevPtr, in_propId, p);
                Marshal.FreeHGlobal(p);
            }
            else // 没有 data 字段被set下去！
            {
                ok = SCDev_Native.SetProperty(mDevPtr, in_propId, IntPtr.Zero);
            }
            return ok;
        }

        /// <summary>
        /// 获取设备属性.注意，一些属性的获取要在设置传感器采集动作启动或停止状态才可以.
        /// </summary>
        /// <param name="in_propId">属性Id，参见PropId枚举</param>
        /// <param name="out_structure">获取到的属性对象</param>
        /// <param name="structureType">属性的数据类型</param>
        /// <returns>成功返回true，失败返回false</returns>
        public bool GetProperty(enPropId in_propId, Type in_structureType, ref object out_structure)
        {
            IntPtr p = Marshal.AllocHGlobal(Marshal.SizeOf(in_structureType));
            bool ok = SCDev_Native.GetProperty(mDevPtr, in_propId, p);

            out_structure = Marshal.PtrToStructure(p, in_structureType);
            Marshal.FreeHGlobal(p);

            return ok;
        }

        /// <summary>
        /// 获取数据流buffer中数据的大小,此函数与getStreamBuffer()配读使用；调用后应当再调用getStreamBuffer()函数获取 buffer中的数据
        /// </summary>
        /// <param name="in_propId">要获取的属性的索引，必须为 PropId::StoredMeasureValues 枚举</param>
        /// <param name="out_pPropLen">返回实际获取到的数据个数,由具体 in_propId 指定，不一定是字节单位。</param>
        /// <param name="in_errTimeout">当发生错误时，指定错误状态的阻塞超时时间，如果为负数，则默认为3000ms</param>
        /// <returns></returns>
        public bool GetStreamBufferLen(enPropId in_propId, ref int out_pPropLen, int in_errTimeout=-1)
        {
            return SCDev_Native.GetStreamBufferLen(mDevPtr, in_propId, ref out_pPropLen, in_errTimeout);
        }

        /// <summary>
        /// 获取数据流buffer中数据,此函数与getStreamBufferLen()配读使用；调用前应当先调用getStreamBufferLen()函数获取 buffer中的大小
        /// </summary>
        /// <param name="in_propId">要获取的属性的索引，必须为 PropId::StoredMeasureValues 枚举</param>
        /// <param name="out_pPropData">返回指定获取到的 buffer数据</param>
        /// <param name="in_propLen">要获取的buffer数据的个数,由具体 in_propId 指定，不一定是字节单位</param>
        /// <returns>成功返回true，失败返回false。</returns>
        public bool GetStreamBuffer(enPropId in_propId, StoredMeasureValues_s[] out_pPropData, int in_propLen)
        {
           int size = Marshal.SizeOf(typeof(StoredMeasureValues_s));
           IntPtr pArray = Marshal.AllocHGlobal(size*in_propLen);
           bool ok = SCDev_Native.GetStreamBuffer(mDevPtr, in_propId, pArray, in_propLen);
           if(!ok)
           {
               Marshal.FreeHGlobal(pArray);
               return false;
           }
           for(int i=0; i<in_propLen; i++)
           {
               long longPtr=(long)pArray+ i*size;
               IntPtr p=(IntPtr)longPtr;
               out_pPropData[i]=(StoredMeasureValues_s)Marshal.PtrToStructure(p, typeof(StoredMeasureValues_s));
           }
           Marshal.FreeHGlobal(pArray);
           return ok;
        }

        public bool GetStreamBuffer(enPropId in_propId, StoredMeasureValuesEx_s[] out_pPropData, int in_propLen)
        {
           int size = Marshal.SizeOf(typeof(StoredMeasureValuesEx_s));
           IntPtr pArray = Marshal.AllocHGlobal(size*in_propLen);
           bool ok = SCDev_Native.GetStreamBuffer(mDevPtr, in_propId, pArray, in_propLen);
           if(!ok)
           {
               Marshal.FreeHGlobal(pArray);
               return false;
           }
           for(int i=0; i<in_propLen; i++)
           {
               long longPtr=(long)pArray+ i*size;
               IntPtr p=(IntPtr)longPtr;
               out_pPropData[i]=(StoredMeasureValuesEx_s)Marshal.PtrToStructure(p, typeof(StoredMeasureValuesEx_s));
           }
           Marshal.FreeHGlobal(pArray);
           return ok;
        }

        #endregion

        #region 公有属性
        /// <summary>
        /// 设备指针，提供给 Native方法使用
        /// </summary>
        public IntPtr NativePtr
        {
            get
            {
                return mDevPtr;
            }
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 对象资源清理；
        /// 对象释放，设备不一定关闭，如果需要，要手动关闭
        /// 调用对象的dispose方法让GC回收对象
        /// </summary>
        public void Dispose()
        {
            if (this == null)
                return;
            //必须为true
            this.Dispose(true);
            //通知垃圾回收机制不再调用终结器（析构器）
            GC.SuppressFinalize(this);
        }
        #endregion

        #region 私有方法

        #region IDisposable 接口实现

        /// <summary>
        /// </summary>
        /// <param name="disposing"></param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
            {
                return;
            }
            if (disposing)
            {

            }
            SCDev_Native.Delete(mDevPtr);
            mDevPtr = IntPtr.Zero;
            //让对象知道自己已经被 GC 释放
            disposed = true;
        }
        #endregion

        #endregion
    }
}
