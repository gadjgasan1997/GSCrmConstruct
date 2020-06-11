using GSCrmLibrary.Data;
using GSCrmLibrary.Models.MainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace GSCrmLibrary.DataMapping
{
    public interface IDataMapping<TContext, TInData, TOutData>
        where TContext : MainContext
        where TInData : IDataEntity
        where TOutData : IDataEntity
    {
        IEnumerable<TOutData> Map(IEnumerable<TInData> records, TContext context);
    }
}
