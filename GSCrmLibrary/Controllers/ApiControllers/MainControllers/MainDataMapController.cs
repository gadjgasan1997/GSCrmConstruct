using System.Linq;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using GSCrmLibrary.Data;
using GSCrmLibrary.DataMapping;
using GSCrmLibrary.Models.MainEntities;
using Microsoft.EntityFrameworkCore;
using System;
using GSCrmLibrary.Factories.MainFactories;

namespace GSCrmLibrary.Controllers.ApiControllers.MainControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MainDataMapController<TInData, TOutData, TDataMapping, TBusinessComponent, TDataBUSFactory, TContext> : ControllerBase
        where TInData : class, IDataEntity, new()
        where TOutData : class, IDataEntity, new()
        where TDataMapping : IDataMapping<TContext, TInData, TOutData>, new()
        where TBusinessComponent : IBUSEntity, new()
        where TDataBUSFactory : IDataBUSFactory<TOutData, TBusinessComponent, TContext>, new()
        where TContext : MainContext
    {
        private readonly TContext context;
        private readonly string componentName;
        private readonly DbSet<TInData> inEntityDBSet;
        private readonly DbSet<TOutData> outEntityDBSet;
        public MainDataMapController(TContext context, string componentName)
        {
            this.context = context;
            this.componentName = componentName;
            this.context.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
            inEntityDBSet = context.Set<TInData>();
            outEntityDBSet = context.Set<TOutData>();
        }

        [HttpGet("CopyRecord")]
        public virtual Dictionary<string, object> CopyRecord()
        {
            TDataMapping dataMapping = new TDataMapping();
            TOutData newRecord = new TOutData();
            Dictionary<string, object> result = new Dictionary<string, object>();
            try
            {
                TDataBUSFactory dataBUSFactory = new TDataBUSFactory();
                newRecord = dataMapping.Map(new List<TInData>() {
                    inEntityDBSet.AsNoTracking().FirstOrDefault(i => i.Id.ToString() == ComponentsRecordsInfo.GetSelectedRecord(componentName))
                }, context).FirstOrDefault();
                outEntityDBSet.AsNoTracking().ToList().Add(newRecord);
                context.Entry(newRecord).State = EntityState.Added;
                context.SaveChanges();
                ComponentsContext<TBusinessComponent>.SetComponentContext(componentName, dataBUSFactory.DataToBusiness(newRecord, context));
                ComponentsRecordsInfo.SetSelectedRecord(componentName, newRecord.Id.ToString());
                List<string> currentDisplayedRecords = ComponentsRecordsInfo.GetDisplayedRecords(componentName);
                ComponentsRecordsInfo.SetDisplayedRecords(componentName, new List<string>() { newRecord.Id.ToString() }.Concat(currentDisplayedRecords).ToList());
                result["ErrorMessages"] = null;
                result["NewRecord"] = newRecord;
            }
            catch(Exception ex)
            {
                result["ErrorMessages"] = Utils.GetErrorsInfo(ex);
                result["NewRecord"] = null;
                return result;
            }
            return result;
        }
    }
}
