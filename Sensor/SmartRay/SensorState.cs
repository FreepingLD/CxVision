using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartray.Sample
{
    //
    // represents the current state of a Sensor
    //
  public  class SensorState : ICloneable
    {
        #region SensorConnectionState

        //
        // reflects the state of the sensor ip connection
        //
        public enum ConnectionState
        {
            Unknown = 0,
            Connecting = 1,
            Connected = 2,
            Disconnected = 3
        }

        #endregion
        
        // reflects the ip connection state of the sensor
	    public ConnectionState SensorConnection;

	    public string LastConnectionMessage = string.Empty;
        public string LastInfoMessage = string.Empty;
        public string LastErrorMessage = string.Empty;

	    // increases for each package that have been received
        public long ImagePackageCounter;

	    // increases for each alive signal
        public long AliveSignalCounter;


	    //
	    // Ctor
	    //
	    public SensorState()
	    {
		    ImagePackageCounter = 0;
		    AliveSignalCounter = 0;
            SensorConnection = ConnectionState.Unknown;
	    }

        /// <summary>
        /// create clone of the object
        /// </summary>
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
