using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Smartray.Sample
{
	#region FatalErrorException
	//
	// fire when a fatal error occurs which disallows test execution
	//
	class FatalErrorException : Exception
	{
		// ctor
		public FatalErrorException(string message)
			: base(message)
		{ }
	}
    #endregion

    // 
    // runs a sample method and handles errors
    //
    public class SampleRunner
	{
		public delegate void SampleMethod();

		SampleMethod _method;
		public string Description;
		public bool AutoRun;

		//
		// Ctor
		//
		public SampleRunner(string description, SampleMethod method, bool autorun=false)
		{
			Description = description;
			_method = method;
			AutoRun = autorun;
		}

		//
		// run the sample
		//
		public bool Run()
		{
			try
			{
				Console.WriteLine("executing sample '" + Description + "'\n");
				_method();
				return true;
			}
			catch (FatalErrorException e)
			{
				Console.WriteLine("FATAL: " + e.Message);
				return false;
			}
			catch (Exception e)
			{
				Console.WriteLine("ERROR: an exception occured during the execution of sample '" + Description + "'\n" + e.ToString());
				return false;
			}
		}
	};
}
