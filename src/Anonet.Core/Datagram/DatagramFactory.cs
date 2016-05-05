using System;
using System.Linq;
using System.Reflection;

namespace Anonet.Core
{
    class DatagramFactory
    {
        private static uint _SerialNumber = 0;

        private static uint SerialNumber
        {
            get
            {
                return ++_SerialNumber;
            }
        }

        public enum DatagramFlag : byte
        {
            Request = 0x80,

            NeedResponse = 0x40,
        }

        public static Datagram CreateRequest(IDataCommandRequest dataCommandRequest)
        {
            return Datagram.Create((byte)dataCommandRequest.Id, dataCommandRequest.NeedResponse ? (byte)(DatagramFlag.Request | DatagramFlag.NeedResponse) : (byte)DatagramFlag.Request, SerialNumber, BinarySerializer.Serialize(dataCommandRequest.PayloadObject));
        }

        public static Datagram CreateResponse(IDataCommandResponse dataCommandResponse)
        {
            return Datagram.Create((byte)dataCommandResponse.Id, 0x00, SerialNumber, BinarySerializer.Serialize(dataCommandResponse.PayloadObject));
        }

        public static Datagram Create(IStreamBuffer streamBuffer)
        {
            var index = -1;
            while ((index = streamBuffer.IndexOf(Datagram.DatagramHeader)) >= 0)
            {
                streamBuffer.Seek(index);

                if (streamBuffer.Length <= 5)
                {
                    continue;
                }

                var header = new byte[5];
                streamBuffer.Read(header, 0, header.Length);

                
            }


            return null;
        }

        public static IDataCommand GetDataCommand(byte[] data)
        {
            var datagram = Datagram.Create(data);

            if ((datagram.Flag & (byte)DatagramFlag.Request) > 0)
            {
                return GetDataCommand<IDataCommandRequest>(datagram);
            }
            else
            {
                return GetDataCommand<IDataCommandResponse>(datagram);
            }
        }

        private static T GetDataCommand<T>(Datagram datagram) where T : class, IDataCommand
        {
            var dataCommandType = Assembly.GetExecutingAssembly().GetTypes().FirstOrDefault(x => x.GetCustomAttribute<DataCommandAttribute>(false) != null && (byte)(x.GetCustomAttribute<DataCommandAttribute>(false) as DataCommandAttribute).DataCommandIdentity == datagram.Command && x.GetInterface(typeof(T).Name) != null);
            if (dataCommandType == null)
            {
                return null;
            }

            if (datagram.Content == null || datagram.Content.Length == 0)
            {
                var dataCommand = Activator.CreateInstance(dataCommandType) as T;
                dataCommand.SerialNumber = datagram.SerialNumber;
                return dataCommand;
            }
            else
            {
                var dataCommand = Activator.CreateInstance(dataCommandType) as T;
                dataCommand.SerialNumber = datagram.SerialNumber;
                dataCommand.PayloadObject = BinarySerializer.Deserialize(datagram.Content, dataCommandType.GetCustomAttribute<DataCommandAttribute>().EntityType);
                return dataCommand;
            }
        }
    }
}
