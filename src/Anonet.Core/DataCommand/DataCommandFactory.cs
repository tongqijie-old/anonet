using System;
using System.Linq;
using System.Reflection;

namespace Anonet.Core
{
    class DataCommandFactory
    {
        public static IDataCommand Create(Datagram datagram)
        {
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
