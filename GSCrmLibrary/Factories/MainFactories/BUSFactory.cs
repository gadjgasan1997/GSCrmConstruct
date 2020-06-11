using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GSCrmLibrary.Data;
using GSCrmLibrary.Models.RequestModels;
using GSCrmLibrary.Models.TableModels;
using GSCrmLibrary.Services.Info;

namespace GSCrmLibrary.Factories.MainFactories
{
    public class BUSFactory<TContext>
        where TContext : MainContext
    {
        public virtual dynamic GetRecord(Type type, TContext context, IViewInfo viewInfo, BusinessComponent busComp)
        {
            return type.InvokeMember("GetComponentRecord",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                default,
                type.GetConstructors().LastOrDefault().Invoke(new object[] {
                    context, viewInfo
                }),
                new object[] { busComp.Name });
        }
        public virtual dynamic GetRecord(Type type, TContext context, IViewInfo viewInfo, BusinessComponent busComp, string propertyName, string propertyValue)
        {
            return type.InvokeMember("GetComponentRecord",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                default,
                type.GetConstructors().LastOrDefault().Invoke(new object[] {
                    context, viewInfo
                }),
                new object[] { propertyName, propertyValue });
        }
        public virtual void SetRecord(Type type, TContext context, IViewInfo viewInfo, BusinessComponent busComp, dynamic record)
        {
            type.InvokeMember("SetComponentRecord",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                default,
                type.GetConstructors().LastOrDefault().Invoke(new object[] {
                    context, viewInfo
                }),
                new object[] { busComp.Name, record });
        }
        public virtual void InitializeComponentsRecords(Type type, TContext context, IViewInfo viewInfo, FilterEntitiesModel model)
        {
            List<string> dataEntities = type.InvokeMember("FilterEntitiesGetId",
                BindingFlags.Public | BindingFlags.Instance | BindingFlags.InvokeMethod,
                default,
                type.GetConstructors().LastOrDefault().Invoke(new object[] {
                    context, viewInfo
                }),
                new object[] { model }) as List<string>;
            ComponentsRecordsInfo.SetDisplayedRecords(model.BusComp.Name, dataEntities);
            if (ComponentsRecordsInfo.GetSelectedRecord(model.BusComp.Name) == null)
                ComponentsRecordsInfo.SetSelectedRecord(model.BusComp.Name, dataEntities.FirstOrDefault());
        }
    }
}
