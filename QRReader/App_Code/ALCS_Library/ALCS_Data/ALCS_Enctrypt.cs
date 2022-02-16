using System;
using System.Security.Cryptography;
using System.IO;
using System.Text;

using ALCS_Library.ALCS_Format;

namespace ALCS_Library.ALCS_Data
{
    #region "The Enumerators ..."

    /// <summary>
	/// All Supported Hash Codes in the system.
	/// </summary>
	public enum HashType :int 
	{ 
		MD5, 
		SHA1, 
		SHA256, 
		SHA384, 
		SHA512 
	}

	// The final format applied to the hash string 
	public enum HashMask:int
	{
		bytes,
		hex,
		HEX
	}

    #endregion 

    #region "Hashing ..."

    /// <summary>
	/// Generate a hash code series from a agiven string.
	/// </summary>
	public static class ALCS_Hashing
	{
		/// <summary>
		/// Compute a hash string from the input string and return it.
		/// </summary>
		/// <param name="strPlain"></param>
		/// <param name="hType"></param>
		/// <param name="hMask"></param>
		/// <returns></returns>
		public static string ComputeHash(string strPlain, HashType hType, HashMask hMask) 
		{
			string outStr = "";
			byte[] hashValue; 
				
			if(hType == HashType.MD5)
			{
				hashValue = ComputeMD5(strPlain);	
			}
			else if(hType == HashType.SHA1)
			{
				hashValue = ComputeSHA1(strPlain);
			}
			else if(hType == HashType.SHA256)
			{
				hashValue = ComputeSHA256(strPlain);
			}
			else if(hType == HashType.SHA384)
			{
				hashValue = ComputeSHA384(strPlain);
			}
			else if(hType == HashType.SHA512)
			{
				hashValue = ComputeSHA512(strPlain);
			}
			else
			{
				return "Error";
			}

			// How should we mask it.
			if(hMask == HashMask.bytes)
			{
				outStr = BitConverter.ToString(hashValue); 
			}
			else if(hMask == HashMask.HEX) 
			{
				foreach(byte b in hashValue) 
				{
					outStr += String.Format("{0:X2}", b);
				}
			}
			else if(hMask == HashMask.hex) 
			{
				foreach(byte b in hashValue) 
				{
					outStr += String.Format("{0:x2}", b);
				}
			}
			else
			{
				outStr = BitConverter.ToString(hashValue); 
			}

			return outStr;
		}

		/// <summary>
		/// Another version of the above ...
		/// </summary>
		/// <param name="strPlain"></param>
		/// <param name="hType"></param>
		/// <returns></returns>
		public static string ComputeHash(string strPlain, HashType hType)
		{
			return ComputeHash(strPlain, hType, HashMask.HEX);
		}

		/// <summary>
		/// Check if the Hash Code is associated with the string.
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="strHash"></param>
		/// <param name="hType"></param>
		/// <returns></returns>
		public static bool CheckHash(string inStr, string strHash, HashType hType) 
		{
			string strOrigHash = ComputeHash(inStr, hType);
			return (strOrigHash == strHash);
		} 

		/// <summary>
		/// Another version of the above .....
		/// </summary>
		/// <param name="inStr"></param>
		/// <param name="strHash"></param>
		/// <param name="hType"></param>
		/// <param name="hMask"></param>
		/// <returns></returns>
		public static bool CheckHash(string inStr, string strHash, HashType hType, HashMask hMask) 
		{
			string strOrigHash = ComputeHash(inStr, hType, hMask);
			return (strOrigHash == strHash);
		} 

		/// <summary>
		/// Compute a hashed array of bites using the MD5 algorithm.
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		private static byte[] ComputeMD5(string inStr)
		{
			byte[] inBytes = String2Byte(inStr);
			MD5 md5 = new MD5CryptoServiceProvider();
			return md5.ComputeHash(inBytes); 
		}

		/// <summary>
		/// Compute a hashed array of bites using the SHA1 algorithm.
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		private static byte[] ComputeSHA1(string inStr)
		{
			byte[] inBytes = String2Byte(inStr);
			SHA1Managed SHhash = new SHA1Managed();
			return SHhash.ComputeHash(inBytes); 
		}

		/// <summary>
		/// Compute a hashed array of bites using the SHA256 algorithm.
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		private static byte[] ComputeSHA256(string inStr)
		{
			byte[] inBytes = String2Byte(inStr);
			SHA256Managed SHhash = new SHA256Managed();
			return SHhash.ComputeHash(inBytes); 
		}

		/// <summary>
		/// Compute a hashed array of bites using the SHA384 algorithm.
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		private static byte[] ComputeSHA384(string inStr)
		{
			byte[] inBytes = String2Byte(inStr);
			SHA384Managed SHhash = new SHA384Managed();
			return SHhash.ComputeHash(inBytes); 
		}
		
		/// <summary>
		/// Compute a hashed array of bites using the SHA512 algorithm.
		/// </summary>
		/// <param name="inStr"></param>
		/// <returns></returns>
		private static byte[] ComputeSHA512(string inStr)
		{
			byte[] inBytes = String2Byte(inStr);
			SHA512Managed SHhash = new SHA512Managed();
			return SHhash.ComputeHash(inBytes); 
		}
		
		/// <summary>
		/// Return the input string as a given an array of bytes.
		/// </summary>
		/// <param name="inStr"></param>
        public static byte[] String2Byte(string inStr)
		{
			UnicodeEncoding UE = new UnicodeEncoding();	
			return UE.GetBytes(inStr); 
		}

        /// <summary>
        /// Return the string representation of the Key.
        /// </summary>
        /// <param name="inBytes"></param>
        /// <returns></returns>
        public static string Byte2String(byte[] inBytes)
        {
            UnicodeEncoding UE = new UnicodeEncoding();
            return UE.GetString(inBytes);
        }
    }

    #endregion 

    #region "Symetric Encryption ..."

    /// <summary>
    /// SymmCrypto is a wrapper of System.Security.Cryptography.SymmetricAlgorithm classes
    /// and simplifies the interface. It supports customized SymmetricAlgorithm as well.
    /// Jamil: this code was originally borrowed from the web but found to have few bugs
    /// The bugs are fixed now but more testing is required before we can be sure everything
    /// is O.K.
    /// </summary>
	public class ALCS_SymmetricEncryption
	{
		private SymmetricAlgorithm algCryptoProvider;
        private int ivSize;

        /// <summary>
        /// Supported .Net intrinsic SymmetricAlgorithm classes.
        /// </summary>
        public enum ALCS_SymmetricProcider : int
	    {
		    DES, 
            RC2, 
            Rijndael
	    }

        /// <summary>
        /// Constructor for using an intrinsic .Net SymmetricAlgorithm class.
        /// </summary>
        /// <param name="symmProvider"></param>
        public ALCS_SymmetricEncryption(ALCS_SymmetricProcider symmProvider)
		{
            switch (symmProvider)
			{
                case ALCS_SymmetricProcider.DES:
					algCryptoProvider = new DESCryptoServiceProvider();
					break;
                case ALCS_SymmetricProcider.RC2:
					algCryptoProvider = new RC2CryptoServiceProvider();
					break;
                case ALCS_SymmetricProcider.Rijndael:
					algCryptoProvider = new RijndaelManaged();
					break;
			}
            this.ivSize = algCryptoProvider.BlockSize / 8;  
		}

        /// <summary>
        /// Constructor for using a customized SymmetricAlgorithm class.
        /// </summary>
        /// <param name="ServiceProvider"></param>
        public ALCS_SymmetricEncryption(SymmetricAlgorithm ServiceProvider)
		{
			algCryptoProvider = ServiceProvider;

            this.ivSize = algCryptoProvider.BlockSize / 8;   
		}

        /// <summary>
        /// Depending on the legal key size limitations of a specific CryptoService provider
        /// and length of the private key provided, padding the secret key with space character
        /// to meet the legal size of the algorithm.
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
		private byte[] GetLegalKey(string Key)
		{
			string sTemp;

            if (algCryptoProvider.LegalKeySizes.Length > 0)
            {
                int lessSize = 0;
                int moreSize = algCryptoProvider.LegalKeySizes[0].MinSize;

                // key sizes are in bits
                while (Key.Length * 8 > moreSize)
                {
                    lessSize = moreSize;
                    moreSize += algCryptoProvider.LegalKeySizes[0].SkipSize;
                }

                sTemp = Key.PadRight(moreSize / 8, ' ');
            }
            else
            {
                sTemp = Key;
            }

			// convert the secret key to byte array
			return ASCIIEncoding.ASCII.GetBytes(sTemp);
		}

        /// <summary>
        /// Make sure the IV size is legal ....
        /// </summary>
        /// <param name="Key"></param>
        /// <returns></returns>
        private byte[] GetLegalIV(string Key)
        {
            string sTemp;

            if (algCryptoProvider.LegalBlockSizes.Length > 0)
            {
                int lessSize = 0;
                int moreSize = algCryptoProvider.LegalBlockSizes[0].MinSize;

                // key sizes are in bits
                while (Key.Length * 8 > moreSize)
                {
                    lessSize = moreSize;
                    moreSize += algCryptoProvider.LegalBlockSizes[0].SkipSize;
                }

                sTemp = Key.PadRight(moreSize / 8, ' ');
            }
            else
            {
                sTemp = Key;
            }

            // convert the secret key to byte array
            return ASCIIEncoding.ASCII.GetBytes(sTemp);
        }


        /// <summary>
        /// Call the Encryption Service and return the encrypted string ...
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
		public string Encrypting(string Source, string Key)
		{
			byte[] bytIn = System.Text.ASCIIEncoding.ASCII.GetBytes(Source);

			// create a MemoryStream so that the process can be done without I/O files
			System.IO.MemoryStream ms = new System.IO.MemoryStream();

            byte[] bytIV = new byte[this.ivSize];  
			byte[] bytKey = GetLegalKey(Key);
            Array.Copy(bytKey, bytIV, this.ivSize); 

			// set the private key
			algCryptoProvider.Key = bytKey;
            algCryptoProvider.IV = bytIV;

			// create an Encryptor from the Provider Service instance
			ICryptoTransform encrypto = algCryptoProvider.CreateEncryptor();

			// create Crypto Stream that transforms a stream using the encryption
			CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Write);

			// write out encrypted content into MemoryStream
			cs.Write(bytIn, 0, bytIn.Length);
			cs.FlushFinalBlock();
            
			// get the output and trim the '\0' bytes
			byte[] bytOut = ms.GetBuffer();
			int i = 0;
			for (i = 0; i < bytOut.Length; i++)
				if (bytOut[i] == 0)
					break;

            //U7wZPrGDZic8jwZ1APSPe1XY2ljMo44gPGsGKrl993s=
            //U7wZPrGDZic8jwZ1
            //Length of the data to decrypt is invalid.
        
			// convert into Base64 so that the result can be used in xml
			return System.Convert.ToBase64String(bytOut, 0, i);
            //return System.Convert.ToBase64String(ms.ToArray());
		}

        /// <summary>
        /// Decrypt the encrypted string the return the decrypted string.
        /// </summary>
        /// <param name="Source"></param>
        /// <param name="Key"></param>
        /// <returns></returns>
		public string Decrypting(string Source, string Key)
		{
			// convert from Base64 to binary
			byte[] bytIn = System.Convert.FromBase64String(Source);

			// create a MemoryStream with the input
			System.IO.MemoryStream ms = new System.IO.MemoryStream(bytIn, 0, bytIn.Length);

            // Create a legal key and a legat IV based on the passed key.
            byte[] bytIV = new byte[this.ivSize];  
            byte[] bytKey   = GetLegalKey(Key);
            Array.Copy(bytKey, bytIV, this.ivSize);  

			// set the private key
			algCryptoProvider.Key = bytKey;
            algCryptoProvider.IV = bytIV;
            //algCryptoProvider.Padding  

			// create a Decryptor from the Provider Service instance
			ICryptoTransform encrypto = algCryptoProvider.CreateDecryptor();
 
			// create Crypto Stream that transforms a stream using the decryption
			CryptoStream cs = new CryptoStream(ms, encrypto, CryptoStreamMode.Read);

			// read out the result from the Crypto Stream
			System.IO.StreamReader sr = new System.IO.StreamReader( cs );
			return sr.ReadToEnd();
		}
    }

    #endregion 

    #region "Simple Encoding and decoding - Base 64"

    public class ALCS_Encryption64
    {
        private bool health = true;
        private string memo = "";
        private byte[] key;
        private byte[] IV = {18, 52, 86, 120, 144, 171, 205, 239 };

        #region "Encrypt and decrypt ..."

        /// <summary>
        /// Encode to 64 Mode  ... if the hex flag i true hex the result 
        /// </summary>
        /// <param name="stringToEncrypt"></param>
        /// <param name="SEncryptionKey"></param>
        /// <param name="hex"></param>
        /// <returns></returns>
        public string Encrypt64(string stringToEncrypt, string SEncryptionKey, bool hex)
        {
            try
            {
                // Qualify the key (i.e. make sure it is 8 bytes);
                key = QualifyKey(SEncryptionKey);

                // Create the encrypted stream.
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                byte[] inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();

                // transform to base 64 string.
                string str64 = Convert.ToBase64String(ms.ToArray());
                
                // Should we hex the form.
                string hexStr;
                string encStr;
                bool dataOK;

                // Should we hex 
                if(hex)
                {
                    dataOK = ALCS_Conversion.StringToHex(str64, ALCS_Conversion.HexForm.x2, out hexStr);

                    if (!dataOK)
                    {
                        encStr = "";
                        this.health = false;
                        this.memo = "The string cannot be turned into a hex form";
                    }
                    else
                    {
                        encStr = hexStr;
                    }
                }
                else
                {
                    encStr = str64;
                }

                return encStr;
            }
            catch (Exception e)
            {
                this.health = false;
                this.memo = e.Message;
                return "";
            }
        }

        /// <summary>
        /// Decode ....
        /// </summary>
        /// <param name="stringToDecrypt"></param>
        /// <param name="sEncryptionKey"></param>
        /// <returns></returns>
        public string Decrypt64(string stringToDecrypt, string sEncryptionKey, bool hex)
        {
            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            string decStr;
            string str64;
            string unhexStr;
            bool dataOK;

            // if the string is already hexed unhex it.
            if (!hex)
            {
                str64 = stringToDecrypt;
            }
            else
            {
                dataOK = ALCS_Conversion.HexToString(stringToDecrypt, ALCS_Conversion.HexForm.x2, out unhexStr);

                if (!dataOK)
                {
                    decStr = "";
                    this.health = false;
                    this.memo = "The string is not in a valid hex form.";
                    return decStr;
                }
                else
                {
                    str64 = unhexStr;
                }
            }

            // Now the decryprion routine.
            try
            {
                // Qualify the Key. (i.e.   )
                key = QualifyKey(sEncryptionKey);

                // Decryption ... Decruption ...
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();
                inputByteArray = Convert.FromBase64String(str64);
                MemoryStream ms = new MemoryStream();
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);
                cs.FlushFinalBlock();
                System.Text.Encoding encoding = System.Text.Encoding.UTF8;
                decStr = encoding.GetString(ms.ToArray());
                return decStr;
            }
            catch (Exception e)
            {
                this.health = false;
                this.memo = e.Message;
                return "";
            }
        }

        /// <summary>
        /// Qulaify the key.  for 64 the key must be 8 bytes ...
        /// </summary>
        /// <returns></returns>
        private byte[] QualifyKey(string SEncryptionKey)
        {
            SEncryptionKey += "mustbeeight";
            return System.Text.Encoding.UTF8.GetBytes(SEncryptionKey.Substring(0, 8));
        }

        #endregion 

        #region "Expose the health and the message ..."

        // The health of the class.
        public bool isHealthy
        {
            get
            {
                return this.health;
            }
        }

        // The error message if any.
        public string Memo
        {
            get
            {
                return this.memo;
            }
        }

        #endregion 
    }

    #endregion 
}
