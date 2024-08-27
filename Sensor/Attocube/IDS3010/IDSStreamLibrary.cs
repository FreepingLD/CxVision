using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Sensor
{
    public class IDSStreamLibrary
    {
        
        #region  流写入

        /// <summary>
        /// 打开流
        /// </summary>
        /// <param name="host">主机IP地址</param>
        /// <param name="isMaster">用于多台设备，一台设备始终为true</param>
        /// <param name="intervalInMicroseconds">采样时间(以微秒为单位)——所需的流速率是1/intervallInMicroseconds(最大)。1 MHz用于一个测量轴，500 kHz用于多个测量轴)</param>
        /// <param name="channelMask"></param>
        /// <returns></returns>
        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern IntPtr OpenStream(string host, bool isMaster, int intervalInMicroseconds, int channelMask = 1 | 4);

        /// <summary>
        /// 关闭流
        /// </summary>
        /// <param name="stream"></param>
        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern void CloseStream(IntPtr stream);

        /// <summary>
        /// 读取流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="datauffer"></param>
        /// <param name="dataBufferSize">必须读取的最大字节数(该数目必须等于或小于缓冲区的大小)</param>
        /// <returns></returns>
        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int ReadStream(IntPtr stream, IntPtr dataBuffer, int dataBufferSize);

        /// <summary>
        /// 读取流
        /// </summary>
        /// <param name="stream"></param>
        /// <param name="datauffer"></param>
        /// <param name="dataBufferSize">必须读取的最大字节数(该数目必须等于或小于缓冲区的大小)</param>
        /// <returns></returns>
        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int ReadStream(IntPtr stream, byte [] dataBuffer, int dataBufferSize);

        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int DecodeStream(IntPtr stream, IntPtr dataBuffer, int dataBufferSize, IntPtr[] destinationBuffers, int destinationBuffersSize, out int decodedSamplesCount);

        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int DecodeStream(IntPtr stream, IntPtr dataBuffer, int dataBufferSize, long[,] destinationBuffers, int destinationBuffersSize, out int decodedSamplesCount);

        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int DecodeStreamSingle(IntPtr stream, IntPtr dataBuffer, int dataBufferSize, ref long [] destinationBuffer1, ref long[] destinationBuffer2, ref long[] destinationBuffer3, int destinationBuffersSize, out int decodedSamplesCount);

        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int DecodeStreamSingle(IntPtr stream, byte [] dataBuffer, int dataBufferSize, ref long[] destinationBuffer1, ref long[] destinationBuffer2, ref long[] destinationBuffer3, int destinationBuffersSize, out int decodedSamplesCount);

        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int StartStreamRecording(IntPtr stream, ref string path);


        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int StopStreamRecording(IntPtr stream);


        [DllImport("Attocube.Common.NativeC.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Ansi)]
        public static extern int DecodePackets(ref byte src, int packetCount, int samplesPerPacket, int channelCount, ref long offsets, IntPtr[] dst);


        #endregion

    }
}
