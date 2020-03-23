using GSCrm.Models.Default.TableModels;
using GSCrm.Services.Info;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GSCrm.Data.Context
{
    public static class ComponentContext
    {
        #region Свойства
        private static Dictionary<string, string> selectedRecords { get; set; }
        private static Dictionary<string, List<string>> displayedRecords { get; set; }
        #endregion

        #region Методы
        // Получение выбранных записей
        public static Dictionary<string, string> GetSelectedRecords()
        {
            if (selectedRecords == null)
                return new Dictionary<string, string>();
            else return selectedRecords;
        }
        // Получение выбранной записи
        public static string GetSelectedRecord(string componentName)
        {
            if (selectedRecords == null)
                return null;
            else return selectedRecords.GetValueOrDefault(componentName);
        }
        // Установка выбранной записи
        public static void SetSelectedRecord(string componentName, string recordId)
        {
            if (selectedRecords == null)
                selectedRecords = new Dictionary<string, string>();

            if (!selectedRecords.ContainsKey(componentName))
                selectedRecords.Add(componentName, recordId);
            else selectedRecords[componentName] = recordId;
        }
        // Получение отоброжаемых записей
        public static List<string> GetDisplayedRecords(string componentName)
        {
            if (displayedRecords == null)
                return new List<string>();
            else
            {
                List<string> records = displayedRecords
                    .GetValueOrDefault(componentName)
                    ?.Where(val => val != null)
                    ?.ToList();
                return records;
            }
        }
        // Установка отоброжаемых записей
        public static void SetDisplayedRecords(string componentName, List<string> recordsId)
        {
            if (displayedRecords == null)
                displayedRecords = new Dictionary<string, List<string>>();

            List<string> records = recordsId.Where(val => val != null).ToList();

            if (!displayedRecords.ContainsKey(componentName))
                displayedRecords.Add(componentName, records);
            else displayedRecords[componentName] = records;
        }
        // Возвращает текущие отображаемые записи для апплетов
        public static Dictionary<string, string> GetUIRecords(ApplicationContext context, IViewInfo viewInfo)
        {
            Dictionary<string, string> UIRecords = new Dictionary<string, string>();
            selectedRecords.ToList().ForEach(record =>
            {
                BusComp busComp = context.BusinessComponents.FirstOrDefault(n => n.Name == record.Key);
                Applet applet = viewInfo.ViewApplets.FirstOrDefault(i => i.BusCompId == busComp.Id);
                if (applet != null)
                    UIRecords.Add(applet.Name, record.Value);
            });
            return UIRecords;
        }
        // Очистка информации о старых отображаемых записях
        public static void ClearOldRecords(List<BusComp> busComps)
        {
            List<string> recordsToRemove = new List<string>();

            // Названия новых отображаемых бизнес компонент
            List<string> busCompsNames = busComps.Select(n => n.Name).ToList();

            // Получение старых бизнес компонент, подлежащих удалению
            displayedRecords?.Keys
                .ToList()
                .ForEach(bc =>
                {
                    if (busCompsNames.IndexOf(bc) == -1)
                        recordsToRemove.Add(bc);
                });

            foreach (string name in recordsToRemove)
                displayedRecords.Remove(name);
        }
        // Очистка информации
        public static void Dispose()
        {
            selectedRecords = null;
            displayedRecords = null;
        }
        #endregion
    }
}
