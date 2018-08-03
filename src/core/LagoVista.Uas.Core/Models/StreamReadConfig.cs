using LagoVista.Core.Models;
using LagoVista.Uas.Core.MavLink;

namespace LagoVista.Uas.Core.Models
{
    public class StreamReadConfig : ModelBase
    {
        MavDataStream _stream;
        public MavDataStream Stream
        {
            get { return _stream; }
            set{ Set(ref _stream, value); }
        }

        ushort _updateHz;
        public ushort UpdateHz
        {
            get { return _updateHz; }
            set { Set(ref _updateHz, value); }
        }
    }
}
