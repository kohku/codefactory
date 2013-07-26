using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace CodeFactory.DataAccess.Mapping
{
    public interface IDatabaseMapped
    {
        void GetObjectData(DataTable context);
    }
}
