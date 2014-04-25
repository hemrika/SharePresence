using System;
using System.Security.Cryptography;
using System.Text;

namespace Hemrika.SharePresence.SPLicense
{
	/// <summary>
	/// A set of static functions to support different functionality throughout Open License 
	/// that is shared.
	/// </summary>
	public class Utilities
	{
		private static bool				debug 				= false;
		
		/// <summary>
		/// This creates the Crypto Service Provider for an encryption stream.  This 
		/// also handles setting up the key for the provider and validating it is 
		/// the proper length.  If it is not the proper length it will be padded 
		/// to the correct size.
		/// </summary>
		/// <param name="key">
		/// The sting to be used as the encryption Key and IV.
		/// </param>
		/// <returns>
		/// The RijndaelManaged to be use as the Encryption Provider.
		/// </returns>
		public static RijndaelManaged CreateCryptoServiceProvider( string key )
		{
			RijndaelManaged	cryptoService	= new RijndaelManaged( );
			SHA512Managed	sha512			= new SHA512Managed(  );
			
			sha512.ComputeHash( Encoding.UTF8.GetBytes( key ) );
		
			cryptoService.Mode	= CipherMode.CBC;

			byte[] byteKeyArray	= new byte[24];
			byte[] byteIVArray	= new byte[16];
			
			for( int loop=0; loop<24; loop++ ) byteKeyArray[loop] = sha512.Hash[loop];
			for( int loop=0; loop<16; loop++ ) byteIVArray[loop] = sha512.Hash[loop];
			
			cryptoService.Key	= byteKeyArray;
			cryptoService.IV	= byteIVArray;
			
			return cryptoService;
		}
		
		/// <summary>
		/// Writes debug output to trace calls in the web environment... To be deleted
		/// and replace with the <see cref="System.Diagnostics.Debug">Debug</see> Class.
		/// </summary>
		/// <param name="str">
		/// The string to output to the screen
		/// </param>
		public static void WriteDebugOutput( string str )
		{
			if( debug == true )
			{
				if( System.Web.HttpContext.Current != null )
				{
					System.Web.HttpContext.Current.Response.Write( "<pre>" + str + "</pre>" );
					System.Web.HttpContext.Current.Trace.Warn( str );  //So it comes out red...
				}
			}
		}
		
		/// <summary>
		/// This will remove any unusable characters in strings which can not be 
		/// used as file names.
		/// </summary>
		/// <param name="str">
		/// The string to check
		/// </param>
		/// <returns>
		/// The resulting string
		/// </returns>
		public static string CheckStringForInvalidFileNameCharacters( string str )
		{
			str = str.Replace( "\\","A" );
			str = str.Replace( "/","B" );
			str = str.Replace( ":","C" );
			str = str.Replace( "*","D" );
			str = str.Replace( "?","E" );
			str = str.Replace( "<","F" );
			str = str.Replace( ">","G" );
			str = str.Replace( "|","H" );
			//str = str.Replace( Chr(34),"I" );
			
			return str;
		}
	}
}

#region Unit Test
#if TEST
namespace Hemrika.SharePresence.SPLicense.UnitTest
{
	using NUnit.Framework;
	
	/// <summary>
	/// 
	/// </summary>
	[TestFixture]
	public class TestUtilities
	{
		/// <summary>
		/// 
		/// </summary>
		[Test]
		public void TestToByteArray( )
		{
//			Assert.AreEqual( 49,	(Utilities.ToByteArray( "1234" ) )[0] );
//			Assert.AreEqual( 50,	(Utilities.ToByteArray( "1234" ) )[1] );
//			Assert.AreEqual( 51,	(Utilities.ToByteArray( "1234" ) )[2] );
//			Assert.AreEqual( 52,	(Utilities.ToByteArray( "1234" ) )[3] );
		}
	}
}
#endif
#endregion

