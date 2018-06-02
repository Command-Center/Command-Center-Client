using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommandApplication.Model
{
    public interface Irepository
    {
        void AddAddress(ConnectionAddress conAddress);
        void DeleteAddress(ConnectionAddress conAddress);
        List<ConnectionAddress> ListAllAddresses();
    }
}
