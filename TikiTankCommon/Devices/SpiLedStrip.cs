using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Drawing;
using System.IO;
using System;

namespace TikiTankCommon.Devices
{
    [DataContract]
    public class SpiLedStrip : IShowable, IDisposable
    {
	    public SpiLedStrip()
	    {
		    Pixels = 1;
	    }

	    public bool Init()
	    {
		    _fstream = File.OpenWrite(Filename);
		    if( null == _fstream )
			    return false;

            _latchBytes = new byte[((this.Pixels + 31) / 32)];
            Array.Clear(_latchBytes, 0, _latchBytes.Length);

		    _fstream.WriteByte(0x00);
            _fstream.Write(_latchBytes, 0, _latchBytes.Length);

		    _fstream.Flush();
		    return true;
	    }

	    public void Dispose()
        {
		    _fstream.Close();
	    }

	    public void Show(Color[] pixels)
	    {
		    System.Diagnostics.Debug.Assert(pixels.Length == Pixels);

		    foreach( Color c in pixels )
		    {
			    _fstream.WriteByte(gamma[c.G]);
			    _fstream.WriteByte(gamma[c.R]);
			    _fstream.WriteByte(gamma[c.B]);
		    }

            _fstream.Write(_latchBytes, 0, _latchBytes.Length);

		    _fstream.Flush();
	    }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Pixels { get; private set; }

        [DataMember]
        public string Filename { get; set; }

        private byte[] _latchBytes;
	    private FileStream _fstream;

	    byte[] gamma = new byte[256] 
	    {	
		    // from math found at 
		    // http://learn.adafruit.com/light-painting-with-raspberry-pi
		    128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
		    128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
		    128, 128, 128, 128, 128, 128, 128, 128, 128, 128,
		    128, 128, 128, 128, 128, 128, 129, 129, 129, 129,
		    129, 129, 129, 129, 129, 129, 129, 129, 130, 130,
		    130, 130, 130, 130, 130, 130, 131, 131, 131, 131,
		    131, 131, 131, 132, 132, 132, 132, 132, 132, 133,
		    133, 133, 133, 133, 133, 134, 134, 134, 134, 135,
		    135, 135, 135, 135, 136, 136, 136, 136, 137, 137,
		    137, 137, 138, 138, 138, 139, 139, 139, 139, 140,
		    140, 140, 141, 141, 141, 142, 142, 142, 143, 143,
		    143, 144, 144, 144, 145, 145, 146, 146, 146, 147,
		    147, 148, 148, 148, 149, 149, 150, 150, 151, 151,
		    152, 152, 152, 153, 153, 154, 154, 155, 155, 156,
		    156, 157, 157, 158, 158, 159, 160, 160, 161, 161,
		    162, 162, 163, 163, 164, 165, 165, 166, 166, 167,
		    168, 168, 169, 170, 170, 171, 172, 172, 173, 174,
		    174, 175, 176, 176, 177, 178, 178, 179, 180, 181,
		    181, 182, 183, 184, 184, 185, 186, 187, 188, 188,
		    189, 190, 191, 192, 192, 193, 194, 195, 196, 197,
		    198, 198, 199, 200, 201, 202, 203, 204, 205, 206,
		    207, 208, 208, 209, 210, 211, 212, 213, 214, 215,
		    216, 217, 218, 219, 220, 221, 222, 224, 225, 226,
		    227, 228, 229, 230, 231, 232, 233, 234, 236, 237,
		    238, 239, 240, 241, 242, 244, 245, 246, 247, 248,
		    250, 251, 252, 253, 254, 255
	    };
    }
}